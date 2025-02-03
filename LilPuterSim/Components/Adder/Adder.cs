namespace LilPuter;

public class Adder
{
	public readonly Pin A;
	public readonly Pin B;
	public readonly Pin CarryIn;
	public readonly Pin Out;
	public readonly Pin CarryOut;
	
	public readonly int BitWidth;
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
			 manager.RegisterSystemAction(_adders[i].SumOut, system => { Out.SetBit(bit, _adders[bit].SumOut.Value[0]); });
		 }

		 manager.RegisterSystemAction(A, InputAChanged);
		 manager.RegisterSystemAction(B, InputBChanged);
		 _adders[0].CarryIn.DependsOn(CarryIn);
		 _adders[1].CarryOut.ConnectTo(CarryOut);

		 //connections
		 Out.DependsOn(A);
		 Out.DependsOn(B);
		 Out.DependsOn(CarryIn);
		 
		 //outputs
		 manager.RegisterSystemAction(CarryIn, CarryInChanged);
		 
		//manager.RegisterSystem([A, B, CarryIn], InputChanged, [Out, CarryOut]);
	}

	public bool ValidateTopoSortInternals()
	{
		var s = _manager.GetTopoSort();

		var a = s.IndexOf(A);
		var b = s.IndexOf(B);
		var c = s.IndexOf(CarryIn);
		var so = s.IndexOf(Out);
		var co = s.IndexOf(CarryOut);
		var a0so = s.IndexOf(_adders[0].SumOut);
		var a1so = s.IndexOf(_adders[1].SumOut);
		var a0a = s.IndexOf(_adders[0].A);
		var a1a = s.IndexOf(_adders[1].A);
		var a0b = s.IndexOf(_adders[0].B);
		var a1b = s.IndexOf(_adders[1].B);
		var a0co = s.IndexOf(_adders[0].CarryOut);
		var a1co = s.IndexOf(_adders[1].CarryOut);
		
		if (a > a0a || a > a1a || a > a0so || a > a1so)
		{
			return false;
		}
		
		if (b > a1b || b > a1so || b > a0b || b > a0so)
		{
			return false;
		}

		if (c > a1so || c > a0a || c > a0b || c > a0so || c > a1so)
		{
			return false;
		}

		if (so < a || so < b || so < c || so < a0so || so < a1so)
		{
			return false;
		}

		if (a1co < a0co)
		{
			return false;
		}
		
		if (co < a || co < b || co < c || co < a1co || co < a0co)
		{
			return false;
		}
		
		return true;
	}

	private void AdderSystemChange(ISystem obj)
	{
		var a = PinUtility.ByteArrayToInt(A.Value);
		var b = PinUtility.ByteArrayToInt(B.Value);
		var carryIn = (int)CarryIn.Value[0];
		var result = a + b + carryIn;
		int max = (int)Math.Pow(2, BitWidth);
		if (result >= max)
		{
			CarryOut.Set(WireSignal.High);
			Out.Set(PinUtility.IntToByteArray(result-max, BitWidth));
		}
		else
		{
			CarryOut.Set(WireSignal.Low);
			Out.Set(PinUtility.IntToByteArray(result, BitWidth));
		}
	}

	//hmmmm
	private void InputAChanged(ISystem changedSystem)
	{
		for (var i = 0; i < BitWidth; i++)
		{
			_adders[i].A.Set([A.Value[i]]);
		}

		Console.WriteLine("Adder InputA Changed");
	}

	private void InputBChanged(ISystem changedSystem)
	{
		for (var i = 0; i < BitWidth; i++)
		{
			_adders[i].B.Set([B.Value[i]]);
		}
		Console.WriteLine("Adder InputB Changed");
	}

	private void CarryInChanged(ISystem changedSystem)
	{
		_adders[0].CarryIn.Set(CarryIn.Value);
	}
}