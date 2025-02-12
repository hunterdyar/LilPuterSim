using System.Text;
using LilPuter;
using NUnit.Framework.Internal;

namespace LilPuterSimTest
{
	public class WireManagerTests
	{
		private ComputerBase _computerBase;
		private WireManager _manager => _computerBase.WireManager;

		[SetUp]
		public void Setup()
		{
			_computerBase = new ComputerBase();
		}

		[Test]
		public void ConnectingPins()
		{
			var a = new Pin(_manager, "A");
			var b = new Pin(_manager, "B");
			var c = new Pin(_manager, "C");

			b.SetSilently(WireSignal.Low);
			a.SetAndImpulse(WireSignal.High);
			Assert.That(b.Value, Is.Not.EqualTo(a.Value));
			a.ConnectTo(b);
			a.ConnectTo(c);
			a.SetAndImpulse(WireSignal.Low);
			Assert.That(b.Value, Is.EqualTo(a.Value));
			Assert.That(c.Value, Is.EqualTo(a.Value));
		}

		[Test]
		public void ListenTest()
		{
			StringBuilder _log = new StringBuilder();
			var a = new Pin(_manager, "A");
			var b = new Pin(_manager, "B");
			var c = new Pin(_manager, "C");
			b.SetSilently(WireSignal.Low);
			a.ConnectTo(b);
			_manager.RegisterSystemAction(a, (p) => { _log.Append($"{p.Name}"); });
			_manager.RegisterSystemAction(b, (p) => { _log.Append($"{p.Name}"); });
			_manager.RegisterSystemAction(c, (p) => { _log.Append($"{p.Name}"); });
			a.SetAndImpulse(WireSignal.High);
			Assert.That(_log.ToString(), Is.EqualTo("AB"));
			_log.Clear();
			b.ConnectTo(c);
			a.SetAndImpulse(WireSignal.Low);
			Assert.That(_log.ToString(), Is.EqualTo("ABC"));

		}

		[Test]
		public void TopoSortTest()
		{
			WireManager m = new WireManager(_computerBase);
			var a = new Pin(m, "A");
			var b = new Pin(m, "B");
			var c = new Pin(m, "C");
			var d = new Pin(m, "D");
			var e = new Pin(m, "E");
			var f = new Pin(m, "F");
			var g = new Pin(m, "G");

			a.ConnectTo(b);
			a.ConnectTo(c);
			b.ConnectTo(c);
			b.ConnectTo(d);
			c.ConnectTo(e);
			d.ConnectTo(e);
			d.ConnectTo(f);
			g.ConnectTo(e);
			g.ConnectTo(f); 
		
			//There are multiple correct answers.
			var sorted = m.GetTopoSort();
			Assert.That(new[] { a, g, b, c, d, e, f }, Is.EqualTo(sorted.ToArray()));
		}

		[Test]
		public void TopoSortTest2()
		{
			WireManager m = new WireManager(_computerBase);
			var a = new Pin(m, "A");
			var b = new Pin(m, "B");
			var c = new Pin(m, "C");
			var d = new Pin(m, "D");
			var e = new Pin(m, "E");
			var f = new Pin(m, "F");
			var g = new Pin(m, "G");
		
			f.ConnectTo(g);
			e.ConnectTo(f);
			m.ConnectPins(d, e);
			c.ConnectTo(d);
			b.ConnectTo(c);
			m.ConnectPins(a,b);

			//There are multiple correct answers.
			var sorted = m.GetTopoSort();
			Assert.That(new[] { a, b, c, d, e, f, g }, Is.EqualTo(sorted.ToArray()));
		}

		//[Test]
		public void CycleDetectionTest()
		{
			Assert.Fail("Cycle Detection not yet implemented");
		}
	}
}