using LilPuter.Clock;

namespace LilPuter
{
	/// <summary>
	/// An 8-bit von neuman ish CPU.
	/// </summary>
	public class CPU
	{
		/// <summary>
		/// Address Register
		/// </summary>
		public Register A;
		/// <summary>
		/// B Register
		/// </summary>
		public Register B;
		/// <summary>
		/// Program Counter
		/// </summary>
		public Counter PC;
		/// <summary>
		/// Arithmetic Logic Unit
		/// </summary>
		public ALUMultiBit ALU;
		public StatusRegister StatusRegister;
		public ConsoleOutput Output;
	
		public InstructionMemory InstructionMemory => _instructionMemory;
		private InstructionMemory _instructionMemory;

		public RAM DataMemory => _dataMemory;
		private RAM _dataMemory;

		public Pin HaltLine;
		public Bus Bus => _bus;
		private Bus _bus;
	
		public ClockPin Clock;
		//Input: Instructions (instructionMem[pc]). inM: Data[pc]. Reset)
		//Output: outM, writeM, addressM <- to data memory pc-> to instruction memory
		public CPUInstructionManager MicrocodeDecoder => _microcodeDecoder;
		private CPUInstructionManager _microcodeDecoder;

		public CPU(ComputerBase comp, int width = 8)
		{
			HaltLine = new Pin(comp.WireManager, "Halt");
			_bus = new Bus(comp, width);

			A = new Register(comp, width);
			B = new Register(comp, width);
			PC = new Counter(comp, "PC",width);
			ALU = new ALUMultiBit(comp,width);
			StatusRegister = new StatusRegister(comp,ALU);
			Clock = new ClockPin(comp.Clock);
			Output = new ConsoleOutput(comp);
		
			_instructionMemory = new InstructionMemory(comp, width, width);
			_dataMemory = new RAM(comp, "Data Memory", width, 1024);
			_dataMemory.Load.Set(WireSignal.Low);
			_microcodeDecoder = new CPUInstructionManager(comp,_bus);
		
			A.Output.ConnectTo(ALU.A);
			B.Output.ConnectTo(ALU.B);
			ALU.Operation.Set(0);//ADD 
			//ALU Status to Status (Combiner then) register.
			//TODO: We don't have invert in the ALU yet.
			comp.WireManager.SetPin(ALU.InvertA, WireSignal.Low);
			comp.WireManager.SetPin(ALU.InvertB, WireSignal.Low);
		
			//Tie CE high. 
			PC.CountEnable.SetSilently(WireSignal.High);
		
			//Manage Halting
			HaltLine.SetSilently(WireSignal.Low);
			comp.WireManager.RegisterSystemAction(HaltLine,(h => comp.Clock.Halt()));
			//Register on the bus!
			//a register
			Bus.RegisterComponent("AI", true, false,A.Input, A.Load);
			Bus.RegisterComponent("AO", false, true, A.Output);

			//b register
			Bus.RegisterComponent("BI", true,false, B.Input, B.Load);
			Bus.RegisterComponent("BO", false,true, B.Output);

			//program counter enable and output
			Bus.RegisterComponent("PCE", false, false, null,PC.Count);
			Bus.RegisterComponent("CO", false,true, PC.Out);//connect the counter to the bus.
			Bus.RegisterComponent("J", false,true, PC.LoadInput, PC.LoadEnable);
			// comp.Bus.RegisterComponent("J", true, PC.Load, PC.Load);

			Bus.RegisterComponent("MAO", false, true, _dataMemory.Address);
			Bus.RegisterComponent("MAI", true, false, _dataMemory.Address);
			Bus.RegisterComponent("MI", true, false, _dataMemory.In, _dataMemory.Load);
			Bus.RegisterComponent("MO", false, true, _dataMemory.Out);

			Bus.RegisterComponent("HLT", false, false, null, HaltLine);
		
			//Logic and Things.
			//We never load the bus data. We only load it's Select state.
			//We need to change this to bus control, breakout select from bus data to control lines. ADD, AND, OR, etc.
			//We need two pins in on the bus for select. todo: BusConnections can have width.
			//comp.Bus.RegisterComponent("ALUS", true, false, ALU.Operation);
			Bus.RegisterComponent("ALUO", false, true, ALU.Result);
		
			//Status Register will get connected directly from the ALU and always be updated. read only, basically
		
			//output enable. (bus IN to the output)
			Bus.RegisterComponent("OI", true, false,Output.OutIn, Output.Enable);

			//Instructions
			Bus.RegisterComponent("IOO", false,true, _instructionMemory.Operand);
			Bus.RegisterComponent("IMO", false, true, _instructionMemory.Instruction);
			Bus.RegisterComponent("II", true, false, _instructionMemory.Address);

			//reset. todo: This is kind of breaking things because the cpu. variables aren't set.
			Bus.SetBus(0);

			_microcodeDecoder.CreateMicrocode();
		}
	
		public void LoadProgram(string program)
		{
			_instructionMemory.Clear();
			var ops = program.Split('\n');
			for (int i = 0; i < ops.Length; i++)
			{
				string d = ops[i].Trim();
				if(d.Length == 0){continue;}
				var m = d.Split(' ');
				int instruction = MicrocodeDecoder.OpCodes[m[0].ToUpper()];

				int data = 0;
				if (m.Length > 1)
				{
					data = int.Parse(m[1]);
				}
				_instructionMemory.Program[i] = (instruction << 8) | data;
			}

			_instructionMemory.ForceUpdateOutputs();
		}
	}
}