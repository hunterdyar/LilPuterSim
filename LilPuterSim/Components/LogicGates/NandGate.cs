namespace LilPuter;

public class NandGate
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
	}

	internal void Trigger(Pin p)
	{
		var data = new byte[]
			{ (byte)(A.Signal == WireSignal.Low ||
			         B.Signal == WireSignal.Low ? WireSignal.High : WireSignal.Low) };
		Out.Set(data);
	}
	
	// public byte[] ReadValue()
	// {
	// 	return Out.ReadValue();
	// }
	// public Type ValueType => Out.ValueType;
} 