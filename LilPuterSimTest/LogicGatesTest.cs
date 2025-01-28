using LilPuter;

namespace LilPuterSimTest;

public class Tests
{
	[SetUp]
	public void Setup()
	{
	}

	[Test]
	public void NandLogicTable()
	{
		var n = new NandGate();
		n.A.Set(WireSignal.Low);
		n.B.Set(WireSignal.Low);
		Assert.That((WireSignal)n.Out.Value[0], Is.EqualTo(WireSignal.High));
		Assert.That(n.Out.Value.Length, Is.EqualTo(1));

		n.A.Set(WireSignal.Low);
		n.B.Set(WireSignal.High);
		Assert.That((WireSignal)n.Out.Value[0], Is.EqualTo(WireSignal.High));
		Assert.That(n.Out.Value.Length, Is.EqualTo(1));

		n.A.Set(WireSignal.High);
		n.B.Set(WireSignal.Low);
		Assert.That((WireSignal)n.Out.Value[0], Is.EqualTo(WireSignal.High));
		Assert.That(n.Out.Value.Length, Is.EqualTo(1));

		n.A.Set(WireSignal.High);
		n.B.Set(WireSignal.High);
		Assert.That((WireSignal)n.Out.Value[0], Is.EqualTo(WireSignal.Low));
		Assert.That(n.Out.Value.Length, Is.EqualTo(1));
	}

	[Test]
	public void AndLogicTable()
	{
		var andGate = new AndGate();
		andGate.A.Set(WireSignal.Low);
		andGate.B.Set(WireSignal.Low);
		Assert.That((WireSignal)andGate.Out.Value[0], Is.EqualTo(WireSignal.Low));
		Assert.That(andGate.Out.Value.Length, Is.EqualTo(1));

		andGate.A.Set(WireSignal.Low);
		andGate.B.Set(WireSignal.High);
		Assert.That((WireSignal)andGate.Out.Value[0], Is.EqualTo(WireSignal.Low));
		Assert.That(andGate.Out.Value.Length, Is.EqualTo(1));

		andGate.A.Set(WireSignal.High);
		andGate.B.Set(WireSignal.Low);
		Assert.That((WireSignal)andGate.Out.Value[0], Is.EqualTo(WireSignal.Low));
		Assert.That(andGate.Out.Value.Length, Is.EqualTo(1));

		andGate.A.Set(WireSignal.High);
		andGate.B.Set(WireSignal.High);
		Assert.That((WireSignal)andGate.Out.Value[0], Is.EqualTo(WireSignal.High));
		Assert.That(andGate.Out.Value.Length, Is.EqualTo(1));
	}

	[Test]
	public void OrLogicTable()
	{
		var orGate = new OrGate();
		orGate.A.Set(WireSignal.Low);
		orGate.B.Set(WireSignal.Low);
		Assert.That((WireSignal)orGate.Out.Value[0], Is.EqualTo(WireSignal.Low));
		Assert.That(orGate.Out.Value.Length, Is.EqualTo(1));

		orGate.A.Set(WireSignal.Low);
		orGate.B.Set(WireSignal.High);
		Assert.That((WireSignal)orGate.Out.Value[0], Is.EqualTo(WireSignal.High));
		Assert.That(orGate.Out.Value.Length, Is.EqualTo(1));

		orGate.A.Set(WireSignal.High);
		orGate.B.Set(WireSignal.Low);
		Assert.That((WireSignal)orGate.Out.Value[0], Is.EqualTo(WireSignal.High));
		Assert.That(orGate.Out.Value.Length, Is.EqualTo(1));

		orGate.A.Set(WireSignal.High);
		orGate.B.Set(WireSignal.High);
		Assert.That((WireSignal)orGate.Out.Value[0], Is.EqualTo(WireSignal.High));
		Assert.That(orGate.Out.Value.Length, Is.EqualTo(1));
	}
}