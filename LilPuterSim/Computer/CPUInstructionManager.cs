using LilPuter.Clock;

namespace LilPuter;

/// <summary>
/// Gets set to an address, then as the clock counts, it increments through a set of instructions that have been loaded into its memory.
/// This manages the microcode and setting the bus from the given instruction.
/// </summary>
public class CPUInstructionManager
{
	public RAM Microcode => _microcode;
	private RAM _microcode;
	private Counter _counter;
	public Pin Instruction => _computer.InstructionMemory.Out;
	private ComputerBase _computer;
	private Bus Bus => _computer.Bus;
	private ClockPin _clock;
	public CPUInstructionManager(ComputerBase computerBase)
	{
		_computer = computerBase;
		_microcode = new RAM(computerBase, computerBase.Width, 64);
		_counter = new Counter(computerBase, computerBase.Width);
		_clock = new ClockPin(computerBase.Clock);
		_clock.OnTick += OnTick;
		_clock.OnTock += OnTock;
	}

	private void OnTock()
	{
		//we get the microcode by combining the current external instruction with our clock.
		
	}

	private void OnTick()
	{
		
	}

	public void CreateMicrocode()
	{
		//NOP is 0000
		_microcode.Registers[0] = 0; //get code for PC Enable;
		//Fetch
		int fetchA = _computer.Bus.GetCodeFor("CO", "IMI");
		//1. CounterOut, MemoryIn (Put the counter on the bus and put that into InstructionMemory (in: address selector)
		//MemoryOut, InstructionRegisterIn, program counter out (Take this instruction from InstrtuctionMemory and put it in the instructin register. Hey, we care about that!)
		int fetchB = _computer.Bus.GetCodeFor("II", "IMO", "PCE");
		
		//execute
		//Read in the instruction registrer, and increment OUR counter, which is tied to clock. It is the clock, really.
		//Set the bus with this and the next x instructions.
		//the Next fetch happens!
	}
}