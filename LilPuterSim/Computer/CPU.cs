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
	
	public Pin InstructionOperandOut;
	
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
		InstructionOperandOut = new Pin(comp.WireManager, "InsOut Pin");

		//Bring in the instruction and break it out to individual bits.
		//todo: not really doing this anymore.
		_instruction = new Breakout(comp, "Instruction", width);
		Instruction.ConnectTo(_instruction.Input);
		
		A.Output.ConnectTo(ALU.A);
		B.Output.ConnectTo(ALU.B);
		
		PC.CountEnable.SetSilently(WireSignal.Low);
		
		//Register on the bus!
		//a register
		comp.Bus.RegisterComponent("AI", true, A.Input, A.Load);
		comp.Bus.RegisterComponent("AO", false, A.Output);

		//b register
		comp.Bus.RegisterComponent("BI", true, B.Input, B.Load);
		comp.Bus.RegisterComponent("BO", false, B.Output);

		//program counter enable and output
		comp.Bus.RegisterComponent("PCE", true, PC.Input, PC.CountEnable, true);
		comp.Bus.RegisterComponent("PCO", false, PC.Out);

		//output enable. (bus IN to the output)
		comp.Bus.RegisterComponent("OI", false, Output.OutIn, Output.Enable);

		//Instructions
		comp.Bus.RegisterComponent("IOO", false, InstructionOperandOut);

		Clock.OnTick += OnTick;
		Clock.OnTock += OnTock;
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