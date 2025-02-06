using LilPuter;

namespace LilPuterSimTest;

public class PlexTests
{
	private ComputerBase _computerBase;
	private WireManager _manager => _computerBase.WireManager;

	[SetUp]
	public void Setup()
	{
		_computerBase = new ComputerBase();
	}
	
	[Test]
	public void MuxTest()
	{
		var mux = new Mux(_manager);
		_manager.SetPin(mux.Select, WireSignal.Low);
		_manager.SetPin(mux.A, WireSignal.High);
		_manager.SetPin(mux.B, WireSignal.Low);
		Assert.That(mux.Out.Signal, Is.EqualTo(WireSignal.High));
		_manager.SetPin(mux.Select, WireSignal.High);
		Assert.That(mux.Out.Signal, Is.EqualTo(WireSignal.Low));
		_manager.SetPin(mux.A, WireSignal.Low);
		_manager.SetPin(mux.B, WireSignal.High);
		Assert.That(mux.Out.Signal, Is.EqualTo(WireSignal.High));
		_manager.SetPin(mux.Select, WireSignal.Low);
		Assert.That(mux.Out.Signal, Is.EqualTo(WireSignal.Low));

	} 
	
	//todo: multiplexer tests.
}