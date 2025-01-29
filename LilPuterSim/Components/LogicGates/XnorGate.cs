namespace LilPuter;

public class XnorGate
{
	private XorGate _xor;
	private NotGate _not;

	public Pin A => _xor.A;
	public Pin B => _xor.B;
	public Pin Out => _not.Out;

	public XnorGate(WireManager manager)
	{
		_xor = new XorGate(manager);
		_not = new NotGate(manager);
		
		_xor.Out.ConnectTo(_not.A);
	}
}