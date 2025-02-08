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
	
	//Connect this pin to the Instruction ROM Output
	public Pin Instruction => _instruction.Input;
	private readonly Breakout _instruction;

	public Bus Bus;
	public ClockPin Clock;
	//Input: Instructions (instructionMem[pc]). inM: Data[pc]. Reset)
	//Output: outM, writeM, addressM <- to data memory pc-> to instruction memory

	private int _aRegInBit;
	private int _aRegOutBit;
	private int _bRegInBit;
	private int _bRegOutBit;
	private int _pcInBit;
	private int _pcOutBit;
	public CPU(ComputerBase comp, int width = 8)
	{
		A = new Register(comp, width);
		B = new Register(comp, width);
		PC = new Counter(comp, width);
		ALU = new ALUMultiBit(comp,width);
		Clock = new ClockPin(comp.Clock);
		Bus = new Bus(comp, width);
		
		//Bring in the instruction and break it out to individual bits.
		_instruction = new Breakout(comp, "Instruction", width);
		Instruction.ConnectTo(_instruction.Input);
		
		A.Output.ConnectTo(ALU.A);
		B.Output.ConnectTo(ALU.B);
		
		//Register on the bus!
		(_aRegInBit, _aRegOutBit) = Bus.RegisterComponent("A", A.Input,A.Output, A.Load);
		(_bRegInBit, _bRegOutBit) = Bus.RegisterComponent("B", B.Input, B.Output, B.Load);
		(_pcInBit, _pcOutBit) = Bus.RegisterComponent("PC", PC.Input, PC.Out, PC.CountEnable, true);
		
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