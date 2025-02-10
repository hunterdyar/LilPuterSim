using LilPuter.Clock;

namespace LilPuter;

public class Register
{
	public readonly int Bits;
	public readonly Pin Load;
	public readonly Pin Input;
	public readonly Pin Output;

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
		Bits = bits;
		
		Output.DependsOn(Input);
		Output.DependsOn(Load);
		
		comp.WireManager.RegisterSystemAction(Load, OnInputChange);
		comp.WireManager.RegisterSystemAction(Input, OnInputChange);
	}

	private void OnInputChange(ISystem pin)
	{
		if (Load.Signal == WireSignal.High)
		{
			Output.Set(Input.Value);
		}
	}

	
}