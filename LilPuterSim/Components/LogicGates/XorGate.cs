namespace LilPuter;

public class XorGate
{
	private readonly NandGate _gateA;
	private readonly NandGate _gateB;
	private readonly NandGate _gateC;
	private readonly NandGate _gateD;
	
	public readonly Pin A;
	public readonly Pin B;
	public Pin Out => _gateD.Out;

	public XorGate(WireManager manager)
	{
		_gateA = new NandGate(manager);
		_gateB = new NandGate(manager);
		_gateC = new NandGate(manager);
		_gateD = new NandGate(manager);

		A = new Pin(manager, "XOR inA");
		B = new Pin(manager, "XOR inB");
		Out.SetName("Xor Out");

		//https://en.wikipedia.org/wiki/OR_gate#/media/File:OR_from_NAND.svg
		A.ConnectTo(_gateB.A);
		A.ConnectTo(_gateA.A);

		B.ConnectTo(_gateA.B);
		B.ConnectTo(_gateC.B);

		_gateA.Out.ConnectTo(_gateB.B);
		_gateA.Out.ConnectTo(_gateC.A);
		
		_gateB.Out.ConnectTo(_gateD.A);
		_gateC.Out.ConnectTo(_gateD.B);
	}

}