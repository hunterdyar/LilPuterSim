using LilPuter;
using NUnit.Framework;

namespace LilPuterSimTest
{
	public class ALUTests
	{
		private ComputerBase _computerBase;
		private WireManager _manager => _computerBase.WireManager;

		[SetUp]
		public void Setup()
		{
			_computerBase = new ComputerBase();
		}
	
		[Test]
		public void OneBitALUConfigurationTest()
		{
			var alu = new ALUOneBit(_manager);

			_manager.SetPin(alu.Op, 0); //000 is add.
			_manager.SetPin(alu.A, WireSignal.Low);
			_manager.SetPin(alu.B, WireSignal.Low);
			_manager.SetPin(alu.CarryIn, WireSignal.Low);
			//some add tests
			Assert.That(alu.Result.Signal, Is.EqualTo(WireSignal.Low));
			_manager.SetPin(alu.CarryIn, WireSignal.High);
			Assert.That(alu.Result.Signal, Is.EqualTo(WireSignal.High));
			_manager.SetPin(alu.CarryIn, WireSignal.Low);
			Assert.That(alu.Result.Signal, Is.EqualTo(WireSignal.Low));
			_manager.SetPin(alu.B, WireSignal.High);
			Assert.That(alu.Result.Signal, Is.EqualTo(WireSignal.High));

			//some and tests
			_manager.SetPin(alu.Op, 1); //001 is and.
			Assert.That(alu.Result.Signal, Is.EqualTo(WireSignal.Low));
			_manager.SetPin(alu.A, WireSignal.High);
			Assert.That(alu.Result.Signal, Is.EqualTo(WireSignal.High));
			_manager.SetPin(alu.B, WireSignal.Low);
			Assert.That(alu.Result.Signal, Is.EqualTo(WireSignal.Low));
			_manager.SetPin(alu.B, WireSignal.High);
			Assert.That(alu.Result.Signal, Is.EqualTo(WireSignal.High));

			//some or tests
			_manager.SetPin(alu.Op, 2); //010 is or.
			Assert.That(alu.Result.Signal, Is.EqualTo(WireSignal.High));
			_manager.SetPin(alu.B, WireSignal.Low);
			Assert.That(alu.Result.Signal, Is.EqualTo(WireSignal.High));
			_manager.SetPin(alu.A, WireSignal.Low);
			Assert.That(alu.Result.Signal, Is.EqualTo(WireSignal.Low));

		}
	
		[Test]
		[TestCase(2)]
		[TestCase(4)]
		[TestCase(8)]
		[TestCase(16)]
		public void ALUConfigurationTest(int width)
		{
			var alu = new ALUMultiBit(_computerBase, width);
			//add, don't zero out all inputs (we will set them to zero ourselves)
			_manager.SetPin(alu.InvertA,WireSignal.Low);
			_manager.SetPin(alu.InvertB,WireSignal.Low);
			_manager.SetPin(alu.A, 0);
			_manager.SetPin(alu.B,0);
		
			_manager.SetPin(alu.Operation, 0);//000 is add
			Assert.That(alu.Result.Value, Is.EqualTo(0));
		}

		[Test]
		public void ALUAddViaCPURegistersTest()
		{
			_computerBase.CPU.A.Load.SetAndImpulse(WireSignal.High);
			_computerBase.CPU.A.Input.SetAndImpulse(32);
			_computerBase.CPU.A.Load.SetAndImpulse(WireSignal.Low);
			_computerBase.CPU.B.Load.SetAndImpulse(WireSignal.High);
			_computerBase.CPU.B.Input.SetAndImpulse(40);
			_computerBase.CPU.B.Load.SetAndImpulse(WireSignal.Low);
			_computerBase.CPU.ALU.Operation.SetAndImpulse(0);
		
			Assert.That(_computerBase.CPU.ALU.Result.Value, Is.EqualTo(72));
		}

	}
}