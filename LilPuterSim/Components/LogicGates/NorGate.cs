namespace LilPuter;

public class NorGate
{
	public Pin A;
	public Pin B;
	public Pin Out => _norGateD.Out;

	//This and gate is connected from Nand gates.
	private readonly NandGate _norGateInA;
	private readonly NandGate _norGateInB;
	private readonly NandGate _norGateC;
	private readonly NandGate _norGateD;

	public NorGate(WireManager manager)
	{
		A=new Pin(manager, "Nor In A");
		B=new Pin(manager, "Nor In B");
		_norGateInA = new NandGate(manager);
		_norGateInB = new NandGate(manager);
		_norGateC = new NandGate(manager);
		_norGateD = new NandGate(manager);
		_norGateD.Out.SetName("Nor Out");
		
		//https://en.wikipedia.org/wiki/NOR_gate#/media/File:NOR_from_NAND.svg
		manager.ConnectPins(A, _norGateInA.A);
		manager.ConnectPins(A, _norGateInB.B);
		
		manager.ConnectPins(B, _norGateInB.A);
		manager.ConnectPins(B, _norGateInB.B);
		
		manager.ConnectPins(_norGateInA.Out, _norGateC.A);
		manager.ConnectPins(_norGateInB.Out, _norGateC.B);
		
		manager.ConnectPins(_norGateC.Out, _norGateD.A);
		manager.ConnectPins(_norGateC.Out, _norGateD.B);
		
	}
}