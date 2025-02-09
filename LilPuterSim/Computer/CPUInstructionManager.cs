﻿using LilPuter.Clock;

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
		return MakeMicrocodeAddress(Instruction.Value,_counter.Out.Value);
	}

	private int MakeMicrocodeAddress(int instruction, int count)
	{
		return (instruction << 8) | count;
	}
	public void CreateMicrocode()
	{
		//Fetch
		int fetchA = _computer.Bus.GetCodeFor("CO", "IMI");
		//1. CounterOut, MemoryIn (Put the counter on the bus and put that into InstructionMemory (in: address selector)
		//MemoryOut, InstructionRegisterIn, program counter out (Take this instruction from InstrtuctionMemory and put it in the instructin register. Hey, we care about that!)
		int fetchB = _computer.Bus.GetCodeFor("II", "IMO", "PCE");

		int nop = 0b0000;
		_microcode.Registers[MakeMicrocodeAddress(nop, 0)] = fetchA; //get code for PC Enable;
		_microcode.Registers[MakeMicrocodeAddress(nop, 1)] = fetchB;
		
		//LOAD A
		int lda = 1;
		_microcode.Registers[MakeMicrocodeAddress(lda, 0)] = fetchA;
		_microcode.Registers[MakeMicrocodeAddress(lda, 1)] = fetchB;
		//Instruction-Operand out, A-in
		_microcode.Registers[MakeMicrocodeAddress(lda, 2)] = _computer.Bus.GetCodeFor("IOO", "AI");
		
		//LOAD B
		int ldb = 2;
		_microcode.Registers[MakeMicrocodeAddress(ldb, 0)] = fetchA;
		_microcode.Registers[MakeMicrocodeAddress(ldb, 1)] = fetchB;
		_microcode.Registers[MakeMicrocodeAddress(ldb, 2)] = _computer.Bus.GetCodeFor("IOO", "BI");
		
		//OUTPUT A Register
		int aOut = 3;
		_microcode.Registers[MakeMicrocodeAddress(aOut, 0)] = fetchA;
		_microcode.Registers[MakeMicrocodeAddress(aOut, 1)] = fetchB;
		_microcode.Registers[MakeMicrocodeAddress(aOut, 2)] = _computer.Bus.GetCodeFor("AO","OI");

		//execute
		//Read in the instruction registrer, and increment OUR counter, which is tied to clock. It is the clock, really.
		//Set the bus with this and the next x instructions.
		//the Next fetch happens!
	}
}