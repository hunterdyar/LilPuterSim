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
	
	public ConsoleOutput Output;
	
	public RAM InstructionMemory => _instructionMemory;
	private RAM _instructionMemory;

	public RAM DataMemory => _dataMemory;
	private RAM _dataMemory;
	
	public Pin InstructionOperand;
	
	//
	public Bus Bus => _bus;
	private Bus _bus;
	
	public ClockPin Clock;
	//Input: Instructions (instructionMem[pc]). inM: Data[pc]. Reset)
	//Output: outM, writeM, addressM <- to data memory pc-> to instruction memory


	public CPU(ComputerBase comp, int width = 8)
	{
		_bus = new Bus(comp, width);

		A = new Register(comp, width);
		B = new Register(comp, width);
		PC = new Counter(comp, width);
		ALU = new ALUMultiBit(comp,width);
		Clock = new ClockPin(comp.Clock);
		Output = new ConsoleOutput(comp);
		
		_instructionMemory = new RAM(comp, "Instruction Memory", width, 256);
		_instructionMemory.Load.Set(WireSignal.Low);
		_dataMemory = new RAM(comp, "Data Memory", width, 1024);
		_dataMemory.Load.Set(WireSignal.Low);
		
		InstructionOperand = new Pin(comp.WireManager, "InsOut Pin");
		
		A.Output.ConnectTo(ALU.A);
		B.Output.ConnectTo(ALU.B);
		ALU.Operation.Set(0);//ADD 
		//ALU Status to Status (Combiner then) register.
		//TODO: We don't have invert in the ALU yet.
		comp.WireManager.SetPin(ALU.InvertA, WireSignal.Low);
		comp.WireManager.SetPin(ALU.InvertB, WireSignal.Low);
		
		PC.CountEnable.SetSilently(WireSignal.Low);
		
		//Register on the bus!
		//a register
		Bus.RegisterComponent("AI", true, false,A.Input, A.Load);
		Bus.RegisterComponent("AO", false, true, A.Output);

		//b register
		Bus.RegisterComponent("BI", true,false, B.Input, B.Load);
		Bus.RegisterComponent("BO", false,true, B.Output);

		//program counter enable and output
		Bus.RegisterComponent("PCE", false, false,null, PC.CountEnable, true);
		Bus.RegisterComponent("CO", false,true, PC.Out);//connect the counter to the bus.
		Bus.RegisterComponent("J", false,true, PC.LoadInput, PC.LoadEnable);
		// comp.Bus.RegisterComponent("J", true, PC.Load, PC.Load);

		Bus.RegisterComponent("MI", true, false, _dataMemory.In, _dataMemory.Load);
		Bus.RegisterComponent("MO", false, true, _dataMemory.Out);

		
		//Logic and Things.
		//We never load the bus data. We only load it's Select state.
		//We need to change this to bus control, breakout select from bus data to control lines. ADD, AND, OR, etc.
		//We need two pins in on the bus for select. todo: BusConnections can have width.
		//comp.Bus.RegisterComponent("ALUS", true, false, ALU.Operation);
		Bus.RegisterComponent("ALUO", false, true, ALU.Result);
		
		//Status Register will get connected directly from the ALU and always be updated. read only, basically
		
		//output enable. (bus IN to the output)
		Bus.RegisterComponent("OI", false, true,Output.OutIn, Output.Enable);

		//Instructions
		Bus.RegisterComponent("IOI", false,true, InstructionOperand);
		
		//reset
		Bus.SetBus(0);
	}
}