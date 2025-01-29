namespace LilPuter;

public class Adder
{
	public Pin A;
	public Pin B;
	public Pin CarryIn;
	public Pin SumOut;
	public Pin CarryOut;

	private XorGate _xorA;
	private XorGate _xorB;
	private AndGate _andA;
	private AndGate _andB;
	private OrGate _or;

	private WireManager _manager;

	public Adder(WireManager manager)
	{
		_manager = manager;
		A = new Pin(manager, "AdderA");
		B = new Pin(manager, "AdderB");
		CarryIn = new Pin(manager, "AdderCarryIn");
		SumOut = new Pin(manager, "AdderSumOut");
		CarryOut = new Pin(manager, "AdderCarryOut");
		_andA = new AndGate(manager);
		_andB = new AndGate(manager);
		_xorA = new XorGate(manager);
		_xorB = new XorGate(manager);
		_or = new OrGate(manager);

		A.ConnectTo(_xorA.A);
		A.ConnectTo(_andB.A);

		B.ConnectTo(_xorA.B);
		B.ConnectTo(_andB.B);

		CarryIn.ConnectTo(_xorB.B);
		CarryIn.ConnectTo(_andA.B);
		
		_xorA.Out.ConnectTo(_xorB.A);
		_xorA.Out.ConnectTo(_andA.A);

		_xorB.Out.ConnectTo(SumOut);
		_andA.Out.ConnectTo(_or.A);
		_andB.Out.ConnectTo(_or.B);
		
		_or.Out.ConnectTo(CarryOut);
	}
	
	
}