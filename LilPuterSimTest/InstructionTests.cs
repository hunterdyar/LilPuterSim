using System.Text;
using LilPuter;
using NUnit.Framework;

namespace LilPuterSimTest
{
	public class  InstructionTests
	{
		private ComputerBase _comp;
		private CPU _cpu => _comp.CPU;
		private Bus _bus => _comp.CPU.Bus;
		private StringBuilder _programOutput = new StringBuilder();
	
		[SetUp]
		public void Setup()
		{
			_programOutput.Clear();
			_comp = new ComputerBase();
			_comp.CPU.Output.OnOutput += o => _programOutput.AppendLine($"{o:D}");
		}


		[Test]
		public void BusAndRegistersTest()
		{
			var loadA = _bus.GetCodeFor("AI", "MO");
			var loadB = _bus.GetCodeFor("BI", "MO");
			var movab =_bus.GetCodeFor("AO", "BI");
			var movba = _bus.GetCodeFor("BO", "AI");

			//set MO to be some non-default register, non-default output.
			_cpu.DataMemory.Registers[3] = 42;
			_cpu.DataMemory.Registers[4] = 100;
			_comp.WireManager.SetPin(_cpu.DataMemory.Address,3);
			_comp.Clock.Cycle();

			_bus.SetBus(loadA);
			Assert.That(_comp.CPU.A.Output.Value, Is.EqualTo(42));

			//manually set memory to address 4.
			_comp.WireManager.SetPin(_cpu.DataMemory.Address, 4);
			_bus.SetBus(loadB);
			Assert.That(_comp.CPU.B.Output.Value, Is.EqualTo(100));
		
		}

		[Test]
		public void AddTwoNumbers()
		{
			var loadA = _bus.GetCodeFor("AI", "MO");
			var loadB = _bus.GetCodeFor("BI", "MO");
			//move the result to a.
			var add = _bus.GetCodeFor("AI", "ALUO");

			//set MO to be some non-default register, non-default output.
			_cpu.DataMemory.Registers[3] = 42;
			_cpu.DataMemory.Registers[4] = 8;
			_comp.WireManager.SetPin(_cpu.DataMemory.Address, 3);
			_comp.Clock.Cycle();

			_bus.SetBus(loadA);
			Assert.That(_comp.CPU.A.Output.Value, Is.EqualTo(42));
			_comp.WireManager.SetPin(_cpu.DataMemory.Address, 4);
		
			_bus.SetBus(loadB);
			Assert.That(_comp.CPU.B.Output.Value, Is.EqualTo(08));
			Assert.That(_comp.CPU.ALU.Result.Value, Is.EqualTo(50));
			_bus.SetBus(add);
			Assert.That(_comp.CPU.A.Output.Value, Is.EqualTo(50));
		}

		[Test]
		public void ProgramTest()
		{
			var sb = new StringBuilder();
			sb.AppendLine("LDAI 22");
			sb.AppendLine("LDBI 20");
			sb.AppendLine("ADD");
			sb.AppendLine("OUT");
			sb.AppendLine("HLT");
			_cpu.LoadProgram(sb.ToString());

			for (var i = 0; i < 100; i++)
			{
				if (_comp.CPU.HaltLine.Signal == WireSignal.Low)
				{
					_comp.Clock.Cycle();
				}
				else
				{
					break;
				}
			}
		
			Assert.That(_programOutput.ToString().Trim(),Is.EqualTo("42"));
		}

		[Test]
		public void StoreValueInstructionTest()
		{
			var sb = new StringBuilder();
			sb.AppendLine("LDAI 3");
			sb.AppendLine("STA 15");
			sb.AppendLine("LDBI 42");
			sb.AppendLine("STB 12");
			sb.AppendLine("HLT");
			_cpu.LoadProgram(sb.ToString());

			for (var i = 0; i < 100; i++)
			{
				if (_comp.CPU.HaltLine.Signal == WireSignal.Low)
				{
					_comp.Clock.Cycle();
				}
				else
				{
					break;
				}
			}

			Assert.That(_cpu.DataMemory.Registers[15], Is.EqualTo(3));
			Assert.That(_cpu.DataMemory.Registers[12], Is.EqualTo(42));

		}
	}
}