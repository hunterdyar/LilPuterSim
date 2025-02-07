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
		Out.DependsOn(A);
		manager.RegisterSystemAction(A, Trigger);
	}

	private void Trigger(ISystem from)
	{
		var val = A.Signal;
		//floats gonna float
		if (val == WireSignal.Floating)
		{
			Out.Set((int)WireSignal.Floating);
			return;
		}
		Out.Set((int)(val == WireSignal.Low ? WireSignal.High : WireSignal.Low));
		
		//onChange
	}
}