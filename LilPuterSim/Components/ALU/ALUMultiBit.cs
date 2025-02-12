namespace LilPuter
{
	public class ALUMultiBit
	{
		public readonly ALUOneBit[] ALUOneBits;
		public readonly Pin Operation;
		public readonly Pin A;
		public readonly Pin B;
		private readonly Pin CarryIn;

		public readonly Pin InvertA;
		public readonly Pin InvertB;
	
		//Outputs
		public readonly Pin CarryOut;//overflow
		public readonly Pin Result;
		public readonly Pin IsZero;

		private readonly int _width;
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

			manager.WireManager.RegisterSystemAction(A, InputAChanged);
			manager.WireManager.RegisterSystemAction(B, InputBChanged);
		
			IsZero.DependsOn(Result);
			manager.WireManager.RegisterSystemAction(Result,ResultChanged);
			manager.WireManager.SetPin(CarryIn,WireSignal.Low);
		
			//this is implicit via the 1bit registers.  
			Result.DependsOn(A);
			Result.DependsOn(B);
		}

		private void ResultChanged(ISystem res)
		{
			//update our various status registers from Result.
			IsZero.Set(Result.Value == 0 ? WireSignal.High : WireSignal.Low);
		}

		private void InternalBitChanged(ISystem system)
		{
			//it's probably slower to figure out which bit changed than it is to just set the result.
			//which means that this function will get called 8 times when we change an 8 bit value?
			//I don't... love that... I think I need some kind of buffered call or something.
			//Luckily, because of the toposort system, we won't actually propogate out 8 times. It will just keep getting changed.
			int before = Result.Value;
			int val = 0;
			for (int i = 0; i < _width; i++)
			{
				//todo: Test this
				val = (ALUOneBits[i].Result.Value << (i)) | val;
			}

			Result.Set(val);

		}

		private void InputAChanged(ISystem obj)
		{
			for (int i = 0; i < A.Width; i++)
			{
				ALUOneBits[i].A.Set((A.Value >> i) & 1);
			}
		}

		private void InputBChanged(ISystem obj)
		{
			for (int i = 0; i < B.Width; i++)
			{
				ALUOneBits[i].B.Set((B.Value >> i) & 1);
			}
		}
	}
}