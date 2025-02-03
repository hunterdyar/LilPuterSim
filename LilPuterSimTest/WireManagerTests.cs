using System.Text;
using LilPuter;
using NUnit.Framework.Internal;

namespace LilPuterSimTest;

public class WireManagerTests
{
	private WireManager _wireManager;

	[SetUp]
	public void Setup()
	{
		_wireManager = new WireManager();
	}

	[Test]
	public void ConnectingPins()
	{
		var a = new Pin(_wireManager, "A");
		var b = new Pin(_wireManager, "B");
		var c = new Pin(_wireManager, "C");

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
		var a = new Pin(_wireManager, "A");
		var b = new Pin(_wireManager, "B");
		var c = new Pin(_wireManager, "C");
		b.SetSilently(WireSignal.Low);
		a.ConnectTo(b);
		_wireManager.RegisterSystemAction(a, (p) => { _log.Append($"{p.Name}"); });
		_wireManager.RegisterSystemAction(b, (p) => { _log.Append($"{p.Name}"); });
		_wireManager.RegisterSystemAction(c, (p) => { _log.Append($"{p.Name}"); });
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
		
		var a = new Pin(_wireManager, "A");
		var b = new Pin(_wireManager, "B");
		var c = new Pin(_wireManager, "C");
		var d = new Pin(_wireManager, "D");
		var e = new Pin(_wireManager, "E");
		var f = new Pin(_wireManager, "F");
		var g = new Pin(_wireManager, "G");

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
		
		var sorted = _wireManager.GetTopoSort();
		Assert.That(new[] { a, g, b, c, d, e, f }, Is.EqualTo(sorted.ToArray()));
	}
}