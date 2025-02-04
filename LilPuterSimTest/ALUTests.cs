using LilPuter;

namespace LilPuterSimTest;

public class ALUTests
{
	private WireManager _manager;

	[SetUp]
	public void Setup()
	{
		_manager = new WireManager();
	}

	[Test]
	public void ALUConfigurationTest()
	{
		var alu = new ArithmeticLogicUnit(_manager, 8);
		//add, don't zero out all inputs (we will set them to zero ourselves)
		alu.SetInputs(WireSignal.High, WireSignal.Low, WireSignal.Low, WireSignal.Low, WireSignal.Low,
			WireSignal.Low);

		//todo: The propagation order is wrong. Zero-ing the inputs means we need intermediary inputs.
		_manager.SetPin(alu.X, new byte[8]);
		_manager.SetPin(alu.Y, new byte[8]);
		_manager.SetPin(alu.F, WireSignal.High);
		Assert.That(PinUtility.ByteArrayToInt(alu.Out.Value), Is.EqualTo(0));
		
	}

	[Test]
	public void OneBitALUConfigurationTest()
	{
		var alu = new ALUOneBit(_manager);
		
		//todo: The propagation order is wrong. Zero-ing the inputs means we need intermediary inputs.
		_manager.SetPin(alu.Op, new byte[2]);//000 is add.
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
		_manager.SetPin(alu.Op, PinUtility.IntToByteArray(1,2)); //001 is and.
		Assert.That(alu.Result.Signal, Is.EqualTo(WireSignal.Low));
		_manager.SetPin(alu.A, WireSignal.High);
		Assert.That(alu.Result.Signal, Is.EqualTo(WireSignal.High));
		_manager.SetPin(alu.B, WireSignal.Low);
		Assert.That(alu.Result.Signal, Is.EqualTo(WireSignal.Low));
		_manager.SetPin(alu.B, WireSignal.High);
		Assert.That(alu.Result.Signal, Is.EqualTo(WireSignal.High));
		
		//some or tests
		_manager.SetPin(alu.Op, PinUtility.IntToByteArray(2, 2)); //010 is or.
		Assert.That(alu.Result.Signal, Is.EqualTo(WireSignal.High));
		_manager.SetPin(alu.B, WireSignal.Low);
		Assert.That(alu.Result.Signal, Is.EqualTo(WireSignal.High));
		_manager.SetPin(alu.A, WireSignal.Low);
		Assert.That(alu.Result.Signal, Is.EqualTo(WireSignal.Low));
		
	}

}