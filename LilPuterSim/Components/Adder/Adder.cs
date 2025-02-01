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
		if (_needsSimulation)
		{
			//does this get set when things change or can we force a simulation?
		}
		//todo: only impulse if different since last impulse.
		//pin's set dirty when set and un-dirty when false.
		
		_manager.Impulse(A);
		_manager.Impulse(B);
		_manager.Impulse(CarryIn);
		//
		base.Simulate();
		//
		//Now 
	}

	//hmmmm
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
		
		SetNeedsSimulation();
	}
}