using System;
using LilPuter;
using NUnit.Framework;

namespace LilPuterSimTest
{
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
			_clock.Cycle();
			Assert.That(bit.Output.Signal, Is.EqualTo(WireSignal.High));
			_manager.SetPin(bit.Input, WireSignal.Low);
			_clock.Cycle();
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
			var c = new Counter(_computerBase, "C",width);
			_manager.SetPin(c.LoadInput,0);
			_manager.SetPin(c.LoadEnable, WireSignal.Low);
			_manager.SetPin(c.CountEnable, WireSignal.High);
			_manager.SetPin(c.Reset, WireSignal.Low);
			//Can we cycle through all possible values?
			int m = (int)Math.Pow(2, width);
			for (int i = 0; i < m; i++)
			{
				Assert.That(c.Out.Value, Is.EqualTo(i));
				_clock.Tick();
				//don't update until output the tock.
				Assert.That(c.Out.Value, Is.EqualTo(i));
				_clock.Tock();
			}
			Assert.That(c.Out.Value, Is.EqualTo(0));

			//Can we load a value?
			_manager.SetPin(c.CountEnable, WireSignal.Low);
			_manager.SetPin(c.LoadEnable, WireSignal.High);
			_manager.SetPin(c.LoadInput,width-1);
			_clock.Cycle();
			Assert.That(c.Out.Value, Is.EqualTo(width-1));
			_manager.SetPin(c.CountEnable, WireSignal.High);
			_manager.SetPin(c.LoadEnable, WireSignal.Low);
			_clock.Cycle();
			Assert.That(c.Out.Value, Is.EqualTo(width));
		
			_manager.SetPin(c.Reset, WireSignal.High);
			Assert.That(c.Out.Value, Is.EqualTo(0));

			//Does reset keep our values low regardless?
			_manager.SetPin(c.CountEnable, WireSignal.Low);
			_manager.SetPin(c.LoadEnable, WireSignal.High);
			_manager.SetPin(c.LoadInput, width - 1);
			_clock.Cycle();
			Assert.That(c.Out.Value, Is.EqualTo(0));
			_manager.SetPin(c.CountEnable, WireSignal.High);
			_manager.SetPin(c.LoadEnable, WireSignal.Low);

			_clock.Cycle();
			Assert.That(c.Out.Value, Is.EqualTo(0));
			_clock.Cycle();
			Assert.That(c.Out.Value, Is.EqualTo(0));
		}
	}
}