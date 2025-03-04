﻿using System.Text;
using LilPuter;
using NUnit.Framework;

namespace LilPuterSimTest
{
	public class LogicSim
	{
		private ComputerBase _computerBase;
		private WireManager _manager => _computerBase.WireManager;
	
		Pin A;
		Pin B;
		Pin C;
		Pin D;
		Pin E;
		Pin F;
		Pin G;
		private StringBuilder _log;
	
		[SetUp]
		public void Setup()
		{
			_computerBase = new ComputerBase();
			_log = new StringBuilder();
		
			A = new Pin(_manager, "A");
			B = new Pin(_manager, "B");
			C = new Pin(_manager, "C");
			D = new Pin(_manager, "D");
			E = new Pin(_manager, "E");
			F = new Pin(_manager, "F");
			G = new Pin(_manager, "G");
		
			A.Subscribe((p)=>_log.Append("A"));
			B.Subscribe((p) => _log.Append("B"));
			C.Subscribe((p) => _log.Append("C"));
			D.Subscribe((p) => _log.Append("D"));
			E.Subscribe((p) => _log.Append("E"));
			F.Subscribe((p) => _log.Append("F"));
			G.Subscribe((p) => _log.Append("G"));
		}

		[Test]
		public void LinearTraversal()
		{
			A.ConnectTo(B);
			B.ConnectTo(C);
			C.ConnectTo(D);
			D.ConnectTo(E);
			E.ConnectTo(F);
			F.ConnectTo(G);
			_manager.SetPin(A,WireSignal.High);
		
			Assert.That(_log.ToString(), Is.EqualTo("ABCDEFG"));
		}
	
	
	}
}