using LilPuter;

namespace LilPuterTerm.ALU;

public class ALUView : Terminal.Gui.Window
{
	private ArithmeticLogicUnit _alu;
	
	public ALUView(ArithmeticLogicUnit alu)
	{
		_alu = alu;
		InitializeComponent();
	}

	private void InitializeComponent()
	{
		//
	}
}