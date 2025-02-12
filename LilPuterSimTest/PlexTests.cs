using LilPuter;
using NUnit.Framework;

// ReSharper disable SuggestVarOrType_BuiltInTypes

namespace LilPuterSimTest
{
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

		[Test]
		public void Mux41Test()
		{
			var mux = new Mux4by1(_manager);
		
			//Let's just do every possible input permutation. 
			//THis doesn't fully test our system, if things switch in a weird order, but this is a fully-simmed system; without onChange listeners. that's to be tested elsewhere.
			for (int s = 0; s < 4; s++)
			{
				for (int a = 0; a < 2; a++)
				{
					for (int b = 0; b < 2; b++)
					{
						for (int c = 0; c < 2; c++)
						{
							for (int d = 0; d < 2; d++)
							{
								_manager.SetPin(mux.Select, s);
								_manager.SetPin(mux.A, a);
								_manager.SetPin(mux.B, b);
								_manager.SetPin(mux.C, c);
								_manager.SetPin(mux.D, d);
								switch (s)
								{
									case 0:
										Assert.That(mux.Out.Signal, Is.EqualTo((WireSignal)a));
										break;
									case 1:
										Assert.That(mux.Out.Signal, Is.EqualTo((WireSignal)b));
										break;
									case 2:
										Assert.That(mux.Out.Signal, Is.EqualTo((WireSignal)c));
										break;
									case 3:
										Assert.That(mux.Out.Signal, Is.EqualTo((WireSignal)d));
										break;
								}
							
							}
						}
					}
				}
			}
		} 
	}
}