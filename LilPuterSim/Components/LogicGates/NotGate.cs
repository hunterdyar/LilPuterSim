namespace LilPuter;

//Todo: replace with Nand construction for fun
public class NotGate
{
	public Pin A;
	public Pin Out;
	public NotGate(WireManager manager)
	{
		A = new Pin(manager, "Not In");
		Out = new Pin(manager, "Not Out");
		manager.Listen(A, Trigger);
	}

	private void Trigger(Pin from)
	{
		var val = A.Signal;
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