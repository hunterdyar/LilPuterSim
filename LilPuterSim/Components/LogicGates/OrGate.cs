﻿namespace LilPuter
{
	public class OrGate
	{
		private readonly NandGate _gateA;
		private readonly NandGate _gateB;
		private readonly NandGate _gateC;

		public readonly Pin A;
		public readonly Pin B;
		public Pin Out => _gateC.Out;

		public OrGate(WireManager manager)
		{
			_gateA = new NandGate(manager);
			_gateB = new NandGate(manager);
			_gateC = new NandGate(manager);
		
			A = new Pin(manager, "Or inA");
			B = new Pin(manager, "Or inB");
			Out.SetName("Or Out");
		
			//https://en.wikipedia.org/wiki/OR_gate#/media/File:OR_from_NAND.svg
			A.ConnectTo(_gateA.A);
			A.ConnectTo(_gateA.B);
		
			B.ConnectTo(_gateB.A);
			B.ConnectTo(_gateB.B);
		
			manager.ConnectPins(_gateA.Out, _gateC.A);
			manager.ConnectPins(_gateB.Out, _gateC.B);
		
			//set outputs from default inputs.
			_gateA.Trigger(A);
			_gateB.Trigger(B);
			_gateC.Trigger(_gateA.Out);
		}

		public override string ToString()
		{
			return "Or Gate (" + A + ", " + B + ": " + Out+")";
		}
	}
}