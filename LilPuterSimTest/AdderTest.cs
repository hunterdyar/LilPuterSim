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
		var a = new Adder(_manager);

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
}