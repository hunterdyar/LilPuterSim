using LilPuter;

namespace LilPuterSimTest;

public class MemoryTests
{
	private ComputerBase _computerBase;
	private WireManager _manager => _computerBase.WireManager;
	private ClockManager _clock => _computerBase.Clock;
	[SetUp]
	public void Setup()
	{
		_computerBase = new ComputerBase();
	}

	[Test]
	public void BitTest()
	{
		Bit bit = new Bit(_manager, _clock);
		//setup
		_manager.SetPin(bit.Input, WireSignal.Low);
		_manager.SetPin(bit.Load, WireSignal.High);
		_clock.Cycle();
		Assert.That(bit.Output.Signal, Is.EqualTo(WireSignal.Low));
		_manager.SetPin(bit.Input, WireSignal.High);
		
		//output doesn't change until we cycle
		Assert.That(bit.Output.Signal, Is.EqualTo(WireSignal.Low));
		_clock.Tick();
		Assert.That(bit.Output.Signal, Is.EqualTo(WireSignal.Low));
		_clock.Tock();
		Assert.That(bit.Output.Signal, Is.EqualTo(WireSignal.High));
		_manager.SetPin(bit.Input, WireSignal.Low);
		Assert.That(bit.Output.Signal, Is.EqualTo(WireSignal.High));//doesn't change instantly
		_clock.Tick();
		Assert.That(bit.Output.Signal, Is.EqualTo(WireSignal.High));//doesn't change on tick
		_clock.Tock();
		Assert.That(bit.Output.Signal, Is.EqualTo(WireSignal.Low));//changes on tock
		
		//Test that nothing happens when load is off.
		_manager.SetPin(bit.Input, WireSignal.High);
		_clock.Cycle();
		//
		_manager.SetPin(bit.Load, WireSignal.Low);
		_manager.SetPin(bit.Input, WireSignal.Low);
		Assert.That(bit.Output.Signal, Is.EqualTo(WireSignal.High));
		_clock.Cycle();
		Assert.That(bit.Output.Signal, Is.EqualTo(WireSignal.High));
		_clock.Cycle();
		Assert.That(bit.Output.Signal, Is.EqualTo(WireSignal.High));
		_manager.SetPin(bit.Input, WireSignal.Low);
		_clock.Cycle();
		Assert.That(bit.Output.Signal, Is.EqualTo(WireSignal.High));
		_manager.SetPin(bit.Input, WireSignal.High);
		_clock.Cycle();
		Assert.That(bit.Output.Signal, Is.EqualTo(WireSignal.High));
		
	}
}