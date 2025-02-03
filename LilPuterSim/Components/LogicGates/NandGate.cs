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
		A.ConnectTo(Out);
		B.ConnectTo(Out);
		manager.RegisterSystemAction(A, Trigger);
		manager.RegisterSystemAction(B, Trigger);
	}

	internal void Trigger(ISystem p)
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