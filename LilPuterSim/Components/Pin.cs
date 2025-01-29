using System.Collections;

namespace LilPuter;

//todo: Create a GUID on creation for passing around serialized forms.

/// <summary>
/// A pin is a wire that can be set. It holds a byte[] value (like everything else).
/// </summary>
public class Pin : IObservable
{
	public string Name { get; private set; }
	public byte[] Value { get; private set; } = [(byte)WireSignal.Floating];//Default should be not connected == floating
	public WireSignal Signal => (WireSignal)Value[0];
	public Type ValueType => typeof(WireSignal);

	private WireManager manager;

	public Pin(WireManager manager, string name)
	{
		this.manager = manager;
		this.Name = name;
	}

	public byte[] ReadValue()
	{
		return Value;
	}

	public bool Set(byte[] value, bool alwaysUpdate = false)
	{
		if (value.Length != 1)
		{
			throw new ArgumentException("Value for pin must be a single byte.");
		}

		var newVal = (WireSignal)value[0];
		bool changed = newVal != Signal;
		if(changed || alwaysUpdate)
		{
			Value = [(byte)newVal];
			manager.Changed(this, Value);
		}

		return changed;
	}

	public void Set(WireSignal newVal, bool alwaysUpdate = false)
	{
		Set([(byte)newVal], alwaysUpdate);
	}

	/// <summary>
	/// Unlike real wires, connections are 1-way unless we explicitly state otherwise.
	/// This function is not memory efficient, but we optimize for the runtime, not configuration time. Connections should rarely change during runtime (use a switch component).
	/// </summary>
	/// <param name="otherPin">The pin to connect a wire to.</param>
	/// <param name="twoWay">Whether to also connect other pin to this pin.</param>
	public void ConnectTo(Pin otherPin, bool twoWay = false)
	{
		manager.Connect(this, otherPin, twoWay);
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
		return $"Pin {Name} ({Signal})"; 
	}
}