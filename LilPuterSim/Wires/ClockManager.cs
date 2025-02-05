using LilPuter.Clock;

namespace LilPuter;

//All clock input pins are tied to the same clock signal. It's silly and absurd to do that full simulation inside of the wire manager.
//So I've cloned this part out. 
public class ClockManager
{
	private List<ClockPin> _clocks = new List<ClockPin>();
	private WireManager _wireManager;

	public ClockManager(WireManager wireManager)
	{
		_wireManager = wireManager;
	}

	public void RegisterPin(ClockPin pin)
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

	public void Cycle()
	{
		Tick();
		Tock();
	}

	public void Tick()
	{
		foreach (var pin in _clocks)
		{
			pin.TickSilent();
		}
		
		_wireManager.Impulse();
	}

	public void Tock()
	{
		foreach (var pin in _clocks)
		{
			pin.TockSilent();
		}
		_wireManager.Impulse();
	}
}