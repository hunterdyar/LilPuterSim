using System.Collections;

namespace LilPuter;

public class Pin : IObservable
{
	public byte[] Value { get; private set; } = [0];
	public WireSignal Signal => (WireSignal)Value[0];
	public Action<byte[]> OnValueChange { get; set; }
	public Type ValueType => typeof(WireSignal);

	public Pin[] Connections = [];
	public byte[] ReadValue()
	{
		return Value;
	}

	public void Set(byte[] value, bool forceUpdate = true)
	{
		if (value.Length != 1)
		{
			throw new ArgumentException("Value for pin must be a single byte.");
		}

		var newVal = (WireSignal)value[0];
		if(newVal != Signal || forceUpdate)
		{
			Value = [(byte)newVal];

			//
			foreach (var connection in Connections)
			{
				connection.Set(value, forceUpdate);
				//todo: This propogates changes. I want to update all of my connections before propogating changes.
				//In graph terms, I want to do breath-first, not depth-first walking. hmmmm.
			}

			// foreach (var connection in Connections)
			// {
			// 	connection.Update();
			// }
			
			OnValueChange?.Invoke(value);
		}
	}

	public void Set(WireSignal newVal, bool update = true)
	{
		Set([(byte)newVal]);
	}

	/// <summary>
	/// Unlike real wires, connections are 1-way unless we explicitly state otherwise.
	/// This function is not memory efficient, but we optimize for the runtime, not configuration time. Connections should rarely change during runtime (use a switch component).
	/// </summary>
	/// <param name="otherPin">The pin to connect a wire to.</param>
	/// <param name="twoWay">Whether to also connect other pin to this pin.</param>
	public void ConnectTo(Pin otherPin, bool twoWay = false)
	{
		//Todo: Change this connection and registering to a list of our wires with the "breadboard manager". Wire will become a listenable component, basically.
		if (Connections.Contains(otherPin))
		{
			throw new Exception("Pin is already connected.");
		}	
		
		Array.Resize(ref Connections, Connections.Length+1);
		Connections[^1] = otherPin;
		if (twoWay)
		{
			Array.Resize(ref otherPin.Connections, otherPin.Connections.Length + 1);
			otherPin.Connections[^1] = this;
		}
	}

	public void DisconnectFrom(Pin otherPin, bool twoWay)
	{
		throw new NotImplementedException("Not Implemented. Pins should not be disconnected during runtime. Just configure it correctly lol?");
	}
}