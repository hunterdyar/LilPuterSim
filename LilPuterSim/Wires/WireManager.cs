namespace LilPuter;

//This will be the main reason that this app is not multi-threadable.

public class WireManager
{
	private HashSet<Pin> _allPins;
	private readonly Dictionary<Pin, Pin[]> _connections = new Dictionary<Pin, Pin[]>();
	private readonly Dictionary<Pin, Action<Pin>> _onValueChangeMap = new Dictionary<Pin, Action<Pin>>(); 
	private readonly Dictionary<Pin, Action<Pin>> _changeMap = new Dictionary<Pin, Action<Pin>>();
	private readonly Queue<Pin> _changeQueue;
	private int _maxQueueCount = 10;

	//topo sort things
	private List<Pin> _tSort = new List<Pin>();
	private Dictionary<Pin, int> _inDegree = new Dictionary<Pin, int>();
	private bool _tSortDirty = true;
	
	//impulse things
	//stores if a pin has been set or updated, and thus needs to be impulsed.
	private readonly Dictionary<Pin, bool> _needsImpulse = new Dictionary<Pin, bool>();

	
	public WireManager()
	{
		_allPins = new HashSet<Pin>();
		_changeQueue = new Queue<Pin>();
	}

	public delegate void PinChangedHandler(Pin pin);

	/// <summary>
	/// Sets a Pin and calls an impulse. This is what we want to call from external systems.
	/// </summary>
	public void SetPin(Pin pin, WireSignal signal)
	{
		pin.Set(signal);
		Impulse(pin);
	}

	public void SetPin(Pin pin, byte[] signal)
	{
		pin.Set(signal);
		Impulse(pin);
	}

	private void CalculateInDegrees()
	{
		_inDegree.Clear();
		foreach (var pin in _allPins)
		{
			_inDegree.TryAdd(pin, 0);
			if (_connections.TryGetValue(pin, out var toPins))
			{
				foreach (var to in toPins)
				{
					if (!_inDegree.TryAdd(to, 1))
					{
						_inDegree[to]++;
					}
					else
					{
						_inDegree[to] = 1;
					}
				}
			}
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
			
		}
		
		return _result;
	}
	public void Impulse(Pin pin)
	{ 
		//todo: change this to getting the topoSort, getting the index of pin, and impulsing from that point and to the end.
		
		
		//_needsImpulse[pin] = false;
		//update the systems that use this pin directly.
		//This is basically only NAND gates in most cases! Neat!
		
		if (_onValueChangeMap.TryGetValue(pin, out var action))
		{
			Console.WriteLine(action.Method.DeclaringType.Name+" - "+action.Method.Name);
			//the dictionary should store some sort of SystemID (just use the pointer; class reference?)
			//Add it to a secondary breadth-first queue?
			//todo: add these to some kind of hashset, and then run when we finish the pin propogation - which will add more pin propogate..s but also prevent changing a lot of pins to have a system keep re-triggering.
			action?.Invoke(pin);
		}
		Console.WriteLine($"just called changes for impulse on {pin.Name}. Now calling impulses for all connected pins.");
		
		//update all children
		if (_connections.TryGetValue(pin, out var connection))
		{
			foreach (var connectedPin in connection)
			{
				connectedPin.Set(pin.Value);
			}
		}
		

		//Propogate through all lowest level ones first. Then the higher ones.
		while (_changeQueue.Count > 0)
		{
			var p = _changeQueue.Dequeue();
			if (pin == p)
			{
				//we are already impulsing this one. Is this an infinite loop, or is this just from us calling "SetPin"?
				continue;
			}

			Impulse(p);
		}
		
		//set pin and collect all pins that update in response to it. Repeat though the queue until propogation is complete.
		//use a hashmap of updated pins to prevent infinite loops (for things like buses), if neccesary?
		
	}
	
	public void Connect(Pin from, Pin to, bool biDirectional = false)
	{
		_tSortDirty = true;
		_allPins.Add(from);
		_allPins.Add(to);
		//Create connection wire.
		if (_connections.ContainsKey(from))
		{
			if (!_connections[from].Contains(to))
			{
				var c = _connections[from];
				Array.Resize(ref c, _connections[from].Length + 1);
				c[^1] = to;
				_connections[from] = c;

				if (biDirectional)
				{
					throw new NotImplementedException("Bidirectional not yet implemented");
				}
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
	}
	
	/// <summary>
	/// Called by pins in set, which is called by the OnValueChange functions we register that do the logic.
	/// </summary>
	public void Changed(Pin pin, byte[] value)
	{
		if (!_changeQueue.Contains(pin))
		{
			_changeQueue.Enqueue(pin);
		}
		_needsImpulse[pin] = true;
	}

	public void Listen(Pin p, Action<Pin> handler)
	{
		if(!_onValueChangeMap.TryAdd(p, handler))
		{
			_onValueChangeMap[p] += handler;
		}
	}
}