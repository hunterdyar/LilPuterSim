using LilPuter;

namespace LilPuterTerm.ALU;

public class ALUView : Terminal.Gui.Window
{
	private ALUMultiBit _alu;
	
	public ALUView(ALUMultiBit alu)
	{
		_alu = alu;
		InitializeComponent();
	}

	private void InitializeComponent()
	{
		//
	}
}