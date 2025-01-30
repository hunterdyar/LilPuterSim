namespace LilPuter;

public class Adder
{
	public Pin A;
	public Pin B;
	public Pin CarryIn;
	public Pin CarryOut;
	public Pin Out;
	public int bitWidth;
	private FullAdder[] _adders;
	private WireManager _manager;
	
	public Adder(WireManager manager, int bitWidth)
	{
		_manager = manager;
		this.bitWidth = bitWidth;
		A = new Pin(manager, "AdderA", bitWidth);
		B = new Pin(manager, "AdderB", bitWidth);
		CarryIn = new Pin(manager, "AdderCarryIn");
		CarryOut = new Pin(manager, "AdderCarryOut");
		Out = new Pin(manager, "AdderOut", bitWidth);
		
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