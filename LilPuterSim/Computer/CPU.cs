using LilPuter.Clock;

namespace LilPuter;
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
	
	public ConsoleOutput Output;
	//Connect this pin to the Instruction ROM Output
	public Pin Instruction => _instruction.Input;
	private readonly Breakout _instruction;
	
	public Pin InstructionOperand;
	
	//
	public Bus Bus;
	public ClockPin Clock;
	//Input: Instructions (instructionMem[pc]). inM: Data[pc]. Reset)
	//Output: outM, writeM, addressM <- to data memory pc-> to instruction memory


	public CPU(ComputerBase comp, int width = 8)
	{
		A = new Register(comp, width);
		B = new Register(comp, width);
		PC = new Counter(comp, width);
		ALU = new ALUMultiBit(comp,width);
		Clock = new ClockPin(comp.Clock);
		Output = new ConsoleOutput(comp);
		InstructionOperand = new Pin(comp.WireManager, "InsOut Pin");

		//Bring in the instruction and break it out to individual bits.
		//todo: not really doing this anymore.
		_instruction = new Breakout(comp, "Instruction", width);
		
		A.Output.ConnectTo(ALU.A);
		B.Output.ConnectTo(ALU.B);
		ALU.Operation.Set(0);//ADD 
		//ALU Status to Status (Combiner then) register.
		//TODO: We don't have invert in the ALU yet.
		comp.WireManager.SetPin(ALU.InvertA, WireSignal.Low);
		comp.WireManager.SetPin(ALU.InvertB, WireSignal.Low);
		
		PC.CountEnable.SetSilently(WireSignal.Low);
		
		//Register on the bus!
		//a register
		comp.Bus.RegisterComponent("AI", true, false,A.Input, A.Load);
		comp.Bus.RegisterComponent("AO", false, true, A.Output);

		//b register
		comp.Bus.RegisterComponent("BI", true,false, B.Input, B.Load);
		comp.Bus.RegisterComponent("BO", false,true, B.Output);

		//program counter enable and output
		comp.Bus.RegisterComponent("PCE", false, false,null, PC.CountEnable, true);
		comp.Bus.RegisterComponent("CO", false,true, PC.Out);//connect the counter to the bus.
		comp.Bus.RegisterComponent("J", false,true, PC.LoadInput, PC.LoadEnable);
		// comp.Bus.RegisterComponent("J", true, PC.Load, PC.Load);

		//Logic and Things.
		//We never load the bus data. We only load it's Select state.
		//We need to change this to bus control, breakout select from bus data to control lines. ADD, AND, OR, etc.
		//We need two pins in on the bus for select. todo: BusConnections can have width.
		//comp.Bus.RegisterComponent("ALUS", true, false, ALU.Operation);
		comp.Bus.RegisterComponent("ALUO", false, true, ALU.Result);
		
		//Status Register will get connected directly from the ALU and always be updated. read only, basically
		
		
		//output enable. (bus IN to the output)
		comp.Bus.RegisterComponent("OI", false, true,Output.OutIn, Output.Enable);

		//Instructions
		comp.Bus.RegisterComponent("IOI", false,true, InstructionOperand);

		Clock.OnTick += OnTick;
		Clock.OnTock += OnTock;
		
		//reset
		comp.Bus.SetBus(0);
	}

	private void OnTick()
	{
		//Todo: Implement microcode?
		//Decode the Instruction. If Instruction is 0, then it's an A instruction, and we load the A register with... something
		if (_instruction.OutPins[0].Signal == WireSignal.High)
		{
			
		}
	}

	private void OnTock()
	{
		
	}
}