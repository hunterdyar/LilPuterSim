namespace LilPuter
{
	public class FullAdder
	{
		public readonly Pin A;
		public readonly Pin B;
		public readonly Pin CarryIn;
		public readonly Pin SumOut;
		public readonly Pin CarryOut;

		private XorGate _xorA;
		private XorGate _xorB;
		private AndGate _andA;
		private AndGate _andB;
		private OrGate _or;
	
		private WireManager _manager;

		public FullAdder(WireManager manager)
		{
			_manager = manager;
			A = new Pin(manager, "FullAdderA");
			B = new Pin(manager, "FullAdderB");
			CarryIn = new Pin(manager, "FullAdderCarryIn");
			SumOut = new Pin(manager, "FullAdderSumOut");
			CarryOut = new Pin(manager, "FullAdderCarryOut");

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

			_andA.Out.ConnectTo(_or.A);
			_andB.Out.ConnectTo(_or.B);
			_xorB.Out.ConnectTo(SumOut);

			_or.Out.ConnectTo(CarryOut);
		}
	}
}