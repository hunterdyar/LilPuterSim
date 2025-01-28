namespace LilPuter;

public class NandGate : IObservable
{
	public readonly Pin A;
	public readonly Pin B;
	public readonly Pin Out;
	
	public NandGate()
	{
		A = new Pin();
		B = new Pin();
		Out = new Pin();
		A.OnValueChange += OnInputChanged;
		B.OnValueChange += OnInputChanged;
		Trigger(true);
	}

	private void OnInputChanged(byte[] obj)
	{
		Trigger();
	}

	private void Trigger(bool forceUpdate = false)
	{
		var data = new byte[]
			{ (byte)(A.Signal == WireSignal.Low || B.Signal == WireSignal.Low ? WireSignal.High : WireSignal.Low) };
		Out.Set(data,forceUpdate);
	}
	
	//TODO: Some way to get the appriate pin from the registration system.
	
	
	//Implement IObservable by passing along the out pin as observed.
	public Action<byte[]> OnValueChange => Out.OnValueChange;
	public byte[] ReadValue()
	{
		return Out.ReadValue();
	}
	public Type ValueType => Out.ValueType;
}