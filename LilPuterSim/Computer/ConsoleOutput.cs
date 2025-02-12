using System;
using LilPuter.Clock;

namespace LilPuter
{
	public class ConsoleOutput
	{
		public Pin OutIn;
		public ClockPin Clock;
		public Pin Enable;
		public Action<int> OnOutput;
	
		public ConsoleOutput(ComputerBase comp)
		{
			Enable = new Pin(comp.WireManager,"Output Enable");
			OutIn = new Pin(comp.WireManager, "Out In");
			Clock = new ClockPin(comp.Clock, "Clock");
		
			OutIn.DependsOn(Enable);
			Clock.OnTick += OnTick;
		}

		private void OnTick()
		{
			if (Enable.Signal == WireSignal.High)
			{
#if DEBUG_STANDALONE
				Console.WriteLine($"{OutIn.Value:B} - {OutIn.Value:D}");
#endif

				OnOutput?.Invoke(OutIn.Value);
			}
		}
	}
}