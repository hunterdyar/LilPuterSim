namespace LilPuter;

public class OrGate
{
	private readonly NandGate _gateA;
	private readonly NandGate _gateB;
	private readonly NandGate _gateC;

	public readonly Pin A;
	public readonly Pin B;
	public Pin Out => _gateC.Out;

	public OrGate()
	{
		_gateA = new NandGate();
		_gateB = new NandGate();
		_gateC = new NandGate();
		A = new Pin();
		B = new Pin();
		
		//https://en.wikipedia.org/wiki/OR_gate#/media/File:OR_from_NAND.svg
		A.ConnectTo(_gateA.A);
		A.ConnectTo(_gateA.B);
		
		B.ConnectTo(_gateB.A);
		B.ConnectTo(_gateB.B);
		
		_gateA.Out.ConnectTo(_gateC.A);
		_gateB.Out.ConnectTo(_gateC.B);
	}
}