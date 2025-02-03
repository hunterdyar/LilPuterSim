namespace LilPuter;

//todo: Create a GUID on creation for passing around serialized forms.

/// <summary>
/// A pin is a wire (or set of wires) that can be set. It holds a byte[] value (like everything else).
/// A "normal singular pin" is an array of 1 byte.
/// A bank of 8 or 16 pins, like inputs into an 8 or 16 bit adder, would hold 8 values.
/// This reduces the number of calls to the wire manager and number of connections for what is conceptually a "single" connection.
///	e.g. conceptually... and actually in this sim.
/// </summary>
public class Pin : IObservable, ISystem
{
	public string Name { get; private set; }
	public byte[] Value { get; private set; } = [(byte)WireSignal.Floating];//Default should be not connected == floating
	public WireSignal Signal => (WireSignal)Value[0];
	public int DataCount => Value.Length;
	public PinType PinType => PinUtility.GetPinType(this);
	private readonly WireManager _manager;
	
	private readonly List<IObservable.OnValueChangeDelegate> _subscribers = [];
	public int SubscriberCount() => _subscribers.Count;
	public Pin(WireManager manager, string name)
	{
		this._manager = manager;
		this.Name = name;
	}

	public Pin(WireManager manager, string name, int bitWidth)
	{
		this._manager = manager;
		this.Name = name;
		Value = new byte[bitWidth]; //Default should be not connected == floating
		for (int i = 0; i < bitWidth; i++)
		{
			Value[i] = (byte)WireSignal.Floating;
		}
	}

	public void Subscribe(IObservable.OnValueChangeDelegate subscriber)
	{
		_subscribers.Add(subscriber);
	}

	public void Unubscribe(IObservable.OnValueChangeDelegate subscriber)
	{
		if (_subscribers.Contains(subscriber))
		{
			_subscribers.Remove(subscriber);
		}
		else
		{
			throw new Exception("Subscriber not found. Can't Remove.");
		}
	}

	public byte[] ReadValue()
	{
		return Value;
	}

	/// <summary>
	/// For use by parent systems that will manually propogate or call impulse.
	/// </summary>
	public void SetSilently(byte[] value)
	{
		Value = value;
	}

	public void SetSilently(WireSignal value)
	{
		Value[0] = (byte)value;
	}
	public bool Set(byte[] value, bool alwaysUpdate = false)
	{
		bool changed = false;
		if (value.Length != Value.Length)
		{
			throw new ArgumentException("Value for pin must match pin length.");
		}

		if (value.Length == 1)
		{
			// if (value[0] == 2)
			// {
			//		//this is allowed, just testing things.
			//		throw new Exception("Setting Value to Floating?");
			// }
			var newVal = (WireSignal)value[0];
			changed = newVal != Signal;
			if (changed || alwaysUpdate)
			{
				Value = [(byte)newVal];
				_manager.Changed(this, Value);
				UpdateSubscribers();
			}

			return changed;
		}
		else
		{
			int c = 0;
			for (int i = 0; i < Value.Length; i++)
			{
				if (Value[i] != value[i])
				{
					Value[i] = value[i];
					c++;
				}
			}

			changed = c > 0;
		}

		if (changed || alwaysUpdate)
		{
			_manager.Changed(this, Value);
			UpdateSubscribers();
		}

		return changed;
	}

	private void UpdateSubscribers()
	{
		foreach (var onChange in _subscribers)
		{
			onChange(Value);
		}
	}

	internal bool Set(WireSignal newVal, bool alwaysUpdate = false)
	{
		return Set([(byte)newVal], alwaysUpdate);
	}

	/// <summary>
	/// Unlike real wires, connections are 1-way unless we explicitly state otherwise.
	/// This function is not memory efficient, but we optimize for the runtime, not configuration time. Connections should rarely change during runtime (use a switch component).
	/// </summary>
	/// <param name="otherPin">The pin to connect a wire to.</param>
	public void ConnectTo(Pin otherPin)
	{
		_manager.ConnectPins(this, otherPin);
	}

	public void DisconnectFrom(Pin otherPin, bool twoWay)
	{
		throw new NotImplementedException("Not Implemented. Pins should not be disconnected during runtime. Just configure it correctly lol?");
	}

	public void SetName(string n)
	{
		Name = n;
	}
	public override string ToString()
	{
		if (PinType == PinType.Single)
		{
			return $"Pin {Name} ({Signal})";
		}
		else
		{
			return $"Pin {Name} ({PinUtility.ByteArrayToInt(Value)})";
		}
	}

	public void SetAndImpulse(WireSignal flip)
	{
		_manager.SetPin(this, flip);
	}

	/// <summary>
	/// Sets the pin values.
	/// </summary>
	public void ZeroOutSilent()
	{
		for (var i = 0; i < Value.Length; i++)
		{
			Value[i] = (byte)WireSignal.Low;
		}
	}
	/// <summary>
	/// Inverts the value.
	/// </summary>
	public void InvertSilent()
	{
		for (var i = 0; i < Value.Length; i++)
		{
			Value[i] = (byte)WireUtility.Invert((WireSignal)Value[i]);
		}
	}

	public bool SetBit(int i, byte b)
	{
		if (i < 0 || i >= Value.Length)
		{
			throw new ArgumentException("Index out of range for setting pin bit");
		}

		if (Value[i] == b)
		{
			return false;
		}
		else
		{
			Value[i] = b;
			return true;
		}
	}

	public void DependsOn(Pin pin)
	{
		_manager.SetDependency(pin,this);
	}

	public bool Enabled { get; }
}