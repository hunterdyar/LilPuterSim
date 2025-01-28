namespace LilPuter;

public class NotGate
{
	public Pin A;
	public Pin Out;

	public NotGate()
	{
		A = new Pin();
		Out = new Pin();

		A.OnValueChange += OnInputChanged;
	}

	private void OnInputChanged(byte[] obj)
	{
		var val = (WireSignal)obj[0];
		//floats gonna float
		if (val == WireSignal.Floating)
		{
			Out.Set([(byte)WireSignal.Floating]);
			return;
		}
		Out.Set([(byte)(val == WireSignal.Low ? WireSignal.High : WireSignal.Low)]);
		
		//onChange
	}
}