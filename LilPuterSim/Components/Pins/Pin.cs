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
	private static readonly int[] FloatingVal = [1, 2, 4, 8, 16, 32, 64, 128, 256, 512, 1024,2048,4096,8192,16384, 32768, 65536];
	public string Name { get; set; }
	public int Value { get; private set; } //Default should be not connected == floating
	public WireSignal Signal => (WireSignal)Value;
	public readonly int Width = 1;
	public PinType PinType => PinUtility.GetPinType(this);
	private readonly WireManager _manager;
	
	private readonly List<IObservable.OnValueChangeDelegate> _subscribers = [];
	public int SubscriberCount() => _subscribers.Count;

	private readonly int _validMask;

	public Pin(WireManager manager, string name, int bitWidth = 1)
	{
		Enabled = true;
		this._manager = manager;
		this.Name = name;
		this.Width = bitWidth;
		Value = (int)Math.Pow(2, bitWidth);//8 bit can't store the value 256, it can store 0-255. A one bit value, floating is 2.

		//TODO: Move these to static constants.
		for (int i = 0; i < bitWidth; i++)
		{
			_validMask = _validMask | (1 << i);
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

	public int ReadValue()
	{
		return Value;
	}

	public WireSignal ReadPin(int pin)
	{
		return (WireSignal)((Value >> pin) & 1);
	}

	/// <summary>
	/// For use by parent systems that will manually propogate or call impulse.
	/// </summary>
	public void SetSilently(int value)
	{
		Value = value;
	}

	public void SetSilently(WireSignal value)
	{
		Value = (int)value;
	}
	/// <summary>
	/// This should only be called by systems downstream of a Tick. e.g. in a system action. Otherwise call SetPin on the wiremanager (or SetAndImpulse) for the "public" api.
	/// </summary>
	/// <param name="value"></param>
	/// <param name="alwaysUpdate"></param>
	/// <returns></returns>
	/// <exception cref="ArgumentException"></exception>
	internal bool Set(int value, bool alwaysUpdate = false)
	{
		bool changed = false;
		
		// if (value[0] == 2)
		// {
		//		//this is allowed, just testing things.
		//		throw new Exception("Setting Value to Floating?");
		// }
		var newVal = value;
		changed = newVal != Value;
		if (changed || alwaysUpdate)//todo: Investigate adding a check to not propagate floating values. (floating is init only)
		{
			Value = newVal;
			_manager.Changed(this, Value);
			UpdateSubscribers();
		}

		return changed;

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
		return Set((int)newVal, alwaysUpdate);
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
			return $"Pin {Name} ({Convert.ToString(Value,2)})";
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
		Value = 0;
	}
	/// <summary>
	/// Inverts the value.
	/// </summary>
	public void InvertSilent()
	{
		Value = ~Value;
	}

	public bool SetBit(int i, WireSignal b)
	{
		if (b == WireSignal.Floating)
		{
			return false;
		}
		
		if (i < 0 || i >= Width)
		{
			throw new ArgumentException("Index out of range for setting pin bit");
		}

		var current = (WireSignal)((Value >> i) & 1);
		if (current == b)
		{
			return false;
		}
		else
		{
			if (b == WireSignal.High)
			{
				Value = Value | (1 << i);
			}
			else
			{
				Value = Value & ~(1 << i);
			}

			//Inversions and such mess up the unused section of the 
			Value = Value & _validMask;
			
			return true;
		}
	}

	public void DependsOn(Pin pin)
	{
		_manager.SetDependentOn(pin,this);
	}

	public bool Enabled { get; }

	public bool IsFloating()
	{
		return Value == FloatingVal[Width];
	}
}