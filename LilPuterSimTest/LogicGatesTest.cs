using LilPuter;

namespace LilPuterSimTest;

public class LogicGatesTests
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
	public void XnorLogicTable()
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
		var notGate = new NotGate(_manager);

		_manager.SetPin(notGate.A, WireSignal.Low);
		Assert.That((WireSignal)notGate.Out.Value[0], Is.EqualTo(WireSignal.High));
		Assert.That(notGate.Out.Value.Length, Is.EqualTo(1));

		_manager.SetPin(notGate.A, WireSignal.High);
		Assert.That((WireSignal)notGate.Out.Value[0], Is.EqualTo(WireSignal.Low));
		Assert.That(notGate.Out.Value.Length, Is.EqualTo(1));

	}

	[TestOf(typeof(AndGate))]
	[Test]
	
	public void AndGateSortTests()
	{
		var andGate = new AndGate(_manager);
		var s = _manager.GetTopoSort();
		var a = s.IndexOf(andGate.A);
		var b = s.IndexOf(andGate.B);
		var o = s.IndexOf(andGate.Out);
		Assert.That(a, Is.LessThan(o));
		Assert.That(b, Is.LessThan(o));
	}

	[Test]

	public void NandGateSortTests()
	{
		var gate = new NandGate(_manager);
		var s = _manager.GetTopoSort();
		var a = s.IndexOf(gate.A);
		var b = s.IndexOf(gate.B);
		var o = s.IndexOf(gate.Out);
		Assert.That(a, Is.LessThan(o));
		Assert.That(b, Is.LessThan(o));
	}

	[Test]

	public void XorGateSortTests()
	{
		var gate = new XorGate(_manager);
		Assert.True(_manager.ValidateTopoSort());
		var s = _manager.GetTopoSort();
		var a = s.IndexOf(gate.A);
		var b = s.IndexOf(gate.B);
		var o = s.IndexOf(gate.Out);
		Assert.That(a, Is.LessThan(o));
		Assert.That(b, Is.LessThan(o));
	}

	[Test]

	public void XnorGateSortTests()
	{
		var gate = new XnorGate(_manager);
		Assert.True(_manager.ValidateTopoSort());
		var s = _manager.GetTopoSort();
		var a = s.IndexOf(gate.A);
		var b = s.IndexOf(gate.B);
		var o = s.IndexOf(gate.Out);
		Assert.That(a, Is.LessThan(o));
		Assert.That(b, Is.LessThan(o));
	}

	[Test]

	public void OrGateSortTests()
	{
		var gate = new OrGate(_manager);
		Assert.True(_manager.ValidateTopoSort());
		var s = _manager.GetTopoSort();
		var a = s.IndexOf(gate.A);
		var b = s.IndexOf(gate.B);
		var o = s.IndexOf(gate.Out);
		Assert.That(a, Is.LessThan(o));
		Assert.That(b, Is.LessThan(o));
	}

	[Test]

	public void NotGateSortTests()
	{
		var gate = new NotGate(_manager);
		Assert.True(_manager.ValidateTopoSort());
		var s = _manager.GetTopoSort();
		var a = s.IndexOf(gate.A);
		var o = s.IndexOf(gate.Out);
		Assert.That(a, Is.LessThan(o));
	}


}