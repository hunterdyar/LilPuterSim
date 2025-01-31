using LilPuter;

namespace LilPuterSimTest;

public class AdderTest
{
	private WireManager _manager;

	[SetUp]
	public void Setup()
	{
		_manager = new WireManager();
	}


	[Test]
	public void HalfAdderLogicTableTests()
	{
		var ha = new HalfAdder(_manager);

		_manager.SetPin(ha.A, WireSignal.Low);
		_manager.SetPin(ha.B, WireSignal.Low);
		Assert.That((WireSignal)ha.CarryOut.Value[0], Is.EqualTo(WireSignal.Low));
		Assert.That(ha.CarryOut.Value.Length, Is.EqualTo(1));
		Assert.That((WireSignal)ha.SumOut.Value[0], Is.EqualTo(WireSignal.Low));
		Assert.That(ha.SumOut.Value.Length, Is.EqualTo(1));

		_manager.SetPin(ha.B, WireSignal.High);
		Assert.That((WireSignal)ha.CarryOut.Value[0], Is.EqualTo(WireSignal.Low));
		Assert.That(ha.CarryOut.Value.Length, Is.EqualTo(1));
		Assert.That((WireSignal)ha.SumOut.Value[0], Is.EqualTo(WireSignal.High));
		Assert.That(ha.SumOut.Value.Length, Is.EqualTo(1));

		_manager.SetPin(ha.A, WireSignal.High);
		_manager.SetPin(ha.B, WireSignal.Low);
		Assert.That((WireSignal)ha.CarryOut.Value[0], Is.EqualTo(WireSignal.Low));
		Assert.That(ha.CarryOut.Value.Length, Is.EqualTo(1));
		Assert.That((WireSignal)ha.SumOut.Value[0], Is.EqualTo(WireSignal.High));
		Assert.That(ha.SumOut.Value.Length, Is.EqualTo(1));

		_manager.SetPin(ha.A, WireSignal.High);
		_manager.SetPin(ha.B, WireSignal.High);
		Assert.That((WireSignal)ha.CarryOut.Value[0], Is.EqualTo(WireSignal.High));
		Assert.That(ha.CarryOut.Value.Length, Is.EqualTo(1));
		Assert.That((WireSignal)ha.SumOut.Value[0], Is.EqualTo(WireSignal.Low));
		Assert.That(ha.SumOut.Value.Length, Is.EqualTo(1));
	}

	[Test]
	public void AdderLogicTableTests()
	{
		var a = new FullAdder(_manager);

		_manager.SetPin(a.A, WireSignal.Low);
		_manager.SetPin(a.B, WireSignal.Low);
		_manager.SetPin(a.CarryIn, WireSignal.Low);
		Assert.That((WireSignal)a.CarryOut.Value[0], Is.EqualTo(WireSignal.Low));
		Assert.That(a.CarryOut.Value.Length, Is.EqualTo(1));
		Assert.That((WireSignal)a.SumOut.Value[0], Is.EqualTo(WireSignal.Low));
		Assert.That(a.SumOut.Value.Length, Is.EqualTo(1));

		_manager.SetPin(a.A, WireSignal.Low);
		_manager.SetPin(a.B, WireSignal.Low);
		_manager.SetPin(a.CarryIn, WireSignal.High);
		Assert.That((WireSignal)a.CarryOut.Value[0], Is.EqualTo(WireSignal.Low));
		Assert.That(a.CarryOut.Value.Length, Is.EqualTo(1));
		Assert.That((WireSignal)a.SumOut.Value[0], Is.EqualTo(WireSignal.High));
		Assert.That(a.SumOut.Value.Length, Is.EqualTo(1));

		_manager.SetPin(a.A, WireSignal.Low);
		_manager.SetPin(a.B, WireSignal.High);
		_manager.SetPin(a.CarryIn, WireSignal.Low);
		Assert.That((WireSignal)a.CarryOut.Value[0], Is.EqualTo(WireSignal.Low));
		Assert.That(a.CarryOut.Value.Length, Is.EqualTo(1));
		Assert.That((WireSignal)a.SumOut.Value[0], Is.EqualTo(WireSignal.High));
		Assert.That(a.SumOut.Value.Length, Is.EqualTo(1));

		_manager.SetPin(a.A, WireSignal.Low);
		_manager.SetPin(a.B, WireSignal.High);
		_manager.SetPin(a.CarryIn, WireSignal.High);
		Assert.That((WireSignal)a.CarryOut.Value[0], Is.EqualTo(WireSignal.High));
		Assert.That(a.CarryOut.Value.Length, Is.EqualTo(1));
		Assert.That((WireSignal)a.SumOut.Value[0], Is.EqualTo(WireSignal.Low));
		Assert.That(a.SumOut.Value.Length, Is.EqualTo(1));

		_manager.SetPin(a.A, WireSignal.High);
		_manager.SetPin(a.B, WireSignal.Low);
		_manager.SetPin(a.CarryIn, WireSignal.Low);
		Assert.That((WireSignal)a.CarryOut.Value[0], Is.EqualTo(WireSignal.Low));
		Assert.That(a.CarryOut.Value.Length, Is.EqualTo(1));
		Assert.That((WireSignal)a.SumOut.Value[0], Is.EqualTo(WireSignal.High));
		Assert.That(a.SumOut.Value.Length, Is.EqualTo(1));

		_manager.SetPin(a.A, WireSignal.High);
		_manager.SetPin(a.B, WireSignal.Low);
		_manager.SetPin(a.CarryIn, WireSignal.High);
		Assert.That((WireSignal)a.CarryOut.Value[0], Is.EqualTo(WireSignal.High));
		Assert.That(a.CarryOut.Value.Length, Is.EqualTo(1));
		Assert.That((WireSignal)a.SumOut.Value[0], Is.EqualTo(WireSignal.Low));
		Assert.That(a.SumOut.Value.Length, Is.EqualTo(1));

		_manager.SetPin(a.A, WireSignal.High);
		_manager.SetPin(a.B, WireSignal.High);
		_manager.SetPin(a.CarryIn, WireSignal.Low);
		Assert.That((WireSignal)a.CarryOut.Value[0], Is.EqualTo(WireSignal.High));
		Assert.That(a.CarryOut.Value.Length, Is.EqualTo(1));
		Assert.That((WireSignal)a.SumOut.Value[0], Is.EqualTo(WireSignal.Low));
		Assert.That(a.SumOut.Value.Length, Is.EqualTo(1));

		_manager.SetPin(a.A, WireSignal.High);
		_manager.SetPin(a.B, WireSignal.High);
		_manager.SetPin(a.CarryIn, WireSignal.High);
		Assert.That((WireSignal)a.CarryOut.Value[0], Is.EqualTo(WireSignal.High));
		Assert.That(a.CarryOut.Value.Length, Is.EqualTo(1));
		Assert.That((WireSignal)a.SumOut.Value[0], Is.EqualTo(WireSignal.High));
		Assert.That(a.SumOut.Value.Length, Is.EqualTo(1));
	}

	[Test]
	public void AdderHookupTests()
	{
		int width = 8;
		var adder = new Adder(_manager, width);
		//zero out
		_manager.SetPin(adder.CarryIn, WireSignal.Low);
		_manager.SetPin(adder.A, new byte[width]);
		_manager.SetPin(adder.B, new byte[width]);
		
		for (int i = 0; i < width; i++)
		{
			Assert.That((WireSignal)adder.Out.Value[i],Is.EqualTo(WireSignal.Low));
		}
	}

	[Test]
	[TestCase(2)]
	[TestCase(4)]

	public void AdderSumAllForWidth(int width)
	{
		int maxCanSum = ((int)Math.Pow(2,width)) -1;
		var adder = new Adder(_manager, width);
		//zero out
		_manager.SetPin(adder.CarryIn, WireSignal.Low);
		_manager.SetPin(adder.A, new byte[width]);
		_manager.SetPin(adder.B, new byte[width]);

		for (int x = 0; x < maxCanSum; x++)
		{
			for (int y = 0; y < maxCanSum; y++)
			{
				_manager.SetPin(adder.A, PinUtility.IntToByteArray(x, width));
				_manager.SetPin(adder.B, PinUtility.IntToByteArray(y, width));
				var result = PinUtility.ByteArrayToInt(adder.Out.Value);
				if (x + y > maxCanSum)
				{
					Assert.That(result, Is.EqualTo((x + y) - (maxCanSum+1)));
					Assert.That((adder.CarryOut.Signal), Is.EqualTo(WireSignal.High));
				}
				else
				{
					Assert.That(result, Is.EqualTo((x + y)));
					Assert.That((adder.CarryOut.Signal), Is.EqualTo(WireSignal.Low));
				}
			}
		}
	}

	[Test]
	public void ALUConfigurationTest()
	{
		var alu = new ArithmeticLogicUnit(_manager, 8);
		//add, zero inputs.
		alu.SetInputs(WireSignal.High,WireSignal.High,WireSignal.Low,WireSignal.High,WireSignal.Low,WireSignal.Low);
		Assert.That(PinUtility.ByteArrayToInt(alu.Out.Value), Is.EqualTo(0));
		
	}
}