namespace LilPuter;

//This will be the main reason that this app is not multi-threadable.

public class WireManager
{
	private HashSet<Pin> _allPins;
	private readonly Dictionary<Pin, List<Pin>> _dependencies = new Dictionary<Pin, List<Pin>>();
	private readonly Dictionary<Pin, Action<Pin>> _simActionMap = new Dictionary<Pin, Action<Pin>>(); 

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

		if (_simActionMap.TryGetValue(pin, out var simAction))
		{
			simAction?.Invoke(pin);
			_needsTick[pin] = false;
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
	public void Connect(Pin from, Pin to)
	{
		//todo: I am not sure how many of these dictionaries we need. Still tinkering as I make it.
		
		//initialize ourselves, we need to tick since we have a new incoming connection.
		
		SetDependency(from,to);
		if (_simActionMap.ContainsKey(from))
		{
			_simActionMap[from] += f => to.Set(f.Value);
		}
		else
		{
			_simActionMap.Add(from, f => to.Set(f.Value));
		}
	}

	public void SetDependency(Pin from, Pin to)
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
	/// <summary>
	/// Called by pins in set, which is called by the OnValueChange functions we register that do the logic.
	/// </summary>
	public void Changed(Pin pin, byte[] value)
	{
		_needsTick[pin] = true;
	}

	public void Listen(Pin p, Action<Pin> handler)
	{
		if(!_simActionMap.TryAdd(p, handler))
		{
			_simActionMap[p] += handler;
		}
	}

	/// <summary>
	/// Marks the outins as dependent on the inpins, and adds ActionCall to when inPin changes.
	/// </summary>
	public void RegisterSystem(List<Pin> ins, Action<Pin> actionCall, List<Pin> outs)
	{
		foreach (var inpIn in ins)
		{
			_simActionMap[inpIn] += actionCall;
			foreach (var outpin in outs)
			{
				SetDependency(inpIn,outpin);
			}
		}
	}
}