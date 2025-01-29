namespace LilPuter;

//This will be the main reason that this app is not multi-threadable.

public class WireManager
{
	private readonly Dictionary<Pin, Pin[]> _connections = new Dictionary<Pin, Pin[]>();
	private readonly Dictionary<Pin, Action<Pin>> _onValueChangeMap = new Dictionary<Pin, Action<Pin>>(); 
	private readonly Queue<Pin> _changeQueue = new Queue<Pin>();
	public delegate void PinChangedHandler(Pin pin);

	public void SetPin(Pin pin, WireSignal signal)
	{
		pin.Set(signal);
		Impulse(pin);
	}
	public void Impulse(Pin pin)
	{
		//update the systems that use this pin directly.
		//This is basically only NAND gates in most cases! Neat!
		if (_onValueChangeMap.TryGetValue(pin, out var action))
		{
			action?.Invoke(pin);
		}
		
		//update all children
		if (_connections.TryGetValue(pin, out var connection))
		{
			foreach (var connectedPin in connection)
			{
				connectedPin.Set(pin.Value);
			}
		}
		
		//propogate the breadth-first queue. This doesn't check for loops yet.
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

	public void Flash()
	{
		//Impulse no pins. Run impulse but on the current state.
		//In most states, this should do nothing! So it's useful for testing if it did do something (which means back-propogation)
		//Used to clean initial state. This is like... power on.
		foreach (var pin in _connections.Keys)
		{
			Impulse(pin);
		}
	}

	/// <summary>
	/// Called by pins in set, which is called by the OnValueChange functions we register that do the logic.
	/// </summary>
	public void Changed(Pin pin, byte[] value)
	{
		_changeQueue.Enqueue(pin);
	}

	public void Listen(Pin p, Action<Pin> handler)
	{
		if(!_onValueChangeMap.TryAdd(p, handler))
		{
			_onValueChangeMap[p] += handler;
		}
	}
}