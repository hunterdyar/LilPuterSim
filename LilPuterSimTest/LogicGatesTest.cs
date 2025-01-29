using LilPuter;

namespace LilPuterSimTest;

public class Tests
{

	private WireManager _manager;
	[SetUp]
	public void Setup()
	{
		_manager = new WireManager();
	}

	[Test]
	public void NandLogicTable()
	{
		var n = new NandGate(_manager);
		
		_manager.SetPin(n.A, WireSignal.Low);
		_manager.SetPin(n.B, WireSignal.Low);
		Assert.That((WireSignal)n.Out.Value[0], Is.EqualTo(WireSignal.High));
		Assert.That(n.Out.Value.Length, Is.EqualTo(1));

		_manager.SetPin(n.A, WireSignal.Low);
		_manager.SetPin(n.B, WireSignal.High);
		Assert.That((WireSignal)n.Out.Value[0], Is.EqualTo(WireSignal.High));
		Assert.That(n.Out.Value.Length, Is.EqualTo(1));

		_manager.SetPin(n.A, WireSignal.High);
		_manager.SetPin(n.B, WireSignal.Low);
		Assert.That((WireSignal)n.Out.Value[0], Is.EqualTo(WireSignal.High));
		Assert.That(n.Out.Value.Length, Is.EqualTo(1));

		_manager.SetPin(n.A, WireSignal.High);
		_manager.SetPin(n.B, WireSignal.High);
		Assert.That((WireSignal)n.Out.Value[0], Is.EqualTo(WireSignal.Low));
		Assert.That(n.Out.Value.Length, Is.EqualTo(1));
	}

	[Test]
	public void AndLogicTable()
	{
		var andGate = new AndGate(_manager);
		_manager.SetPin(andGate.A, WireSignal.Low);
		_manager.SetPin(andGate.B, WireSignal.Low);
		Assert.That((WireSignal)andGate.Out.Value[0], Is.EqualTo(WireSignal.Low));
		Assert.That(andGate.Out.Value.Length, Is.EqualTo(1));

		_manager.SetPin(andGate.A, WireSignal.Low);
		_manager.SetPin(andGate.B, WireSignal.High);
		Assert.That((WireSignal)andGate.Out.Value[0], Is.EqualTo(WireSignal.Low));
		Assert.That(andGate.Out.Value.Length, Is.EqualTo(1));

		_manager.SetPin(andGate.A, WireSignal.High);
		_manager.SetPin(andGate.B, WireSignal.Low);
		Assert.That((WireSignal)andGate.Out.Value[0], Is.EqualTo(WireSignal.Low));
		Assert.That(andGate.Out.Value.Length, Is.EqualTo(1));

		_manager.SetPin(andGate.A, WireSignal.High);
		_manager.SetPin(andGate.B, WireSignal.High);
		Assert.That((WireSignal)andGate.Out.Value[0], Is.EqualTo(WireSignal.High));
		Assert.That(andGate.Out.Value.Length, Is.EqualTo(1));
	}

	[Test]
	public void OrLogicTable()
	{
		var orGate = new OrGate(_manager);
		_manager.SetPin(orGate.A, WireSignal.Low);
		_manager.SetPin(orGate.B, WireSignal.Low);
		
		Assert.That((WireSignal)orGate.Out.Value[0], Is.EqualTo(WireSignal.Low));
		Assert.That(orGate.Out.Value.Length, Is.EqualTo(1));
		
		_manager.SetPin(orGate.A, WireSignal.Low);
		_manager.SetPin(orGate.B, WireSignal.High);
		Assert.That((WireSignal)orGate.Out.Value[0], Is.EqualTo(WireSignal.High));
		Assert.That(orGate.Out.Value.Length, Is.EqualTo(1));

		_manager.SetPin(orGate.A, WireSignal.High);
		_manager.SetPin(orGate.B, WireSignal.Low);
		Assert.That((WireSignal)orGate.Out.Value[0], Is.EqualTo(WireSignal.High));
		Assert.That(orGate.Out.Value.Length, Is.EqualTo(1));

		_manager.SetPin(orGate.A, WireSignal.High);
		_manager.SetPin(orGate.B, WireSignal.High);
		Assert.That((WireSignal)orGate.Out.Value[0], Is.EqualTo(WireSignal.High));
		Assert.That(orGate.Out.Value.Length, Is.EqualTo(1));
	}

	[Test]
	public void NotLogicTable()
	{
		var orGate = new NotGate(_manager);
		orGate.A.Set(WireSignal.Low);
		_manager.Impulse(orGate.A);
		Assert.That((WireSignal)orGate.Out.Value[0], Is.EqualTo(WireSignal.High));
		Assert.That(orGate.Out.Value.Length, Is.EqualTo(1));

		orGate.A.Set(WireSignal.High);
		_manager.Impulse(orGate.A);
		Assert.That((WireSignal)orGate.Out.Value[0], Is.EqualTo(WireSignal.Low));
		Assert.That(orGate.Out.Value.Length, Is.EqualTo(1));

	}
	
}