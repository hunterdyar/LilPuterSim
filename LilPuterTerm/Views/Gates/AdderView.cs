using LilPuter;
using Terminal.Gui;

namespace LilPuterTerm;

public class AdderView : Terminal.Gui.Window
{
	//system
	private FullAdder _fullAdder;

	//components
	private PinView _a;
	private PinView _b;
	private PinView _carryIn;

	private PinView _carryOut;
	private PinView _sumOut;
	
	public AdderView(FullAdder fullAdder)
	{
		_fullAdder = fullAdder;
		InitializeComponent();
	}
	private void InitializeComponent()
	{
		this.Height = 4;
		this.X = 0;
		this.Y = 0;
		this.Modal = false;
		this.Border.BorderStyle = Terminal.Gui.BorderStyle.Single;
		this.Border.Effect3D = false;
		this.Border.DrawMarginFrame = true;
		this.TextAlignment = Terminal.Gui.TextAlignment.Centered;
		this.Title = "Adder";

		_a = new PinView(_fullAdder.A, "A");
		this.Add(_a);
		_b = new PinView(_fullAdder.B, "B");
		_b.X = Pos.Right(_a);
		this.Add(_b);
		_carryIn = new PinView(_fullAdder.CarryIn, "Carry In");
		_carryIn.X = Pos.Right(_b);
		this.Add(_carryIn);
			
		_sumOut = new PinView(_fullAdder.SumOut, "Sum");
		_sumOut.Y = Pos.Bottom(_a);
		this.Add(_sumOut);
		_carryOut = new PinView(_fullAdder.CarryOut, "Carry Out");
		_carryOut.Y = Pos.Bottom(_a);
		_carryOut.X = Pos.Right(_sumOut);
		this.Add(_carryOut);

		this.Width = Dim.Width(_a) + Dim.Width(_b)+ Dim.Width(_carryIn)+2;

	}
}