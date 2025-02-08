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
	public readonly List<BusConnection> Inputs;
	public readonly List<BusConnection> Outputs;
	
	private int _width;
	private ComputerBase _computer;
	public readonly ClockPin ClockPin;
	private uint _inputMask =  0x00FF;
	private uint _outputMask = 0xFF00;
	public Bus(ComputerBase comp, int dataWidth)
	{
		_computer = comp;
		_width = dataWidth;
		Inputs = [];
		Outputs = [];
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
	
	public (int inputPinIndex, int outputPinIndex) RegisterComponent(string compName, Pin inputPin, Pin outputPin,  Pin? loadPin = null, bool invertedLoad = false)
	{
		int i = Inputs.Count;
		Inputs.Add(new BusConnection()
		{
			Name = compName,
			IsInput = true,
			LoadPin = loadPin,
			Pin = inputPin,
			InvertedLoad = invertedLoad,
			Index = i,
		});
		int o = Outputs.Count;
		Outputs.Add(new BusConnection()
		{
			Name = compName,
			IsInput = false,
			LoadPin = loadPin,
			Pin = outputPin,
			InvertedLoad = invertedLoad,
			Index = o,
		});
		return (i, o);
	}

	public void SetInputComponent(int inputComponentIndex, bool enabled)
	{
		Inputs[inputComponentIndex].SetEnabled(enabled);
	}

	public void SetOutputComponent(int outputComponentIndex, bool enabled)
	{
		Outputs[outputComponentIndex].SetEnabled(enabled);
	}

	public void SetBus(uint controlCode)
	{
		uint ic = controlCode & _inputMask;
		for (int i = 0; i < Inputs.Count; i++)
		{
			Inputs[0].SetEnabled(((ic >> i) & 1) == 1);
		}

		uint oc = controlCode & _outputMask;
		for (int i = 0; i < Outputs.Count; i++)
		{
			Outputs[0].SetEnabled(((oc >> i) & 1) == 1);
		}
	}

	
	private void Trigger()
	{
		//Get the input value that is enabled and set our value to it's value.
		for (int i = 0; i < Inputs.Count; i++)
		{
			if (Inputs[i].Enabled)
			{
				_value = Inputs[i].Pin.Value;
				break;
			}
		}
		
		if(Inputs.Count(x => x.Enabled) > 1)
		{
			throw new Exception("More than one input enabled! This isn't allowed on trigger.");
		}
		
		//Get the output pins that are enabled and set their value to value.
		for (int i = 0; i < Outputs.Count; i++)
		{
			if (Outputs[i].Enabled)
			{
				//through the wiremanager to trigger propagation.
				_computer.WireManager.SetPin(Outputs[i].Pin,_value);
			}
		}
	}
}

public class BusConnection
{
	public string Name;
	public bool Enabled { get; private set; }
	public bool IsInput;
	public Pin Pin;
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