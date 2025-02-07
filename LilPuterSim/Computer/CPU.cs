using LilPuter.Clock;

namespace LilPuter;
/// <summary>
/// An 8-bit von neuman CPU.
/// </summary>
public class CPU
{
	/// <summary>
	/// Address Register
	/// </summary>
	public Register A;
	/// <summary>
	/// Data Register
	/// </summary>
	public Register D;
	/// <summary>
	/// Program Counter
	/// </summary>
	public Counter PC;
	/// <summary>
	/// Arithmetic Logic Unit
	/// </summary>
	public ALUMultiBit ALU;
	public Pin Instruction => _instruction.Input;
	private readonly Breakout _instruction;
	
	public Pin InM;
	public Pin Reset;
	public ClockPin Clock;
	//Input: Instructions (instructionMem[pc]). inM: Data[pc]. Reset)
	//Output: outM, writeM, addressM <- to data memory pc-> to instruction memory
	
	public CPU(ComputerBase comp)
	{
		A = new Register(comp, 8);
		D = new Register(comp, 8);
		PC = new Counter(comp, 8);
		ALU = new ALUMultiBit(comp,8);
		Clock = new ClockPin(comp.Clock);
		
		//Bring in the instruction and break it out to individual bits.
		_instruction = new Breakout(comp, "Instruction", 8);
		Instruction.ConnectTo(_instruction.Input);
		
		//Fetch
		//Decode the Instruction
		//
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