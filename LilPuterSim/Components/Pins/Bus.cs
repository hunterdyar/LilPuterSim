using LilPuter.Clock;
using Microsoft.VisualBasic;

namespace LilPuter;

/// <summary>
/// TODO: This is currently the controller for setting various components, but... we're not using the load pins of our simulated systems, which we need for zooming into the ALU and such.
/// </summary>
public class Bus : SubscriberBase<int>
{
	public int Value => _value;
	private int _value;
	public override int ReadValue() => _value;

	//todo: make this a pre-allocated array.
	public readonly List<BusConnection> Connections;
	
	private readonly int _width;
	private readonly ComputerBase _computer;

	public Bus(ComputerBase comp, int dataWidth)
	{
		_computer = comp;
		_width = dataWidth;
		Connections = [];
	}
	
	public int RegisterComponent(string compName, bool setFromBus, bool setTobus, Pin? pin,  Pin? loadPin = null, bool invertedLoad = false)
	{
		if (Connections.Any(x => x.Name == compName))
		{
			throw new Exception($"Component {compName} is already registered on the bus.");
		}
		
		int i = Connections.Count;
		if (setTobus || setFromBus)
		{
			if (pin == null)
			{
				throw new Exception($"Invalid bus component {compName}. Must have a data-line connection to bus.");
			}
			//todo: check pin width.
		}
		Connections.Add(new BusConnection()
		{
			Name = compName,
			SetFromBus = setFromBus,
			SetToBus = setTobus,
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
			Connections[i].SetEnabled(((controlCode >> i) & 1) == 1);
		}

		if (controlCode != 0)
		{
			Trigger();
		}
		else
		{
			//useful spot to put break point on NOPS in the middle of a program.
		}
	}

	
	private void Trigger()
	{
		//Run through twice, first for data to the bus, second data from the bus.
		//Todo: During setup, create separte internal lists to cache. (or lists of indices in the connections)
		string debug = "";
		var ic = 0;
		for (var i = 0; i < Connections.Count; i++)
		{
			if (Connections[i].Enabled)
			{
				if (Connections[i].SetToBus)
				{
					//IsInput and Null Pin cannot both be true, so we supress.
					_value = Connections[i].Pin!.Value;
					debug += Connections[i].Name + " ";
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
				if (Connections[i].SetFromBus )
				{
					var pin = Connections[i].Pin;
					if (pin != null)
					{
						//Todo: replace with pin SetAndImpulse so we can use multiple wiremanagers.
						_computer.WireManager.SetPin(pin, _value);
						debug += Connections[i].Name + " ";
					}

					break;
				}
				else
				{
					//could be a load-pin only signal, which will have gotten set with SetBus()
					if (Connections[i].LoadPin != null)
					{
						debug += Connections[i].Name + " ";
					}
				}
				
			}
		}
		
		Console.WriteLine(debug);
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