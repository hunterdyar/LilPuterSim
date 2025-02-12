namespace LilPuter
{
	public class AndBank
	{
		public Pin A;
		public Pin B;
		public Pin Out;
		private int bitWidth;
	
		public AndBank(WireManager manager, int bitWidth)
		{
			this.bitWidth = bitWidth;
			A = new Pin(manager, "AndBankA", bitWidth);
			B = new Pin(manager, "AndBankB", bitWidth);
			Out = new Pin(manager, "AndBankOut", bitWidth);
			Out.DependsOn(A);
			Out.DependsOn(B);
			manager.RegisterSystemAction(A, Trigger);
			manager.RegisterSystemAction(B, Trigger);
		}

		private void Trigger(ISystem pin)
		{
			Out.Set(A.Value & B.Value);
		}
	}
}