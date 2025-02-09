using LilPuter.Clock;

namespace LilPuter;

public class ConsoleOutput
{
	public Pin OutIn;
	public ClockPin Clock;
	public Pin Enable;
	
	public ConsoleOutput(ComputerBase comp)
	{
		OutIn = new Pin(comp.WireManager, "Out In");
		Clock = new ClockPin(comp.Clock, "Clock");
		
		OutIn.DependsOn(Enable);
		Clock.OnTick += OnTick;
	}

	private void OnTick()
	{
		if (Enable.Signal == WireSignal.High)
		{
			Console.WriteLine($"{OutIn.Value:B} - {OutIn.Value:D}");
		}
	}
}