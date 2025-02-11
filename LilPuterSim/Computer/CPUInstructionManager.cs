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
	private Bus _bus;
	private ClockPin _clock;
	public readonly Dictionary<string,int> OpCodes = new Dictionary<string, int>();
	public CPUInstructionManager(ComputerBase computerBase, Bus bus)
	{
		_bus = bus;
		_computer = computerBase;
		_counter = new Counter(computerBase, computerBase.Width, 5);//5 microinstructions per instruction.
		_counter.CountEnable.Set(WireSignal.High);
		_counter.Reset.Set(WireSignal.Low);
		_counter.LoadEnable.Set(WireSignal.Low);
		_microcode = new RAM(computerBase, "Microcode Lookup Table", computerBase.Width, 1024);
		_clock = new ClockPin(computerBase.Clock);
		_clock.OnTick += OnTick;
		_clock.OnTock += OnTock;
	}

	private void OnTock()
	{
		//Set the bus on Tock, right before whatever-is-set does it's thing on tick.
		
		//we get the microcode by combining the current external instruction with our clock.
		var insadr = GetMicrocodeAddress();
		Console.WriteLine($"{_computer.CPU.InstructionMemory.Address.Value}: {_computer.CPU.InstructionMemory.Instruction.Value}-{_counter.Out.Value} and Microcode {insadr}");
		_computer.WireManager.SetPin(_microcode.Address, insadr);//Set and Impulse to get the value we want. This is inefficient.
		//after impulse, we're updated.
		var controlCode = _microcode.Out.Value;
		_bus.SetBus(controlCode);
	}

	private void OnTick()
	{
		
	}

	private int GetMicrocodeAddress()
	{
		 return MakeMicrocodeAddress(_computer.CPU.InstructionMemory.Instruction.Value,_counter.Out.Value);
		return 0;
	}

	private int MakeMicrocodeAddress(int instruction, int count)
	{
		return (instruction << 4) | (count & 0b00001111);
	}
	
	public void CreateMicrocode()
	{
		int nop = 0b0000;
		OpCodes.Add("NOP", nop);
		CreateInstructionMicrocode(nop,[]);
		
		//LOAD A
		int lda = 1;
		OpCodes.Add("LDA", lda);
		CreateInstructionMicrocode(lda, ["IOO", "AI"]);
		//LOAD B
		int ldb = 2;
		CreateInstructionMicrocode(ldb, ["IOO", "BI"]);
		
		//OUTPUT A Register
		int aOut = 3;
		OpCodes.Add("OUT", aOut);
		CreateInstructionMicrocode(aOut, ["AO", "OI"]);
		
		//Take the sum of current A and B and put it into A.
		int addAB = 4;
		OpCodes.Add("ADD", addAB);
		CreateInstructionMicrocode(addAB, ["AI", "ALUO"]);

	}

	private void CreateInstructionMicrocode(int instruction, params string[] cCodeSets)
	{
		int fetchA = _bus.GetCodeFor("CO", "II");
		//1. CounterOut, InstructionIn (Put the counter on the bus and put that into InstructionMemory (in: address selector)
		//MemoryOut, InstructionRegisterIn, program counter out (Take this instruction from InstrtuctionMemory and put it in the instructin register. Hey, we care about that!)
		int fetchB = _bus.GetCodeFor("IMO", "PCE");
		
		_microcode.Registers[MakeMicrocodeAddress(instruction, 0)] = fetchA;
		_microcode.Registers[MakeMicrocodeAddress(instruction, 1)] = fetchB;

		for (int i = 0; i < cCodeSets.Length; i++)
		{
			_microcode.Registers[MakeMicrocodeAddress(instruction, 2+i)] = _bus.GetCodeFor(cCodeSets[i]);
		}
	}
}