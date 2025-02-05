﻿using LilPuter;

namespace LilPuterSimTest;

public class PlexTests
{
	private WireManager _manager;
	[SetUp]
	public void Setup()
	{
		_manager = new WireManager();
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
}