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

}