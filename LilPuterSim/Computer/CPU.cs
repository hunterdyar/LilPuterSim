﻿using LilPuter.Clock;

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

	public readonly int ARegInBit;
	public readonly int ARegOutBit;
	public readonly int BRegInBit;
	public readonly int BRegOutBit;
	public readonly int PCInBit;
	public readonly int PCOutBit;
	public CPU(ComputerBase comp, int width = 8)
	{
		A = new Register(comp, width);
		B = new Register(comp, width);
		PC = new Counter(comp, width);
		ALU = new ALUMultiBit(comp,width);
		Clock = new ClockPin(comp.Clock);
		
		//Bring in the instruction and break it out to individual bits.
		_instruction = new Breakout(comp, "Instruction", width);
		Instruction.ConnectTo(_instruction.Input);
		
		A.Output.ConnectTo(ALU.A);
		B.Output.ConnectTo(ALU.B);
		
		PC.CountEnable.SetSilently(WireSignal.Low);
		
		//Register on the bus!
		ARegInBit = comp.Bus.RegisterComponent("AI", true, A.Input, A.Load);
		ARegInBit = comp.Bus.RegisterComponent("AO", false, A.Output);

		BRegInBit = comp.Bus.RegisterComponent("BI", true, B.Input, B.Load);
		BRegInBit = comp.Bus.RegisterComponent("BO", false, B.Output);

		PCInBit = comp.Bus.RegisterComponent("PCI", true, PC.Input, PC.CountEnable, true);
		PCOutBit = comp.Bus.RegisterComponent("PCO", false, PC.Out);
		
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