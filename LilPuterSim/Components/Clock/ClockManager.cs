using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LilPuter.Clock;

namespace LilPuter
{
	//All clock input pins are tied to the same clock signal. It's silly and absurd to do that full simulation inside of the wire manager.
//So I've cloned this part out. 
	public class ClockManager
	{
		public readonly Pin EnableClock;
		public readonly Pin ClockIsHighPin;
		private readonly List<ClockPin> _clocks = new List<ClockPin>();
		private readonly WireManager _wireManager;
		
		public ClockManager(ComputerBase comp)
		{
			_wireManager = comp.WireManager;
			EnableClock = new Pin(comp.WireManager, "Enable Clock Pin");
			ClockIsHighPin = new Pin(comp.WireManager, "Clock High Pin");
			EnableClock.SetSilently(WireSignal.High);
		}

		public void RegisterClockPin(ClockPin pin)
		{
			if (!_clocks.Contains(pin))
			{
				_clocks.Add(pin);
			}
			else
			{
				throw new ArgumentException("Clock pin already registered.");
			}
		}

		public void UnregisterPin(ClockPin pin)
		{
			var didRemove = _clocks.Remove(pin);
			if (!didRemove)
			{
				throw new ArgumentException("Unable to unregister clock pin.");
			}
		}

		/// <summary>
		/// Calls Tick, then Tock. I called this "TickTock" for a while, which I love, but - while clear - it wasn't discoverable.
		/// </summary>
		public void Cycle()
		{
			if (EnableClock.Signal == WireSignal.Low)
			{
				return;
			}

			Tick();
			Tock();
		}

		public void Tick()
		{
			if (EnableClock.Signal == WireSignal.Low)
			{
				return;
			}
			//This feels like the part that we can multithread. Run all of the cores, then run all of the wires to propogate on one thread after.
			foreach (var pin in _clocks)
			{
				pin.TickSilent();
			}

			//Parallel.ForEach(_clocks, pin => pin.TickSilent());
			ClockIsHighPin.Set(WireSignal.High);
			_wireManager.Impulse();
		}

		public void Tock()
		{
			if (EnableClock.Signal == WireSignal.Low)
			{
				return;
			}
			foreach (var pin in _clocks)
			{
				pin.TockSilent();
			}

			//Parallel.ForEach(_clocks, pin => pin.TockSilent());
			ClockIsHighPin.Set(WireSignal.Low);
			_wireManager.Impulse();
		}
	}
}