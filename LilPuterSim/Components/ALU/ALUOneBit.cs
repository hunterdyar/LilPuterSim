namespace LilPuter
{
	public class ALUOneBit : SubscriberBase<ALUOneBit>
	{
		//Inputs
		public readonly Pin A;
		public readonly Pin B;
		public readonly Pin InvertA;
		public readonly Pin InvertB;
		public readonly Pin CarryIn;
	
		//OpCode
		public readonly Pin Op;// 000 = Add, 001... etc.
	
		//Outputs
		public readonly Pin Result;
		public readonly Pin CarryOut;
	
		//Internal
		public Mux4by1 Mux => _mux;
		private readonly Mux4by1 _mux;
		public FullAdder FullAdder => _adder;
		private readonly FullAdder _adder;
		public AndGate And => _and;
		private readonly AndGate _and;
		public OrGate Or => _or;
		private readonly OrGate _or;
	
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

		public static string OpAsString(int op)
		{
			switch (op)
			{
				case 0:
					return "+";
				case 1:
					return "AND";
				case 2:
					return "OR";
			}

			return "";
		}

		public override ALUOneBit ReadValue()
		{
			return this;
		}
	}
}