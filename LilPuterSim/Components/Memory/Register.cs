using LilPuter.Clock;

namespace LilPuter;

public class Register
{
	public readonly int Bits;
	public readonly Pin Load;
	public readonly Pin Input;
	public readonly Pin Output;
	public readonly ClockPin ClockIn;
	private byte[] _data;

	/// <summary>
	/// This multi-bit register is not based off of DataFlipFlops.
	/// I would like it to be in the future, but this is simple scope constriants.
	/// DFF's are not able to be simulated by nand gates since I don't support feedback loops.
	/// So DFF's are a primitive like NAND gates. And, well. It made sense to make this a primitive too in the meantime, since shortcuts are getting made anyway.
	/// </summary>
	public Register(ComputerBase comp, int bits)
	{
		Load = new Pin(comp.WireManager,"Register Load");
		Input = new Pin(comp.WireManager, "Register In", bits);
		Output = new Pin(comp.WireManager, "Register Out", bits);
		ClockIn = new ClockPin(comp.Clock);
		_data = new byte[bits];
		Bits = bits;
		Output.DependsOn(Input);
		Output.DependsOn(Load);
		
		ClockIn.OnTick += OnTick;
		ClockIn.OnTock += OnTock;
	}

	private void OnTick()
	{
		if (Load.Signal == WireSignal.High)
		{
			_data = Input.Value;
		}
	}

	private void OnTock()
	{
		Output.Set(_data);
	}
}