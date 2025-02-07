using LilPuter;
using NUnit.Framework.Constraints;

namespace LilPuterSimTest;

public class AdderTest
{
	private ComputerBase _computerBase;
	private WireManager _manager => _computerBase.WireManager;

	[SetUp]
	public void Setup()
	{
		_computerBase = new ComputerBase();
	}


	[Test]
	public void HalfAdderLogicTableTests()
	{
		var ha = new HalfAdder(_manager);

		_manager.SetPin(ha.A, WireSignal.Low);
		_manager.SetPin(ha.B, WireSignal.Low);
		Assert.That((WireSignal)ha.CarryOut.Value, Is.EqualTo(WireSignal.Low));
		Assert.That(ha.CarryOut.PinType, Is.EqualTo(PinType.Single));
		Assert.That((WireSignal)ha.SumOut.Value, Is.EqualTo(WireSignal.Low));
		Assert.That(ha.SumOut.PinType, Is.EqualTo(PinType.Single));

		_manager.SetPin(ha.B, WireSignal.High);
		Assert.That((WireSignal)ha.CarryOut.Value, Is.EqualTo(WireSignal.Low));
		Assert.That(ha.CarryOut.PinType, Is.EqualTo(PinType.Single));
		Assert.That((WireSignal)ha.SumOut.Value, Is.EqualTo(WireSignal.High));
		Assert.That(ha.SumOut.PinType, Is.EqualTo(PinType.Single));

		_manager.SetPin(ha.A, WireSignal.High);
		_manager.SetPin(ha.B, WireSignal.Low);
		Assert.That((WireSignal)ha.CarryOut.Value, Is.EqualTo(WireSignal.Low));
		Assert.That(ha.CarryOut.PinType, Is.EqualTo(PinType.Single));
		Assert.That((WireSignal)ha.SumOut.Value, Is.EqualTo(WireSignal.High));
		Assert.That(ha.SumOut.PinType, Is.EqualTo(PinType.Single));

		_manager.SetPin(ha.A, WireSignal.High);
		_manager.SetPin(ha.B, WireSignal.High);
		Assert.That((WireSignal)ha.CarryOut.Value, Is.EqualTo(WireSignal.High));
		Assert.That(ha.CarryOut.PinType, Is.EqualTo(PinType.Single));
		Assert.That((WireSignal)ha.SumOut.Value, Is.EqualTo(WireSignal.Low));
		Assert.That(ha.SumOut.PinType, Is.EqualTo(PinType.Single));
	}
	
	[Test]
	public void FullAdderLogicTableTests()
	{
		var a = new FullAdder(_manager);

		_manager.SetPin(a.A, WireSignal.Low);
		_manager.SetPin(a.B, WireSignal.Low);
		_manager.SetPin(a.CarryIn, WireSignal.Low);
		Assert.That((WireSignal)a.CarryOut.Value, Is.EqualTo(WireSignal.Low));
		Assert.That(a.CarryOut.PinType, Is.EqualTo(PinType.Single));
		Assert.That((WireSignal)a.SumOut.Value, Is.EqualTo(WireSignal.Low));
		Assert.That(a.SumOut.PinType, Is.EqualTo(PinType.Single));

		_manager.SetPin(a.A, WireSignal.Low);
		_manager.SetPin(a.B, WireSignal.Low);
		_manager.SetPin(a.CarryIn, WireSignal.High);
		Assert.That((WireSignal)a.CarryOut.Value, Is.EqualTo(WireSignal.Low));
		Assert.That(a.CarryOut.PinType, Is.EqualTo(PinType.Single));
		Assert.That((WireSignal)a.SumOut.Value, Is.EqualTo(WireSignal.High));
		Assert.That(a.SumOut.PinType, Is.EqualTo(PinType.Single));

		_manager.SetPin(a.A, WireSignal.Low);
		_manager.SetPin(a.B, WireSignal.High);
		_manager.SetPin(a.CarryIn, WireSignal.Low);
		Assert.That((WireSignal)a.CarryOut.Value, Is.EqualTo(WireSignal.Low));
		Assert.That(a.CarryOut.PinType, Is.EqualTo(PinType.Single));
		Assert.That((WireSignal)a.SumOut.Value, Is.EqualTo(WireSignal.High));
		Assert.That(a.SumOut.PinType, Is.EqualTo(PinType.Single));

		_manager.SetPin(a.A, WireSignal.Low);
		_manager.SetPin(a.B, WireSignal.High);
		_manager.SetPin(a.CarryIn, WireSignal.High);
		Assert.That((WireSignal)a.CarryOut.Value, Is.EqualTo(WireSignal.High));
		Assert.That(a.CarryOut.PinType, Is.EqualTo(PinType.Single));
		Assert.That((WireSignal)a.SumOut.Value, Is.EqualTo(WireSignal.Low));
		Assert.That(a.SumOut.PinType, Is.EqualTo(PinType.Single));

		_manager.SetPin(a.A, WireSignal.High);
		_manager.SetPin(a.B, WireSignal.Low);
		_manager.SetPin(a.CarryIn, WireSignal.Low);
		Assert.That((WireSignal)a.CarryOut.Value, Is.EqualTo(WireSignal.Low));
		Assert.That(a.CarryOut.PinType, Is.EqualTo(PinType.Single));
		Assert.That((WireSignal)a.SumOut.Value, Is.EqualTo(WireSignal.High));
		Assert.That(a.SumOut.PinType, Is.EqualTo(PinType.Single));

		_manager.SetPin(a.A, WireSignal.High);
		_manager.SetPin(a.B, WireSignal.Low);
		_manager.SetPin(a.CarryIn, WireSignal.High);
		Assert.That((WireSignal)a.CarryOut.Value, Is.EqualTo(WireSignal.High));
		Assert.That(a.CarryOut.PinType, Is.EqualTo(PinType.Single));
		Assert.That((WireSignal)a.SumOut.Value, Is.EqualTo(WireSignal.Low));
		Assert.That(a.SumOut.PinType, Is.EqualTo(PinType.Single));

		_manager.SetPin(a.A, WireSignal.High);
		_manager.SetPin(a.B, WireSignal.High);
		_manager.SetPin(a.CarryIn, WireSignal.Low);
		Assert.That((WireSignal)a.CarryOut.Value, Is.EqualTo(WireSignal.High));
		Assert.That(a.CarryOut.PinType, Is.EqualTo(PinType.Single));
		Assert.That((WireSignal)a.SumOut.Value, Is.EqualTo(WireSignal.Low));
		Assert.That(a.SumOut.PinType, Is.EqualTo(PinType.Single));

		_manager.SetPin(a.A, WireSignal.High);
		_manager.SetPin(a.B, WireSignal.High);
		_manager.SetPin(a.CarryIn, WireSignal.High);
		Assert.That((WireSignal)a.CarryOut.Value, Is.EqualTo(WireSignal.High));
		Assert.That(a.CarryOut.PinType, Is.EqualTo(PinType.Single));
		Assert.That((WireSignal)a.SumOut.Value, Is.EqualTo(WireSignal.High));
		Assert.That(a.SumOut.PinType, Is.EqualTo(PinType.Single));
	}

	// [Test]
	// [TestCase(2)]
	// [TestCase(4)]
	// [TestCase(6)]
	// [TestCase(8)]
	//
	// public void AdderHookupTests(int width)
	// {
	// 	var adder = new Adder(_manager, width);
	// 	//zero out
	// 	_manager.SetPin(adder.A, new byte[width]);
	// 	_manager.SetPin(adder.B, new byte[width]);
	// 	_manager.SetPin(adder.CarryIn, WireSignal.Low);
	//
	// 	for (int i = 0; i < width; i++)
	// 	{
	// 		Assert.That((WireSignal)adder.Out.Value[i],Is.EqualTo(WireSignal.Low));
	// 	}
	//
	// 	_manager.SetPin(adder.A, new byte[width]);
	// 	_manager.SetPin(adder.B, new byte[width]);
	// 	_manager.SetPin(adder.CarryIn, WireSignal.High);
	// 	//now the value should should be one.
	// 	for (int i = 0; i < width; i++)
	// 	{
	// 		Assert.That((WireSignal)adder.Out.Value[i], Is.EqualTo(i == 0 ? WireSignal.High : WireSignal.Low));
	// 	}
	// }
	//
	// [Test]
	// public void AdderTopoSortTest()
	// {
	// 	var add = new Adder(_manager, 2);
	// 	var s = _manager.GetTopoSort();
	// 	Assert.True(_manager.ValidateTopoSort());
	// 	var a = s.IndexOf(add.A);
	// 	var b = s.IndexOf(add.B);
	// 	var c = s.IndexOf(add.CarryIn);
	// 	var so = s.IndexOf(add.Out);
	// 	var co = s.IndexOf(add.CarryOut);
	//
	// 	Assert.That(a, Is.LessThan(so));
	// 	Assert.That(a, Is.LessThan(co));
	// 	Assert.That(b, Is.LessThan(so));
	// 	Assert.That(b, Is.LessThan(co));
	// 	Assert.That(c, Is.LessThan(so));
	// 	Assert.That(c, Is.LessThan(co));
	// }
	//
	// [Test]
	// public void FullAdderTopoSortTest()
	// {
	// 	var add = new FullAdder(_manager);
	// 	Assert.True(_manager.ValidateTopoSort());
	// 	var s = _manager.GetTopoSort();
	// 	var a = s.IndexOf(add.A);
	// 	var b = s.IndexOf(add.B);
	// 	var c = s.IndexOf(add.CarryIn);
	// 	var so = s.IndexOf(add.SumOut);
	// 	var co = s.IndexOf(add.CarryOut);
	//
	// 	Assert.That(a, Is.LessThan(so));
	// 	Assert.That(a, Is.LessThan(co));
	// 	Assert.That(b, Is.LessThan(so));
	// 	Assert.That(b, Is.LessThan(co));
	// 	Assert.That(c, Is.LessThan(so));
	// 	Assert.That(c, Is.LessThan(co));
	// }
	//
	// [Test]
	// [TestCase(2)]
	// [TestCase(4)]
	// public void AdderSumAllForWidth(int width)
	// {
	// 	int maxCanSum = ((int)Math.Pow(2,width));
	// 	var adder = new Adder(_manager, width);
	// 	//zero out
	// 	_manager.SetPin(adder.CarryIn, WireSignal.Low);
	// 	_manager.SetPin(adder.A, new byte[width]);
	// 	_manager.SetPin(adder.B, new byte[width]);
	//
	// 	for (int x = 0; x < maxCanSum; x++)
	// 	{
	// 		for (int y = 0; y < maxCanSum; y++)
	// 		{
	// 			_manager.SetPin(adder.A, PinUtility.IntToByteArray(x, width));
	// 			_manager.SetPin(adder.B, PinUtility.IntToByteArray(y, width));
	// 			var result = PinUtility.ByteArrayToInt(adder.Out.Value);
	// 			Console.WriteLine($"{result} is {x}+{y}. CO is {adder.CarryOut.Signal}. Max is {maxCanSum}");
	//
	// 			if (x + y >= maxCanSum)
	// 			{
	// 				Assert.That(result, Is.EqualTo((x + y) - (maxCanSum)));
	// 				Assert.That((adder.CarryOut.Signal), Is.EqualTo(WireSignal.High));
	// 			}
	// 			else
	// 			{
	// 				Assert.That(result, Is.EqualTo((x + y)));
	// 				Assert.That((adder.CarryOut.Signal), Is.EqualTo(WireSignal.Low));
	// 			}
	// 		}
	// 	}
	// }
	//
	//
	// [Test]
	// public void AdderTopoSortInternalsTest()
	// {
	// 	var add = new Adder(_manager, 8);
	// 	var s = _manager.GetTopoSort();
	// 	var a = s.IndexOf(add.A);
	// 	var b = s.IndexOf(add.B);
	// 	var c = s.IndexOf(add.CarryIn);
	// 	var so = s.IndexOf(add.Out);
	// 	var co = s.IndexOf(add.CarryOut);
	// 	var a0so = s.IndexOf(add.Adders[0].SumOut);
	// 	var a1so = s.IndexOf(add.Adders[1].SumOut);
	// 	var a0a = s.IndexOf(add.Adders[0].A);
	// 	var a1a = s.IndexOf(add.Adders[1].A);
	// 	var a0b = s.IndexOf(add.Adders[0].B);
	// 	var a1b = s.IndexOf(add.Adders[1].B);
	// 	var a0co = s.IndexOf(add.Adders[0].CarryOut);
	// 	var a1co = s.IndexOf(add.Adders[1].CarryOut);
	//
	// 	Assert.False(a > a0a || a > a1a || a > a0so || a > a1so);
	//
	// 	Assert.False(b > a1b || b > a1so || b > a0b || b > a0so);
	//
	// 	Assert.False(c > a1so || c > a0a || c > a0b || c > a0so || c > a1so);
	//
	// 	Assert.False(so < a || so < b || so < c || so < a0so || so < a1so);
	//
	// 	Assert.False(a1co < a0co);
	//
	// 	Assert.False(co < a || co < b || co < c || co < a1co || co < a0co);
	//
	// }
}