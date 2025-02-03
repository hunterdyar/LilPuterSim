namespace LilPuter;

//This will be the main reason that this app is not multi-threadable.

public class WireManager
{
	private HashSet<Pin> _allPins;
	private readonly Dictionary<Pin, Pin[]> _connections = new Dictionary<Pin, Pin[]>();
	private readonly Dictionary<Pin, Pin[]> _incomingConnections = new Dictionary<Pin, Pin[]>();
	private readonly Dictionary<Pin, List<Pin>> _dependencies = new Dictionary<Pin, List<Pin>>();

	private readonly Dictionary<Pin, Action<Pin>> _onValueChangeMap = new Dictionary<Pin, Action<Pin>>(); 
	private readonly Dictionary<Pin, Action<Pin>> _changeMap = new Dictionary<Pin, Action<Pin>>();
	private int _maxQueueCount = 10;

	//topo sort things
	private List<Pin> _tSort = new List<Pin>();
	private Dictionary<Pin, int> _inDegree = new Dictionary<Pin, int>();
	private bool _tSortDirty = true;
	
	//impulse things
	//stores if a pin has been set or updated, and thus needs to be impulsed.
	private readonly Dictionary<Pin, bool> _needsTick = new Dictionary<Pin, bool>();

	
	public WireManager()
	{
		_allPins = new HashSet<Pin>();
	}

	public delegate void PinChangedHandler(Pin pin);

	/// <summary>
	/// Sets a Pin and calls an impulse. This is what we want to call from external systems.
	/// </summary>
	public void SetPin(Pin pin, WireSignal signal)
	{
		_needsTick[pin] = pin.Set(signal);
		Impulse(pin);
	}

	public void SetPin(Pin pin, byte[] signal)
	{
		_needsTick[pin] = pin.Set(signal);
		Impulse(pin);
	}

	private void CalculateInDegrees()
	{
		_inDegree.Clear();
		foreach (var pin in _allPins)
		{
			_inDegree.TryAdd(pin, 0);
			if (_incomingConnections.TryGetValue(pin, out var toPins))
			{
				_inDegree[pin] += toPins.Length;
			}

			var deps = _dependencies.Count(x => x.Value.Contains(pin));
			_inDegree[pin] += deps;
			
		}
	}

	public List<Pin> GetTopoSort()
	{
		if (!_tSortDirty)
		{
			return _tSort;
		}
		var _result = new List<Pin>();
		CalculateInDegrees();
		var s = _inDegree.Where(x=>x.Value == 0).Select(x=>x.Key).ToList();
		Queue<Pin> q = new Queue<Pin>(s);

		while (q.Count > 0)
		{
			var p = q.Dequeue();
			_result.Add(p);
			if (_connections.TryGetValue(p, out var toPins))
			{
				foreach (var toPin in toPins)
				{
					_inDegree[toPin]--;
					if (_inDegree[toPin] == 0)
					{
						q.Enqueue(toPin);
					}
				}
			}

			if (_dependencies.TryGetValue(p, out var depPins))
			{
				foreach (var toPin in depPins)
				{
					_inDegree[toPin]--;
					if (_inDegree[toPin] == 0)
					{
						q.Enqueue(toPin);
					}
				}
			}
			//dependencies aren't getting included. We need their reverse as well?
		}
		
		return _result;
	}

	public void Tick(Pin pin)
	{
		if (!_needsTick.ContainsKey(pin))
		{
			return;
		}
		//not set from anything, so we're good!
		

		if (_incomingConnections.TryGetValue(pin, out var fPins))
		{
			foreach (var f in fPins)
			{
				//todo: didUpdateThisFrame
				if (_needsTick[f])
				{
					var changed = pin.Set(f.Value);
					_needsTick[pin] = false;
				}
			}
		}

		
		if (pin.PinType == PinType.Single)
		{
			if (_onValueChangeMap.TryGetValue(pin, out var action))
			{
				Console.WriteLine(action.Method.DeclaringType.Name + " - " + action.Method.Name);
				//the dictionary should store some sort of SystemID (just use the pointer; class reference?)
				//Add it to a secondary breadth-first queue?
				//todo: add these to some kind of hashset, and then run when we finish the pin propogation - which will add more pin propogate..s but also prevent changing a lot of pins to have a system keep re-triggering.
				action?.Invoke(pin);
			}

		}
		
	}
	public void Impulse(Pin pin)
	{ 
		//todo: change this to getting the topoSort, getting the index of pin, and impulsing from that point and to the end.
		var topo = GetTopoSort();
		var indexOfPin = topo.IndexOf(pin);
		if (indexOfPin != -1)
		{
			for (var i = indexOfPin; i < topo.Count; i++)
			{
				Tick(topo[i]);
			}
		}
		else
		{
			//only tick this pin... because of the OnValueChange map where others will update.
			Tick(pin);
		}
		
		//set pin and collect all pins that update in response to it. Repeat though the queue until propogation is complete.
		//use a hashmap of updated pins to prevent infinite loops (for things like buses), if neccesary?
		
	}
	
	/// <param name="disconnected">dependency only for graph. Update via listeners.</param>
	/// <exception cref="ArgumentException"></exception>
	public void Connect(Pin from, Pin to, bool disconnected = false)
	{
		//todo: I am not sure how many of these dictionaries we need. Still tinkering as I make it.
		_tSortDirty = true;
		_allPins.Add(from);
		_allPins.Add(to);
		
		//initialize ourselves, we need to tick since we have a new incoming connection.
		if (!_needsTick.TryAdd(to, true))
		{
			_needsTick[to] = true;
		}

		if (!_needsTick.TryAdd(from, true))
		{
			_needsTick[from] = true;
		}

		if (!disconnected)
		{
			//Create connection wire.
			if (_connections.ContainsKey(from))
			{
				if (!_connections[from].Contains(to))
				{
					var c = _connections[from];
					Array.Resize(ref c, _connections[from].Length + 1);
					c[^1] = to;
					_connections[from] = c;

				}
				else
				{
					throw new ArgumentException("Cannot connect twice");
				}
			}
			else
			{
				_connections.Add(from, [to]);
			}

			//now do it again! but the other way!
			if (_incomingConnections.ContainsKey(to))
			{
				if (!_incomingConnections[to].Contains(from))
				{
					var c = _incomingConnections[to];
					Array.Resize(ref c, _incomingConnections[to].Length + 1);
					c[^1] = from;
					_incomingConnections[to] = c;
				}
				else
				{
					throw new ArgumentException("Cannot connect twice");
				}
			}
			else
			{
				_incomingConnections.Add(to, [from]);
			}
		}
		else
		{
			if (_dependencies.ContainsKey(from))
			{
				if (!_dependencies[from].Contains(to))
				{
					_dependencies[from].Add(to);
				}
				else
				{
					throw new ArgumentException("Cannot connect twice");
				}
			}
			else
			{
				_dependencies.Add(from, [to]);
			}
		}
	}
	
	/// <summary>
	/// Called by pins in set, which is called by the OnValueChange functions we register that do the logic.
	/// </summary>
	public void Changed(Pin pin, byte[] value)
	{
		_needsTick[pin] = true;
	}

	public void Listen(Pin p, Action<Pin> handler)
	{
		if(!_onValueChangeMap.TryAdd(p, handler))
		{
			_onValueChangeMap[p] += handler;
		}
	}
}