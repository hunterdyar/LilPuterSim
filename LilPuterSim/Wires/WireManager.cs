namespace LilPuter;

//This will be the main reason that this app is not multi-threadable.

public class WireManager
{
	private readonly Dictionary<Pin, Pin[]> _connections = new Dictionary<Pin, Pin[]>();
	private readonly Dictionary<Pin, Action<Pin>> _onValueChangeMap = new Dictionary<Pin, Action<Pin>>(); 
	private readonly List<Queue<Pin>> _changeQueues;
	private int _maxQueueCount = 10;
	private int _maxPinWeightInGraph = 0;
	public WireManager()
	{
		_changeQueues = new List<Queue<Pin>>();
		for (var i = 0; i < _maxQueueCount; i++)
		{
			_changeQueues.Add(new Queue<Pin>());
		}
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

	public void Impulse(Pin pin)
	{
		//Send out the first impulse, collecting the rest along the way. This will complete all pins of weight 0.
		ImpulseRecursive(pin, 0);
		for (int i = 1; i < _maxPinWeightInGraph; i++)
		{
			ImpulseRecursive(pin, i);
		}
	}
	public void ImpulseRecursive(Pin pin, int pinWeight)
	{ 
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
				if (connectedPin.PinWeight == pinWeight)
				{
					connectedPin.Set(pin.Value);
				}
				else
				{
					_changeQueues[connectedPin.PinWeight].Enqueue(connectedPin);
				}
			}
		}
		

		while (_changeQueues[pinWeight].Count > 0)
		{
			var p = _changeQueues[pinWeight].Dequeue();
			if (pin == p)
			{
				//we are already impulsing this one. Is this an infinite loop, or is this just from us calling "SetPin"?
				continue;
			}

			ImpulseRecursive(p, pinWeight);
		}
		
		//set pin and collect all pins that update in response to it. Repeat though the queue until propogation is complete.
		//use a hashmap of updated pins to prevent infinite loops (for things like buses), if neccesary?
		
	}
	
	public void Connect(Pin from, Pin to, bool biDirectional = false)
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
		//todo: This can be a configuration check done once for the whole system, not a runtime check!
		if (pin.PinWeight > _maxPinWeightInGraph)
		{
			_maxPinWeightInGraph = pin.PinWeight;

			if (pin.PinWeight >= _maxQueueCount)
			{
				int diff = pin.PinWeight - _maxQueueCount + 1;
				_maxQueueCount += diff;
				for (int i = 0; i < diff; i++)
				{
					_changeQueues.Add(new Queue<Pin>());
				}
			}
		}
		
		if (!_changeQueues[pin.PinWeight].Contains(pin))
		{
			_changeQueues[pin.PinWeight].Enqueue(pin);
		}
	}

	public void Listen(Pin p, Action<Pin> handler)
	{
		if(!_onValueChangeMap.TryAdd(p, handler))
		{
			_onValueChangeMap[p] += handler;
		}
	}
}