using LilPuter;

namespace LilPuterSimTest;

public class  InstructionTests
{
	private ComputerBase _comp;

	[SetUp]
	public void Setup()
	{
		_comp = new ComputerBase();
	}


	[Test]
	public void BusAndRegistersTest()
	{
		var loadA = _comp.Bus.GetCodeFor("AI", "MO");
		var loadB = _comp.Bus.GetCodeFor("BI", "MO");
		var movab = _comp.Bus.GetCodeFor("AO", "BI");
		var movba = _comp.Bus.GetCodeFor("BO", "AI");

		//set MO to be some non-default register, non-default output.
		_comp.Memory.Registers[3] = 42;
		_comp.Memory.Registers[4] = 100;
		_comp.WireManager.SetPin(_comp.Memory.Address,3);
		_comp.Clock.Cycle();

		_comp.Bus.SetBus(loadA);
		Assert.That(_comp.CPU.A.Output.Value, Is.EqualTo(42));

		//manually set memory to address 4.
		_comp.WireManager.SetPin(_comp.Memory.Address, 4);
		_comp.Bus.SetBus(loadB);
		Assert.That(_comp.CPU.B.Output.Value, Is.EqualTo(100));
		
	}

	[Test]
	public void AddTwoNumbers()
	{
		var loadA = _comp.Bus.GetCodeFor("AI", "MO");
		var loadB = _comp.Bus.GetCodeFor("BI", "MO");
		//move the result to a.
		var add = _comp.Bus.GetCodeFor("AI", "ALUO");

		//set MO to be some non-default register, non-default output.
		_comp.Memory.Registers[3] = 42;
		_comp.Memory.Registers[4] = 8;
		_comp.WireManager.SetPin(_comp.Memory.Address, 3);
		_comp.Clock.Cycle();

		_comp.Bus.SetBus(loadA);
		Assert.That(_comp.CPU.A.Output.Value, Is.EqualTo(42));
		_comp.WireManager.SetPin(_comp.Memory.Address, 4);
		
		_comp.Bus.SetBus(loadB);
		Assert.That(_comp.CPU.B.Output.Value, Is.EqualTo(08));
		Assert.That(_comp.CPU.ALU.Result.Value, Is.EqualTo(50));
		_comp.Bus.SetBus(add);
		Assert.That(_comp.CPU.A.Output.Value, Is.EqualTo(50));
	}
}