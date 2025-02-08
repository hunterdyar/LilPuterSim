namespace LilPuter;

/// <summary>
/// Gets set to an address, then as the clock counts, it increments through a set of instructions that have been loaded into its memory.
/// </summary>
public class CPUInstructionManager
{
	public RAM RAM => _ram;
	private RAM _ram;
	private Counter _counter;
	
	private ComputerBase _computer;
	private Bus Bus => _computer.Bus;
	public CPUInstructionManager(ComputerBase computerBase)
	{
		_computer = computerBase;
		_ram = new RAM(computerBase, computerBase.Width, 64);
		_counter = new Counter(computerBase, computerBase.Width);
	}

	public void CreateMicrocode()
	{
		//NOP is 0000
		_ram.Registers[0] = 0; //get code for PC Enable;
		//Fetch
		int fetchA = _computer.Bus.GetCodeFor("CO", "IMI");
		//1. CounterOut, MemoryIn (Put the counter on the bus and put that into InstructionMemory (in: address selector)
		//MemoryOut, InstructionRegisterIn, program counter out (Take this instruction from InstrtuctionMemory and put it in the instructin register. Hey, we care about that!)
		int fetchB = _computer.Bus.GetCodeFor("II", "IMO");
		
		
		//execute
		//Read in the instruction registrer, and increment OUR counter, which is tied to clock. It is the clock, really.
		//Set the bus with this and the next x instructions.
		//Re-enable the counter.
		//the Next fetch happens!
	}
}