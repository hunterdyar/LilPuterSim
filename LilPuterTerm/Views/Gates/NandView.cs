using Terminal.Gui;
using LilPuter;

namespace LilPuterTerm;

public partial class NandView : Terminal.Gui.Window
{
	//system
	private NandGate _nand;
	
	//components
	private PinView _a;
	private PinView _b;
	private PinView _out;
	public NandView(NandGate nandGate)
	{
		_nand = nandGate;
		InitializeComponent();
	}

	private void InitializeComponent()
	{
		this.Height = 3;
		this.X = 0;
		this.Y = 0;
		this.Modal = false;
		this.Border.BorderStyle = Terminal.Gui.BorderStyle.Single;
		this.Border.Effect3D = false;
		this.Border.DrawMarginFrame = true;
		this.TextAlignment = Terminal.Gui.TextAlignment.Centered;
		this.Title = "Nand";
		
		_a = new PinView(_nand.A, "A");
		this.Add(_a);
		_b = new PinView(_nand.B, "B");
		_b.X = Pos.Right(_a);
		this.Add(_b);
		_out = new PinView(_nand.Out, "Out");
		_out.X = Pos.Right(_b);
		this.Add(_out);

		this.Width = Dim.Width(_a)+Dim.Width(_b)+Dim.Width(_out)+2;

	}
}