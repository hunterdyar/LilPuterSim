using LilPuter.Clock;

namespace LilPuter;

/// <summary>
/// Right now, this is a high-level only view of RAM. It's just an array!
/// It works identically to a Register by function. Creating this out of Nand and DFF gates is possible.
///	//In that case, it will be a set of nested registers. 8 Registers, then 8 or 16 of those RAM8's, etc. Then some and logic to select the right one.
///
/// The main blocker right now is that I don't have a clean Pin[x] -> Pins breakout system.
///		New type of Breakout and Collector pins for turning into ints and such. I'm on the fence of getting rid of byte[] and replacing them with byte and a type info, so i'm holding on this decision for now.
/// 
/// </summary>
public class RAM
{
	public readonly string Name;
	public int[] Registers => _registers;
	private readonly int[] _registers;
	public readonly Pin Load;
	public readonly Pin In;
	public readonly Pin Address;
	public readonly Pin Out;

	private readonly int _size;
	private int _currentAddress = 0;
	
	public RAM(ComputerBase comp, string name, int width, int size)
	{
		Name = name;
		_size = size;
		Load = new Pin(comp.WireManager, "RAM8 Load");
		Address = new Pin(comp.WireManager, "RAM Address", PinUtility.SizeToRequiredBits(size));
		Address.SetSilently(0);
		In = new Pin(comp.WireManager, "RAM8 In", width);
		Out = new Pin(comp.WireManager, "RAM8 Out", width);
		
		_registers = new int[size];
		for (int i = 0; i < size; i++)
		{
			_registers[i] = 0;
		}
		
		Out.DependsOn(Address);
		Load.DependsOn(Address);
		Out.DependsOn(In);
		Load.DependsOn(In);
		comp.WireManager.RegisterSystemAction(Address, OnAddressChange);
		
	}

	private void OnAddressChange(ISystem obj)
	{
		_currentAddress = Address.Value;
		Out.Set(_registers[_currentAddress]);
	}

	private void OnLoadChange()
	{
		if (Load.Signal == WireSignal.High)
		{
			_currentAddress = Address.Value;
			if (_currentAddress < 0 || _currentAddress > _size)
			{
				//is this a real error? 
				throw new AggregateException("Invalid Memory Address");
			}

			_registers[_currentAddress] = In.Value;
		}
	}

	/// <summary>
	/// Call after manually writing over the internals to update the simulation.
	/// </summary>
	public void ForceUpdateOutput()
	{
		_currentAddress = Address.Value;
		Out.Set(_registers[_currentAddress]);
	}
}