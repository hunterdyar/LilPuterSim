using LilPuter;
using LilPuterTerm;
using Terminal.Gui;

Application.Init();

try
{
	WireManager wire = new WireManager();
	FullAdder fullAdder = new FullAdder(wire);
	//Starts floating. Let's ground it.
	wire.SetPin(fullAdder.A, WireSignal.Low);
	wire.SetPin(fullAdder.B, WireSignal.Low);
	wire.SetPin(fullAdder.CarryIn, WireSignal.Low);
	Application.Run(new AdderView(fullAdder));
}
finally
{
	Application.Shutdown();
}