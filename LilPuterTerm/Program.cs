using LilPuter;
using LilPuterTerm;
using Terminal.Gui;

Application.Init();

try
{
	ComputerBase compy = new ComputerBase();
	FullAdder fullAdder = new FullAdder(compy.WireManager);
	//Starts floating. Let's ground it.
	compy.WireManager.SetPin(fullAdder.A, WireSignal.Low);
	compy.WireManager.SetPin(fullAdder.B, WireSignal.Low);
	compy.WireManager.SetPin(fullAdder.CarryIn, WireSignal.Low);
	Application.Run(new AdderView(fullAdder));
}
finally
{
	Application.Shutdown();
}