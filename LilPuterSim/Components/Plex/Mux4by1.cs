namespace LilPuter;

public class Mux4by1
{
	public Pin A;
	public Pin B;
	public Pin C;
	public Pin D;

	public Pin Select;
	private Breakout controlSignals;

	private NotGate _invertA;
	private NotGate _invertB;
	private AndGate _andA1;
	private AndGate _andA2;
	private AndGate _andB1;
	private AndGate _andB2;
	private AndGate _andC1;
	private AndGate _andC2;
	private AndGate _andD1;
	private AndGate _andD2;
	
	private OrGate _orA;
	private OrGate _orB;
	private OrGate _orC;

	public Pin Out => _orC.Out;
	public Mux4by1(WireManager wm)
	{
		//internals
		_orA = new OrGate(wm);
		_orB = new OrGate(wm);
		_orC = new OrGate(wm);
		_invertA = new NotGate(wm);
		_invertB = new NotGate(wm);
		
		_andA1 = new AndGate(wm);
		_andA2 = new AndGate(wm);
		_andB1 = new AndGate(wm);
		_andB2 = new AndGate(wm);
		_andC1 = new AndGate(wm);
		_andC2 = new AndGate(wm);
		_andD1 = new AndGate(wm);
		_andD2 = new AndGate(wm);
		
		A = new Pin(wm, "Mux A");
		B = new Pin(wm, "Mux B");
		C = new Pin(wm, "Mux C");
		D = new Pin(wm, "Mux D");
		Select = new Pin(wm, "Mux S",2);
		controlSignals = new Breakout(wm, "control breakout",2);
		controlSignals.OutPins[0].ConnectTo(_invertA.A);
		controlSignals.OutPins[1].ConnectTo(_invertB.A);
		Select.ConnectTo(controlSignals.Input);
		//Andgates.
		_andA1.Out.ConnectTo(_andA2.A);
		_andB1.Out.ConnectTo(_andB2.A);
		_andC1.Out.ConnectTo(_andC2.A);
		_andD1.Out.ConnectTo(_andD2.A);
		
		_andA2.Out.ConnectTo(_orA.A);
		_andB2.Out.ConnectTo(_orA.B);
		_andC2.Out.ConnectTo(_orB.A);
		_andD2.Out.ConnectTo(_orB.B);
		//final or output
		_orA.Out.ConnectTo(_orC.A);
		_orB.Out.ConnectTo(_orC.B);
		
		//Signals for first and group, both low.
		_invertA.Out.ConnectTo(_andA1.A);
		_invertB.Out.ConnectTo(_andA1.B);
		A.ConnectTo(_andA2.B);
		
		//Signals for second group. 0 high, 1 low
		controlSignals.OutPins[0].ConnectTo(_andB1.A);
		_invertB.Out.ConnectTo(_andB1.B);
		B.ConnectTo(_andB2.B);

		//Signals for second group. 1 high, 0 low
		_invertA.Out.ConnectTo(_andC1.A);
		controlSignals.OutPins[1].ConnectTo(_andC1.B);
		C.ConnectTo(_andC2.B);
		
		//final group, both high.
		controlSignals.OutPins[0].ConnectTo(_andD1.A);
		controlSignals.OutPins[1].ConnectTo(_andD1.B);
		D.ConnectTo(_andD2.B);
	}
}