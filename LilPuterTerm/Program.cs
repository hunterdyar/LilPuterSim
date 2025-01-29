using LilPuter;
using LilPuterTerm;
using Terminal.Gui;

Application.Init();

try
{
	WireManager wire = new WireManager();
	Adder adder = new Adder(wire);
	//Starts floating. Let's ground it.
	wire.SetPin(adder.A, WireSignal.Low);
	wire.SetPin(adder.B, WireSignal.Low);
	wire.SetPin(adder.CarryIn, WireSignal.Low);
	Application.Run(new AdderView(adder));
}
finally
{
	Application.Shutdown();
}