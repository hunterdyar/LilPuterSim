namespace LilPuter;

public class Adder
{
	public readonly Pin A;
	public readonly Pin B;
	public readonly Pin CarryIn;
	public readonly Pin Out;
	public readonly Pin CarryOut;
	
	public readonly int BitWidth;
	public  FullAdder[] Adders => _adders;
	private readonly FullAdder[] _adders;
	private readonly WireManager _manager;
	
	public Adder(WireManager manager, int bitWidth)
	{
		if (bitWidth <= 1)
		{
			throw new ArgumentException("BitWidth must be greater than 1");
		}
		_manager = manager;
		BitWidth = bitWidth;
		A = new Pin(manager, "Adder A", bitWidth); 
		B = new Pin(manager, "Adder B", bitWidth);
		CarryIn = new Pin(manager, "AdderCarryIn");
		Out = new Pin(manager, "Adder Out", bitWidth);
		CarryOut = new Pin(manager, "AdderCarryOut");
		
		//This system works without the internals! wheee!
		//manager.RegisterSystem([A,B,CarryIn], AdderSystemChange, [Out,CarryOut]);//do the work without simulating the subsystems.
		
		//inputs
		 _adders = new FullAdder[bitWidth];

		 for (int i = 0; i < _adders.Length; i++)
		 {
			 _adders[i] = new FullAdder(manager);
			 _adders[i].A.DependsOn(A);
			 _adders[i].B.DependsOn(B);
			 if (i > 0)
			 {
				 _adders[i-1].CarryOut.ConnectTo(_adders[i].CarryIn);
			 }

			 Out.DependsOn(_adders[i].SumOut);
			 int bit = i;
			 manager.RegisterSystemAction(_adders[i].SumOut, system => { Out.SetBit(bit, (WireSignal)_adders[bit].SumOut.Value); });
		 }

		 manager.RegisterSystemAction(A, InputAChanged);
		 manager.RegisterSystemAction(B, InputBChanged);
		 //_adders[0].CarryIn.DependsOn(CarryIn);
		 _adders[bitWidth-1].CarryOut.ConnectTo(CarryOut);

		 //connections
		 CarryIn.ConnectTo(_adders[0].CarryIn);

		 //system notes.
		 //Carry out is depending correctly on the last adder.
		 Out.DependsOn(A);
		 Out.DependsOn(B);
		 Out.DependsOn(CarryIn);
		 
	}
	
	private void AdderSystemChange(ISystem obj)
	{
		var a = A.Value;
		var b = B.Value;
		var carryIn = CarryIn.Value;
		var result = a + b + carryIn;
		int max = (int)Math.Pow(2, BitWidth);
		if (result >= max)
		{
			CarryOut.Set(WireSignal.High);
			Out.Set(result-max);
		}
		else
		{
			CarryOut.Set(WireSignal.Low);
			Out.Set(result);
		}
	}

	//hmmmm
	private void InputAChanged(ISystem changedSystem)
	{
		for (var i = 0; i < BitWidth; i++)
		{
			_adders[i].A.Set((A.Value >> i) & 1);
		}

		Console.WriteLine("Adder InputA Changed");
	}

	private void InputBChanged(ISystem changedSystem)
	{
		for (var i = 0; i < BitWidth; i++)
		{
			_adders[i].B.Set((B.Value >> i) & 1);
		}
		Console.WriteLine("Adder InputB Changed");
	}
}