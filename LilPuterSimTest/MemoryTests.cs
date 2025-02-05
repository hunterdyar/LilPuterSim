using LilPuter;

namespace LilPuterSimTest;

public class MemoryTests
{
	private WireManager _manager;
	private ClockManager _clock;

	[SetUp]
	public void Setup()
	{
		_manager = new WireManager();
		_clock = new ClockManager(_manager);
	}
	
	[Test]
	public void TestDFlipFlop()
	{
		var dff = new DataFlipFlop(_manager,_clock);
		_manager.SetPin(dff.In, WireSignal.High);
		_clock.Tick();
		_clock.Tock();
		Assert.That(dff.Out.Signal, Is.EqualTo(WireSignal.High));

		_manager.SetPin(dff.In, WireSignal.Low);
		_clock.Tick();
		Assert.That(dff.Out.Signal, Is.EqualTo(WireSignal.High));
		_clock.Tock();
		Assert.That(dff.Out.Signal, Is.EqualTo(WireSignal.Low));
		_clock.Cycle();//TickTock a few times to ensure it's stable.
		_clock.Cycle();
		_clock.Cycle();
		Assert.That(dff.Out.Signal, Is.EqualTo(WireSignal.Low));
		_manager.SetPin(dff.In, WireSignal.High);
		_clock.Tick();
		Assert.That(dff.Out.Signal, Is.EqualTo(WireSignal.Low));
		_clock.Tock();
		Assert.That(dff.Out.Signal, Is.EqualTo(WireSignal.High));
	}
}