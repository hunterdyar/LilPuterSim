using LilPuter.Clock;

namespace LilPuter;

//All clock input pins are tied to the same clock signal. It's silly and absurd to do that full simulation inside of the wire manager.
//So I've cloned this part out. 
public class ClockManager
{
	private List<ClockPin> _clocks = new List<ClockPin>();
	private WireManager _wireManager;

	public ClockManager(ComputerBase comp)
	{
		_wireManager = comp.WireManager;
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
		Tick();
		Tock();
	}

	public void Tick()
	{
		//This feels like the part that we can multithread. Run all of the cores, then run all of the wires to propogate on one thread after.
		// foreach (var pin in _clocks)
		// {
		// 	pin.TickSilent();
		// }
		Parallel.ForEach(_clocks, pin => pin.TickSilent());
		_wireManager.Impulse();
	}

	public void Tock()
	{
		// foreach (var pin in _clocks)
		// {
		// 	pin.TockSilent();
		// }
		Parallel.ForEach(_clocks, pin => pin.TockSilent());

		_wireManager.Impulse();
	}
}