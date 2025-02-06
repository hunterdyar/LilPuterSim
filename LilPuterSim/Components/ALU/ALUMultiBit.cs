﻿namespace LilPuter;

public class ALUMultiBit
{
	public ALUOneBit[] ALUOneBits;
	public Pin Operation;
	public Pin A;
	public Pin B;
	private Pin CarryIn;

	public Pin InvertA;
	public Pin InvertB;
	
	//Outputs
	public Pin CarryOut;//overflow
	public Pin Result;
	public Pin IsZero;

	private int _width;
	private WireManager _manager;
	public ALUMultiBit(ComputerBase manager, int width)
	{
		_manager = manager.WireManager;
		_width = width;
		ALUOneBits = new ALUOneBit[width];
		A = new Pin(manager.WireManager, "ALU A",width);
		B = new Pin(manager.WireManager, "ALU B",width);
		CarryIn = new Pin(manager.WireManager,"ALU CarryIn");
		CarryOut = new Pin(manager.WireManager,"ALU CarryOut");
		Operation = new Pin(manager.WireManager, "ALU Select",PinUtility.SizeToRequiredBits(3));//3 options right now.
		Result = new Pin(manager.WireManager, "ALU Result",width);
		IsZero = new Pin(manager.WireManager, "ALU IsZero");
		InvertA	= new Pin(manager.WireManager, "ALU Invert A");
		InvertB = new Pin(manager.WireManager, "ALU Invert B");
		for (int i = 0; i < width; i++)
		{
			ALUOneBits[i] = new ALUOneBit(_manager);
			if (i > 0)
			{
				ALUOneBits[i-1].CarryOut.ConnectTo(ALUOneBits[i].CarryIn);
			}
			ALUOneBits[i].A.DependsOn(A);//set by system action InputAChanged
			ALUOneBits[i].B.DependsOn(B); //set by system action InputBChanged
			Operation.ConnectTo(ALUOneBits[i].Op);
			InvertA.ConnectTo(ALUOneBits[i].InvertA);
			InvertB.ConnectTo(ALUOneBits[i].InvertB);
			Result.DependsOn(ALUOneBits[i].Result);
			manager.WireManager.RegisterSystemAction(ALUOneBits[i].Result, InternalBitChanged);
		}
		//Does this need to be a pin, or can we just do ALU[7]=>CarryOut
		CarryIn.ConnectTo(ALUOneBits[0].CarryIn);
		ALUOneBits[width-1].CarryOut.ConnectTo(CarryOut);

		manager.WireManager.RegisterSystemAction(A,InputAChanged);
		manager.WireManager.RegisterSystemAction(B, InputBChanged);
		
		IsZero.DependsOn(Result);
		manager.WireManager.RegisterSystemAction(Result,ResultChanged);
		manager.WireManager.SetPin(CarryIn,WireSignal.Low);
	}

	private void ResultChanged(ISystem res)
	{
		//update our various status registers from Result.
		IsZero.Set(PinUtility.ByteArrayToInt(Result.Value) == 0 ? WireSignal.High : WireSignal.Low);
	}

	private void InternalBitChanged(ISystem system)
	{
		//it's probably slower to figure out which bit changed than it is to just set the result.
		//which means that this function will get called 8 times when we change an 8 bit value?
		//I don't... love that... I think I need some kind of buffered call or something.
		//Luckily, because of the toposort system, we won't actually propogate out 8 times. It will just keep getting changed.

		for (int i = 0; i < _width; i++)
		{
			Result.SetBit(i, ALUOneBits[i].Result.Value[0]);
		}
	}

	private void InputAChanged(ISystem obj)
	{
		for (int i = 0; i < A.Value.Length; i++)
		{
			ALUOneBits[i].A.Set(A.Value[i]);
		}
	}

	private void InputBChanged(ISystem obj)
	{
		for (int i = 0; i < B.Value.Length; i++)
		{
			ALUOneBits[i].B.Set(B.Value[i]);
		}
	}
}