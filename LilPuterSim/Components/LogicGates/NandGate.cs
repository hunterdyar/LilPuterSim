namespace LilPuter;

public class NandGate : IObservable
{
	public readonly Pin A;
	public readonly Pin B;
	public readonly Pin Out;
	
	public NandGate(WireManager manager)
	{
		A = new Pin(manager, "NandGateA");
		B = new Pin(manager, "NandGateB");
		Out = new Pin(manager, "NandGateOut");
		manager.Listen(A, Trigger);
		manager.Listen(B, Trigger);
		
		//ensure our nandgate output begins in the correct state.
		//Techniclly we ignore the pin that changed, so we only need to call this once.
		Trigger(A);
	}

	internal void Trigger(Pin p)
	{
		var data = new byte[]
			{ (byte)(A.Signal == WireSignal.Low || B.Signal == WireSignal.Low ? WireSignal.High : WireSignal.Low) };
		Out.Set(data);
	}
	
	public byte[] ReadValue()
	{
		return Out.ReadValue();
	}
	public Type ValueType => Out.ValueType;

	
}