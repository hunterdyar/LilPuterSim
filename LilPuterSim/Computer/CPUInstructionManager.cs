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
	private readonly Dictionary<int,string> OpCodeReverseLookup = new Dictionary<int,string>();
	private string InstructionName => OpCodeReverseLookup[_computer.CPU.InstructionMemory.Instruction.Value];
	public CPUInstructionManager(ComputerBase computerBase, Bus bus)
	{
		_bus = bus;
		_computer = computerBase;
		_counter = new Counter(computerBase, "Microcode Counter",computerBase.Width, 5);//5 microinstructions per instruction.
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
		_counter.Count.SetAndImpulse(WireSignal.High);
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
		_counter.Count.SetAndImpulse(WireSignal.Low);
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
		CreateInstructionMicrocode("NOP",[]);
		
		CreateInstructionMicrocode("LDA", ["IOO", "AI"]);
		
		CreateInstructionMicrocode("LDB", ["IOO", "BI"]);
		
		CreateInstructionMicrocode("OUT", ["AO", "OI"]);
		
		CreateInstructionMicrocode("ADD", ["AI", "ALUO"]);
		
		CreateInstructionMicrocode("HLT", ["HLT"]);
	}

	private int CreateInstructionMicrocode(string instructionName, params string[][] cCodeSets)
	{
		int instructionCode = 0;
		
		if(OpCodes.Any())
		{
			instructionCode = OpCodes.Values.Max() + 1;
		};
		OpCodes.Add(instructionName, instructionCode);
		OpCodeReverseLookup.Add(instructionCode,instructionName);
		
		int fetchA = _bus.GetCodeFor("CO", "II");
		//1. CounterOut, InstructionIn (Put the counter on the bus and put that into InstructionMemory (in: address selector)
		//MemoryOut, InstructionRegisterIn, program counter out (Take this instruction from InstrtuctionMemory and put it in the instructin register. Hey, we care about that!)
		int fetchB = _bus.GetCodeFor("IMO", "PCE");
		
		_microcode.Registers[MakeMicrocodeAddress(instructionCode, 0)] = fetchA;
		_microcode.Registers[MakeMicrocodeAddress(instructionCode, 1)] = fetchB;

		for (int i = 0; i < cCodeSets.Length; i++)
		{
			_microcode.Registers[MakeMicrocodeAddress(instructionCode, 2+i)] = _bus.GetCodeFor(cCodeSets[i]);
		}

		return instructionCode;
	}
}