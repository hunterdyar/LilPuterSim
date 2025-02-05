namespace LilPuter;

/// <summary>
/// 2x1 Multiplexer implemented with NAND gates.
/// </summary>
public class Mux
{
	public readonly Pin Select;
	public readonly Pin A;
	public readonly Pin B;

	private readonly NandGate _a;
	private readonly NandGate _b;
	private readonly NandGate _c;
	private readonly NandGate _invert;
	public Pin Out => _c.Out;

	
	public Mux(WireManager manager)
	{
		Select = new Pin(manager, "Mux Select");
		A = new Pin(manager, "Mux A");
		B = new Pin(manager, "Mux B");
		
		_a = new NandGate(manager);
		_b = new NandGate(manager);
		_c = new NandGate(manager);
		_invert = new NandGate(manager);
		Out.Name = "Mux Out";

		B.ConnectTo(_a.A);
		Select.ConnectTo(_a.B);
		Select.ConnectTo(_invert.A);
		Select.ConnectTo(_invert.B);
		_invert.Out.ConnectTo(_b.A);
		A.ConnectTo(_b.B);
		_a.Out.ConnectTo(_c.A);
		_b.Out.ConnectTo(_c.B);
	}
}