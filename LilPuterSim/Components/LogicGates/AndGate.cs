namespace LilPuter;

public class AndGate
{
	public Pin A => _nandGateA.A;
	public Pin B => _nandGateA.B;
	public Pin Out => _nandGateB.Out;

	//This and gate is connected from Nand gates.
	private readonly NandGate _nandGateA;
	private readonly NandGate _nandGateB;

	public AndGate()
	{
		_nandGateA = new NandGate();
		_nandGateB = new NandGate();

		//The out gets split and goes to both pins.
		_nandGateA.Out.ConnectTo(_nandGateB.A);
		_nandGateB.Out.ConnectTo(_nandGateB.B);
	}
}