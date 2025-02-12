namespace LilPuter;

public class ALUOneBit
{
	//Inputs
	public Pin A;
	public Pin B;
	public Pin InvertA;
	public Pin InvertB;
	public Pin CarryIn;
	
	//OpCode
	public Pin Op;// 000 = Add, 001... etc.
	
	//Outputs
	public Pin Result;
	public Pin CarryOut;
	
	//Internal
	private Mux4by1 _mux;
	private FullAdder _adder;
	private AndGate _and;
	private OrGate _or;
	
	public ALUOneBit(WireManager manager)
	{
		A = new Pin(manager, "A");
		B = new Pin(manager, "B");
		//todo: Wire up the inverts.
		InvertA = new Pin(manager, "InvertA");
		InvertB = new Pin(manager, "InvertB");
		CarryIn = new Pin(manager, "CarryIn");
		CarryOut = new Pin(manager, "CarryOut");
		Result = new Pin(manager, "Result");
		_mux = new Mux4by1(manager);//Add, And, Or, and UNUSED
		Op = new Pin(manager,"Op");
		
		CarryOut.DependsOn(Op);
		Result.DependsOn(Op);
		Result.DependsOn(A);
		Result.DependsOn(B);
		
		_adder = new FullAdder(manager);
		_and = new AndGate(manager);
		_or = new OrGate(manager);
		//Connect the internal components
		Op.ConnectTo(_mux.Select);
		_mux.Out.ConnectTo(Result);
		
		//Adder. Op is 0
		A.ConnectTo(_adder.A);
		B.ConnectTo(_adder.B);
		CarryIn.ConnectTo(_adder.CarryIn);
		_adder.CarryOut.ConnectTo(CarryOut);
		_adder.SumOut.ConnectTo(_mux.A);
		
		//And. Op is 001
		A.ConnectTo(_and.A);
		B.ConnectTo(_and.B);
		_and.Out.ConnectTo(_mux.B);
		
		//Or. Op is 10
		A.ConnectTo(_or.A);
		B.ConnectTo(_or.B);
		_or.Out.ConnectTo(_mux.C);
		
		_mux.D.SetSilently(WireSignal.Low);
	}

	public override string ToString()
	{
		return $"ALUOneBit ({Result.Value})";
	}
}