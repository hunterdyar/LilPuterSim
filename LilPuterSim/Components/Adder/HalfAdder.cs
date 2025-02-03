namespace LilPuter;

public class HalfAdder
{
	public Pin A;
	public Pin B;
	public Pin SumOut;
	public Pin CarryOut;

	private XorGate _xor;
	private AndGate _and;
	
	private WireManager _manager;

	public HalfAdder(WireManager manager)
	{
		_manager = manager;
		A = new Pin(manager, "HalfAdderA");
		B = new Pin(manager, "HalfAdderB");
		SumOut = new Pin(manager, "HalfAdderSumOut");
		CarryOut = new Pin(manager, "HalfAdderCarryOut");
		_and = new AndGate(manager);
		_xor = new XorGate(manager);
		
		A.ConnectTo(_xor.A);
		A.ConnectTo(_and.A);
		
		B.ConnectTo(_xor.B);
		B.ConnectTo(_and.B);
		
		_xor.Out.ConnectTo(SumOut);
		_and.Out.ConnectTo(CarryOut);
	}
	
}