using LilPuter;

namespace LilPuterSimTest;

public class InstructionTests
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
		_comp.WireManager.SetPin(_comp.Memory.Address,3);
		_comp.Bus.ClockPin.SetEnabled(false);//we are setting control bits manually!
		_comp.Clock.Cycle();

		_comp.Bus.SetBus(loadA);
		_comp.Clock.Cycle();

	}
	
	[Test]
	public void LoadA()
	{
		
	}
}