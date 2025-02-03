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
		A.ConnectTo(Out, true);
		B.ConnectTo(Out, true);
		manager.Listen(A, Trigger);
		manager.Listen(B, Trigger);
	}

	private void Trigger(Pin pin)
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