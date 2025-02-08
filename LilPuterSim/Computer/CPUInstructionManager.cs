namespace LilPuter;

/// <summary>
/// Gets set to an address, then as the clock counts, it increments through a set of instructions that have been loaded into its memory.
/// </summary>
public class CPUInstructionManager
{
	public RAM RAM => _ram;
	private RAM _ram;
	private Counter _counter;
	
	public CPUInstructionManager(ComputerBase computerBase)
	{
		_ram = new RAM(computerBase, computerBase.Width, 64);
		_counter = new Counter(computerBase, computerBase.Width);
	}

	public void CreateMicrocode()
	{
		//NOP is 0000
		_ram.Registers[0] = 0; //get code for PC Enable;
		
		//Fetch
		//1. CounterOut, MemoryIn (Put the counter on the bus and put that into InstructionMemory (in: address selector)
		//MemoryOut, InstructionRegisterIn, CounterEnable (Take this instruction and put it in the instructin register. Hey, that's ... this!)
		
		//execute
			//Read in the instruction registrer, and increment OUR counter, which is tied to clock. It is the clock, really.
			//Set the bus with this and the next x instructions.
			//Re-enable the counter.
			//the Next fetch happens!
	}
}