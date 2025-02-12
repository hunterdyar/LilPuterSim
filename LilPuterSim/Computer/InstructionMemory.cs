namespace LilPuter
{
	/// <summary>
	/// A light wrapper for a bit of RAM that splits the output into two pin lines, address and output.
	/// Each are 8 bits, but in "real life" we wouldn't bother with extra pins.
	/// Right now we only have one operand. 
	/// </summary>
	public class InstructionMemory
	{
		//Input
		public Pin Address => _memory.Address;

		//Output
		public readonly Pin Instruction;
		public readonly Pin Operand;

		public int[] Program => _memory.Registers;
		private readonly RAM _memory;

		private int _iWidth;
		private int _dWidth;
		private int _dataMask;
		public InstructionMemory(ComputerBase comp, int instructionWidth, int dataWidth)
		{
			_iWidth = instructionWidth;
			_dWidth = dataWidth;
			_dataMask = 0;
			_memory = new RAM(comp, "Instruction Memory", 16, 1024);

			for (int i = 0; i < dataWidth; i++)
			{
				_dataMask <<= 1;
				_dataMask |= 1;
			}
			Address.Name ="Instruction Memory Address";
			Instruction = new Pin(comp.WireManager,"Instruction Memory Instruction");
			Operand = new Pin(comp.WireManager,"Instruction Memory Operand");

		
			//tie down load. We will modify the memory directly when loading a program.
			_memory.Load.Set(0);
		
			Instruction.DependsOn(_memory.Out);
			Operand.DependsOn(_memory.Out);
		
			comp.WireManager.RegisterSystemAction(_memory.Out, OnMemoryOutChange);
		
		}

		private void OnMemoryOutChange(ISystem obj)
		{
			Instruction.Set(_memory.Out.Value >> _dWidth);
			Operand.Set(_memory.Out.Value & _dataMask);
		}
	
		public void Clear()
		{
			for (int i = 0; i < _memory.Registers.Length; i++)
			{
				_memory.Registers[i] = 0;
			}
		}

		public void ForceUpdateOutputs()
		{
			_memory.ForceUpdateOutput();
			OnMemoryOutChange(Address);
		}
	}
}