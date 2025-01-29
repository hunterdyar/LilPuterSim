using LilPuter;
using LilPuterTerm;
using Terminal.Gui;

Application.Init();

try
{
	WireManager wire = new WireManager();
	NandGate nand = new NandGate(wire);
	wire.SetPin(nand.A, WireSignal.Low);
	wire.SetPin(nand.B, WireSignal.Low);
	
	Application.Run(new NandView(nand));
}
finally
{
	Application.Shutdown();
}