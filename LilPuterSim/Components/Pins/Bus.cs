using LilPuter.Clock;
using Microsoft.VisualBasic;

namespace LilPuter;

/// <summary>
/// TODO: This is currently the controller for setting various components, but... we're not using the load pins of our simulated systems, which we need for zooming into the ALU and such.
/// </summary>
public class Bus
{
	public int Value => Value;
	private int _value;
	
	//todo: make this a pre-allocated array.
	public readonly List<BusConnection> Connections;
	
	private int _width;
	private ComputerBase _computer;
	public readonly ClockPin ClockPin;

	public Bus(ComputerBase comp, int dataWidth)
	{
		_computer = comp;
		_width = dataWidth;
		Connections = [];
		ClockPin = new ClockPin(comp.Clock, "Bus Clock");
		ClockPin.OnTick += OnTick;
		ClockPin.OnTock += OnTock;
	}

	private void OnTick()
	{
		Trigger();
	}
	private void OnTock()
	{
		Trigger();
	}
	
	public int RegisterComponent(string compName, bool isInput, Pin pin,  Pin? loadPin = null, bool invertedLoad = false)
	{
		if (Connections.Any(x => x.Name == compName))
		{
			throw new Exception($"Component {compName} is already registered on the bus.");
		}
		int i = Connections.Count;
		Connections.Add(new BusConnection()
		{
			Name = compName,
			IsInput = isInput,
			LoadPin = loadPin,
			Pin = pin,
			InvertedLoad = invertedLoad,
			Index = i,
		});
		
		return i;
	}

	public void SetComponent(int inputComponentIndex, bool enabled)
	{
		Connections[inputComponentIndex].SetEnabled(enabled);
	}

	public void SetBus(int controlCode)
	{
		for (int i = 0; i < Connections.Count; i++)
		{
			Connections[0].SetEnabled(((controlCode >> i) & 1) == 1);
		}
	}

	
	private void Trigger()
	{
		//Run through twice, first for inputs, second for outputs.
		//Todo: During setup, create separte internal lists to cache. (or lists of indices in the connections)
		
		var ic = 0;
		for (var i = 0; i < Connections.Count; i++)
		{
			if (Connections[i].Enabled)
			{
				if (Connections[i].IsInput)
				{
					_value = Connections[i].Pin.Value;
					ic++;
					break;
				}
			}
		}
		
		if(ic > 1)
		{
			throw new Exception("More than one input enabled! This isn't allowed on bus trigger.");
		}
		
		//Get the output pins that are enabled and set their value to value.
		for (var i = 0; i < Connections.Count; i++)
		{
			if (Connections[i].Enabled)
			{
				if (!Connections[i].IsInput)
				{
					Connections[i].Pin.Set(_value);
					break;
				}
			}
		}
	}

	public int GetCodeFor(params string[] enabledConnections)
	{
		int code = 0;
		for (int i = 0; i < enabledConnections.Length; i++)
		{
			int bit = GetBitFor(enabledConnections[i]);
			code = code | (1 << bit);
		}

		return code;
	}

	private int GetBitFor(string connection)
	{
		var find = Connections.Find(x => x.Name == connection);
		if (find != null)
		{
			return find.Index;
		}
		else
		{
			throw new Exception($"Unable to find bus connection {connection}");
		}
	}
}

public class BusConnection
{
	public string Name;
	public bool Enabled { get; private set; }
	public bool IsInput;
	public required Pin Pin;
	public Pin? LoadPin;//gets set before the input pin gets set, if it's an input pin. (or output)
	public bool InvertedLoad;
	public int Index;

	/// <summary>
	/// Sets enabled and updates the given load pin.
	/// </summary>
	public void SetEnabled(bool enabled)
	{
		if (enabled != Enabled)
		{
			Enabled = enabled;
			SetLoadPin(enabled);
		}
	}
	/// <summary>
	/// Sets or Unsets the load pin, inverting if neccesary and ignoring if null.
	/// </summary>
	private void SetLoadPin(bool enabled)
	{
		if (LoadPin != null)
		{
			if (!InvertedLoad)
			{
				LoadPin.Set(enabled? WireSignal.High : WireSignal.Low);
			}
			else
			{
				LoadPin.Set(enabled? WireSignal.Low : WireSignal.High);
			}
		}
		
	}
}