namespace LilPuter;

public class StatusRegister
{
	//The status register is not really a register. It's a collection of flags.
	//Could use the "Bit" or "Register" class, but they work the same way as Pin for now. The reason to do that is the visualization side of things.

	public readonly Pin Zero;
	public readonly Pin Carry;
	//public Pin Negative;

	private ALUMultiBit _alu;
	private ComputerBase _computer;
	public StatusRegister(ComputerBase comp, ALUMultiBit alu)
	{
		_computer = comp;
		_alu = alu;
		
		Zero = new Pin(comp.WireManager, "Zero Flag");
		alu.IsZero.ConnectTo(Zero);
		Carry = new Pin(comp.WireManager, "Carry Flag");
		alu.CarryOut.ConnectTo(Carry);
		//Negative = new Pin(comp.WireManager, "Negative Flag");
		
	}
}