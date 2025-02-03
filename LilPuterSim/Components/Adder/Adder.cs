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
		 _adders = new FullAdder[2];
		 _adders[0] = new FullAdder(manager);
		 _adders[1] = new FullAdder(manager);
		 _adders[0].A.DependsOn(A);
		 _adders[1].A.DependsOn(A);
		 manager.RegisterSystemAction(A,InputAChanged);
		 _adders[0].B.DependsOn(B);
		 _adders[1].B.DependsOn(B);
		 manager.RegisterSystemAction(B,InputBChanged);
		 _adders[0].CarryIn.DependsOn(CarryIn);
		 //connections
		 _adders[0].CarryOut.ConnectTo(_adders[1].CarryIn);
		 
		 //outputs
		 Out.DependsOn(_adders[0].SumOut);
		 manager.RegisterSystemAction(_adders[0].SumOut, system =>
		 {
			 Out.SetBit(0, _adders[0].SumOut.Value[0]);
		 });
		 Out.DependsOn(_adders[1].SumOut);
		 manager.RegisterSystemAction(_adders[1].SumOut, system =>
		 {
			 Out.SetBit(1, _adders[1].SumOut.Value[0]);
		 });
		 
		 manager.RegisterSystemAction(CarryIn, CarryInChanged);
		 

		//manager.RegisterSystem([A, B, CarryIn], InputChanged, [Out, CarryOut]);
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