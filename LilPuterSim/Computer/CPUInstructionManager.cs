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
	
	private ComputerBase _computer;
	private CPU CPU => _computer.CPU;
	private Bus Bus => _computer.CPU.Bus;
	private ClockPin _clock;
	public CPUInstructionManager(ComputerBase computerBase)
	{
		_computer = computerBase;
		_microcode = new RAM(computerBase, "Microcode Lookup Table",computerBase.Width, 1024);
		_counter = new Counter(computerBase, computerBase.Width, 5);//5 microinstructions per instruction.
		_clock = new ClockPin(computerBase.Clock);
		_clock.OnTick += OnTick;
		_clock.OnTock += OnTock;
	}

	private void OnTock()
	{
		//Set the bus on Tock, right before whatever-is-set does it's thing on tick.
		
		//we get the microcode by combining the current external instruction with our clock.
		var insadr = GetMicrocodeAddress();
		
		_computer.WireManager.SetPin(_microcode.Address, insadr);//Set and Impulse to get the value we want. This is inefficient.
		//after impulse, we're updated.
		var controlCode = _microcode.Out.Value;
		Bus.SetBus(controlCode);
	}

	private void OnTick()
	{
		
	}

	private int GetMicrocodeAddress()
	{
		// return MakeMicrocodeAddress(CPU.Instruction.Value,_counter.Out.Value);
		return 0;
	}

	private int MakeMicrocodeAddress(int instruction, int count)
	{
		return (instruction << 4) | (count & 0b00001111);
	}
	public void CreateMicrocode()
	{
		//Fetch
		int fetchA = Bus.GetCodeFor("CO", "IMI");
		//1. CounterOut, MemoryIn (Put the counter on the bus and put that into InstructionMemory (in: address selector)
		//MemoryOut, InstructionRegisterIn, program counter out (Take this instruction from InstrtuctionMemory and put it in the instructin register. Hey, we care about that!)
		int fetchB = Bus.GetCodeFor("IMO", "PCE");

		int nop = 0b0000;
		CreateInstructionMicrocode(nop,[]);
		
		//LOAD A
		int lda = 1;
		CreateInstructionMicrocode(lda, ["IOO", "AI"]);
		//LOAD B
		int ldb = 2;
		CreateInstructionMicrocode(ldb, ["IOO", "BI"]);
		
		//OUTPUT A Register
		int aOut = 3;
		CreateInstructionMicrocode(aOut, ["AO", "OI"]);
		
		//Take the sum of A and B and put into A.
		int addAB = 4;
		CreateInstructionMicrocode(addAB, ["AI", "ALUO"]);

	}

	private void CreateInstructionMicrocode(int instruction, params string[] cCodeSets)
	{
		int fetchA = Bus.GetCodeFor("CO", "IMI");
		//1. CounterOut, MemoryIn (Put the counter on the bus and put that into InstructionMemory (in: address selector)
		//MemoryOut, InstructionRegisterIn, program counter out (Take this instruction from InstrtuctionMemory and put it in the instructin register. Hey, we care about that!)
		int fetchB = Bus.GetCodeFor("IMO", "PCE");
		
		_microcode.Registers[MakeMicrocodeAddress(instruction, 0)] = fetchA;
		_microcode.Registers[MakeMicrocodeAddress(instruction, 1)] = fetchB;

		for (int i = 0; i < cCodeSets.Length; i++)
		{
			_microcode.Registers[MakeMicrocodeAddress(instruction, 2+i)] = Bus.GetCodeFor(cCodeSets[i]);
		}
	}
}