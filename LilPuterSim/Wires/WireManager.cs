namespace LilPuter;

//This will be the main reason that this app is not multi-threadable.

public class WireManager
{
	private HashSet<ISystem> _allPins = new HashSet<ISystem>();
	private readonly Dictionary<ISystem, List<ISystem>> _dependencies = new Dictionary<ISystem, List<ISystem>>();
	private readonly Dictionary<ISystem, Action<ISystem>> _simActionMap = new Dictionary<ISystem, Action<ISystem>>(); 

	//topo sort things
	private List<ISystem> _tSort = new List<ISystem>();
	private Dictionary<ISystem, int> _inDegree = new Dictionary<ISystem, int>();
	private bool _tSortDirty = true;
	
	//impulse things
	//stores if a pin has been set or updated, and thus needs to be impulsed.
	private readonly Dictionary<ISystem, bool> _needsTick = new Dictionary<ISystem, bool>();
	

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
			var deps = _dependencies.Count(x => x.Value.Contains(pin));
			_inDegree[pin] += deps;
		}
	}

	public List<ISystem> GetTopoSort()
	{
		if (!_tSortDirty)
		{
			return _tSort;
		}
		_tSort.Clear();
		CalculateInDegrees();
		var s = _inDegree.Where(x=>x.Value == 0).Select(x=>x.Key).ToList();
		Queue<ISystem> q = new Queue<ISystem>(s);

		while (q.Count > 0)
		{
			var p = q.Dequeue();
			_tSort.Add(p);
			
			if (_dependencies.TryGetValue(p, out var depSys))
			{
				foreach (var toSys in depSys)
				{
					_inDegree[toSys]--;
					if (_inDegree[toSys] == 0)
					{
						q.Enqueue(toSys);
					}
				}
			}
			//dependencies aren't getting included. We need their reverse as well?
		}

		_tSortDirty = false;
		return _tSort;
	}

	public void Tick(ISystem system)
	{
		if (!_needsTick.ContainsKey(system))
		{
			return;
		}
		//not set from anything, so we're good!

		if (_simActionMap.TryGetValue(system, out var simAction))
		{
			simAction?.Invoke(system);
			_needsTick[system] = false;
		}
	}
	public void Impulse(ISystem system)
	{ 
		//todo: change this to getting the topoSort, getting the index of pin, and impulsing from that point and to the end.
		var topo = GetTopoSort();
		var indexOfPin = topo.IndexOf(system);
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
			Tick(system);
		}
		
		//set pin and collect all pins that update in response to it. Repeat though the queue until propogation is complete.
		//use a hashmap of updated pins to prevent infinite loops (for things like buses), if neccesary?
		
	}
	
	public void ConnectPins(Pin from, Pin to)
	{
		//todo: I am not sure how many of these dictionaries we need. Still tinkering as I make it.
		
		//initialize ourselves, we need to tick since we have a new incoming connection.

		if (from == to)
		{
			throw new Exception("Cannot connect a system to itself.");
		}
		
		SetDependency(to,from);
		if (_simActionMap.ContainsKey(from))
		{
			_simActionMap[from] += f => to.Set(((Pin)f).Value);
		}
		else
		{
			_simActionMap.Add(from, f => to.Set(((Pin)f).Value));
		}
	}

	public void SetDependency(ISystem from, ISystem to)
	{
		_tSortDirty = true;
		_allPins.Add(from);
		_allPins.Add(to);

		if (!_needsTick.TryAdd(to, true))
		{
			_needsTick[to] = true;
		}

		if (!_needsTick.TryAdd(from, true))
		{
			_needsTick[from] = true;
		}
		
		if (!_dependencies.TryAdd(to, [from]))
		{
			_dependencies[to].Add(from);
		}
	}

	public void RegisterSystemAction(ISystem system, Action<ISystem> handler)
	{
		if(!_simActionMap.TryAdd(system, handler))
		{
			_simActionMap[system] += handler;
		}
	}

	/// <summary>
	/// Marks the outins as dependent on the inpins, and adds ActionCall to when inPin changes.
	/// </summary>
	public void RegisterSystem(List<ISystem> ins, Action<ISystem> actionCall, List<ISystem> outs)
	{
		foreach (var inpIn in ins)
		{
			if (_simActionMap.ContainsKey(inpIn))
			{
				_simActionMap[inpIn] += actionCall;
			}
			else
			{
				_simActionMap.Add(inpIn, actionCall);
			}
			foreach (var outpin in outs)
			{
				SetDependency(inpIn,outpin);
			}
		}
	}

	public void Changed(ISystem system, byte[] value)
	{
		//todo: skip propogation
		//_needsTick[system] = false;
	}
}