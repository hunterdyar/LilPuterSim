namespace LilPuter;

public class Adder : SimSystem
{
	public Pin A => Inputs[0];
	public Pin B => Inputs[1];
	public Pin CarryIn => Inputs[2];
	public Pin Out => Outputs[0];
	public Pin CarryOut => Outputs[1];
	
	public int bitWidth;
	private FullAdder[] _adders;
	private WireManager _manager;
	
	public Adder(WireManager manager, int bitWidth)
	{
		_manager = manager;
		Inputs = new Pin[3];
		Outputs = new Pin[2];
		this.bitWidth = bitWidth;
		Inputs[0] = new Pin(manager, "AdderA", bitWidth); 
		Inputs[1] = new Pin(manager, "AdderB", bitWidth);
		Inputs[2] = new Pin(manager, "AdderCarryIn");
		Outputs[0] = new Pin(manager, "AdderOut", bitWidth);
		Outputs[1] = new Pin(manager, "AdderCarryOut");
		
		Out.PinWeight++;//Resolve all internal pins before setting the output.
		
		_adders = new FullAdder[bitWidth];
		for (var i = 0; i < bitWidth; i++)
		{
			_adders[i] = new FullAdder(manager);
			int bit = i;
			_manager.Listen(_adders[i].SumOut, (p) =>
			{
				Out.SetBit(bit, p.Value[0]);
			});
		}
		CarryIn.ConnectTo(_adders[0].CarryIn);

		//connect them up!
		for (int i = 0; i < this.bitWidth-1; i++)
		{
			_adders[i].CarryOut.ConnectTo(_adders[i + 1].CarryIn);
		}
		_adders[bitWidth-1].CarryOut.ConnectTo(CarryOut);
		
		manager.Listen(A, InputChanged);
		manager.Listen(B, InputChanged);
	}

	public override void Simulate()
	{
		//todo
		base.Simulate();
	}

	private void InputChanged(Pin changed)
	{
		if (changed == A)
		{
			for (var i = 0; i < bitWidth; i++)
			{
				_adders[i].A.Set([changed.Value[i]]);
			}
		}
		else if (changed == B)
		{
			for (var i = 0; i < bitWidth; i++)
			{
				_adders[i].B.Set([changed.Value[i]]);
			}
		}
		else
		{
			throw new Exception("Adder accidentally listening to wrong pin");
		}
		
		//Now we need to set the output! But like, after the rest of the propogation.
		
	}
}