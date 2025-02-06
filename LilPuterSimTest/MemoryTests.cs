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
		Bit bit = new Bit(_computerBase);
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

	[Test]
	[TestCase(2)]
	[TestCase(4)]
	[TestCase(8)]
	// [TestCase(16)]//this one takes a lil longer :p
	public void CounterTest(int width)
	{
		var c = new Counter(_computerBase,width);
		_manager.SetPin(c.Input,new byte[width]);
		_manager.SetPin(c.CountEnable, WireSignal.High);

		//Can we cycle through all possible values?
		int m = (int)Math.Pow(2, width);
		for (int i = 0; i < m; i++)
		{
			Assert.That(PinUtility.ByteArrayToInt(c.Out.Value), Is.EqualTo(i));
			_clock.Tick();
			//don't update until output the tock.
			Assert.That(PinUtility.ByteArrayToInt(c.Out.Value), Is.EqualTo(i));
			_clock.Tock();
		}
		Assert.That(PinUtility.ByteArrayToInt(c.Out.Value), Is.EqualTo(0));

		//Can we load a value?
		_manager.SetPin(c.CountEnable, WireSignal.Low);
		_manager.SetPin(c.Input, PinUtility.IntToByteArray(width-1,width));
		_clock.Cycle();
		Assert.That(PinUtility.ByteArrayToInt(c.Out.Value), Is.EqualTo(width-1));
		_manager.SetPin(c.CountEnable, WireSignal.High);
		_clock.Cycle();
		Assert.That(PinUtility.ByteArrayToInt(c.Out.Value), Is.EqualTo(width));
		//todo: What happens when both load and count are enabled?
	}
}