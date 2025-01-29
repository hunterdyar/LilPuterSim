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
	public void NorLogicTable()
	{
		var norGate = new NorGate(_manager);
		
		_manager.SetPin(norGate.A, WireSignal.Low);
		_manager.SetPin(norGate.B, WireSignal.Low);
		
		Assert.That((WireSignal)norGate.Out.Value[0], Is.EqualTo(WireSignal.High));
		Assert.That(norGate.Out.Value.Length, Is.EqualTo(1));

		_manager.SetPin(norGate.A, WireSignal.Low);
		_manager.SetPin(norGate.B, WireSignal.High);
		Assert.That((WireSignal)norGate.Out.Value[0], Is.EqualTo(WireSignal.Low));
		Assert.That(norGate.Out.Value.Length, Is.EqualTo(1));

		_manager.SetPin(norGate.A, WireSignal.High);
		_manager.SetPin(norGate.B, WireSignal.Low);
		Assert.That((WireSignal)norGate.Out.Value[0], Is.EqualTo(WireSignal.Low));
		Assert.That(norGate.Out.Value.Length, Is.EqualTo(1));
		
		_manager.SetPin(norGate.A, WireSignal.High);
		_manager.SetPin(norGate.B, WireSignal.High);
		Assert.That((WireSignal)norGate.Out.Value[0], Is.EqualTo(WireSignal.Low));
		Assert.That(norGate.Out.Value.Length, Is.EqualTo(1));
	}

	[Test]
	public void XOrLogicTable()
	{
		var xorGate = new XorGate(_manager);

		_manager.SetPin(xorGate.A, WireSignal.Low);
		_manager.SetPin(xorGate.B, WireSignal.Low);
		Assert.That((WireSignal)xorGate.Out.Value[0], Is.EqualTo(WireSignal.Low));
		Assert.That(xorGate.Out.Value.Length, Is.EqualTo(1));

		_manager.SetPin(xorGate.A, WireSignal.Low);
		_manager.SetPin(xorGate.B, WireSignal.High);
		Assert.That((WireSignal)xorGate.Out.Value[0], Is.EqualTo(WireSignal.High));
		Assert.That(xorGate.Out.Value.Length, Is.EqualTo(1));

		_manager.SetPin(xorGate.A, WireSignal.High);
		_manager.SetPin(xorGate.B, WireSignal.Low);
		Assert.That((WireSignal)xorGate.Out.Value[0], Is.EqualTo(WireSignal.High));
		Assert.That(xorGate.Out.Value.Length, Is.EqualTo(1));

		_manager.SetPin(xorGate.A, WireSignal.High);
		_manager.SetPin(xorGate.B, WireSignal.High);
		Assert.That((WireSignal)xorGate.Out.Value[0], Is.EqualTo(WireSignal.Low));
		Assert.That(xorGate.Out.Value.Length, Is.EqualTo(1));
	}

	[Test]
	public void XNORLogicTable()
	{
		var xnorGate = new XnorGate(_manager);

		_manager.SetPin(xnorGate.A, WireSignal.Low);
		_manager.SetPin(xnorGate.B, WireSignal.Low);
		Assert.That((WireSignal)xnorGate.Out.Value[0], Is.EqualTo(WireSignal.High));
		Assert.That(xnorGate.Out.Value.Length, Is.EqualTo(1));

		_manager.SetPin(xnorGate.A, WireSignal.Low);
		_manager.SetPin(xnorGate.B, WireSignal.High);
		Assert.That((WireSignal)xnorGate.Out.Value[0], Is.EqualTo(WireSignal.Low));
		Assert.That(xnorGate.Out.Value.Length, Is.EqualTo(1));

		_manager.SetPin(xnorGate.A, WireSignal.High);
		_manager.SetPin(xnorGate.B, WireSignal.Low);
		Assert.That((WireSignal)xnorGate.Out.Value[0], Is.EqualTo(WireSignal.Low));
		Assert.That(xnorGate.Out.Value.Length, Is.EqualTo(1));

		_manager.SetPin(xnorGate.A, WireSignal.High);
		_manager.SetPin(xnorGate.B, WireSignal.High);
		Assert.That((WireSignal)xnorGate.Out.Value[0], Is.EqualTo(WireSignal.High));
		Assert.That(xnorGate.Out.Value.Length, Is.EqualTo(1));
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