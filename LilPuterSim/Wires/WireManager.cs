namespace LilPuter;

//This will be the main reason that this app is not multi-threadable.

public class WireManager
{
	private readonly Dictionary<Pin, Pin[]> _connections = new Dictionary<Pin, Pin[]>();
	private readonly Dictionary<Pin, Action<Pin>> _onValueChangeMap = new Dictionary<Pin, Action<Pin>>(); 
	private readonly Queue<Pin> _changeQueue;
	private int _maxQueueCount = 10;

	public WireManager()
	{
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
	public void Impulse(Pin pin)
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
	}

	public void Listen(Pin p, Action<Pin> handler)
	{
		if(!_onValueChangeMap.TryAdd(p, handler))
		{
			_onValueChangeMap[p] += handler;
		}
	}
}