namespace LilPuter;

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
		var data = Out.Value;
		for (int i = 0; i < bitWidth; i++)
		{
			data[i] =
				(byte)(A.Value[i] == (byte)WireSignal.High && B.Value[i] == (byte)WireSignal.High
				? (byte)
				WireSignal.High
				: (byte)WireSignal.Low);
		}
		Out.Set(data);
	}
}