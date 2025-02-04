namespace LilPuter;

public class ArithmeticLogicUnit
{
	
	public Pin X;
	public Pin Y;
	///Input Control Pins
	/// <summary>
	/// Zero the X Input
	/// </summary>
	public Pin ZX;

	/// <summary>
	/// Negate the X Input
	/// </summary>
	public Pin NX;
	
	/// <summary>
	/// Zero the Y Input
	/// </summary>
	public Pin ZY;

	/// <summary>
	/// Negate the Y Input
	/// </summary>
	public Pin NY;
	
	/// <summary>
	/// If f ==1, out= Add(x,y) else out = And(x,y)
	/// </summary>
	public Pin F;

	/// <summary>
	/// Negate the Output
	/// </summary>
	public Pin NO;
	
	//Output Pins
	private readonly Pin _rawOut;
	public Pin Out;
	/// <summary>
	/// HIGH when output is zero.:
	/// </summary>
	public Pin ZR;

	/// <summary>
	/// Set when output is negative.
	/// </summary>
	public Pin NG;
	
	/// <summary>
	/// Set when input overflows
	/// </summary>
	public Pin Overflow;
	
	//private AndBank _andBank;
	private readonly Adder _adder;
	private WireManager _manager;

	public ArithmeticLogicUnit(WireManager manager, int bitWidth)
	{
		_manager = manager;
		//ins
		X = new Pin(manager, "ALU X", bitWidth);
		ZX = new Pin(manager, "ALU ZX");
		NX = new Pin(manager, "ALU NX");
		
		Y = new Pin(manager, "ALU Y", bitWidth);

		ZY = new Pin(manager, "ALU ZY");
		NY = new Pin(manager, "ALU NY");
		
		F = new Pin(manager, "ALU F");
		NO = new Pin(manager, "ALU NO");
		//outs
		Out = new Pin(manager, "ALU Out", bitWidth);
		_rawOut = new Pin(manager, "ALU Raw Out", bitWidth);
		ZR = new Pin(manager, "ALU ZR");
		NG = new Pin(manager, "ALU NG");
		Overflow = new Pin(manager, "ALU Overflow");
		//
		_adder = new Adder(manager, bitWidth);
		//_andBank = new AndBank(manager, bitWidth);
		//
		_rawOut.DependsOn(_adder.A);
		_rawOut.DependsOn(_adder.B);
		//andbank
		_rawOut.DependsOn(NX);
		_rawOut.DependsOn(ZX);
		_rawOut.DependsOn(NY);
		_rawOut.DependsOn(ZY);
		_rawOut.DependsOn(F);
		_rawOut.DependsOn(NO);

		Out.DependsOn(_rawOut);
		
		Overflow.DependsOn(Out);
		NG.DependsOn(Out);
		ZR.DependsOn(Out);
		//
		
		//When X or Y change, run ExsternalInputChange which inverts or zeros the internal (_adder.A) values appropriately.
		_adder.A.DependsOn(X);
		_adder.B.DependsOn(Y);
		
		manager.RegisterSystemAction(X, ExternalInputChange);
		manager.RegisterSystemAction(Y, ExternalInputChange);
		//connect both and only update one of them internally?
		_adder.Out.ConnectTo(_rawOut);
		//_andBank.Out.ConnectTo(RawOut);
		
		manager.RegisterSystemAction(ZX, Trigger);
		manager.RegisterSystemAction(ZY, Trigger);
		manager.RegisterSystemAction(NX, Trigger);
		manager.RegisterSystemAction(NY, Trigger);
		manager.RegisterSystemAction(F, ALUTypeChanged);
		
		manager.RegisterSystemAction(_rawOut, RawOutChanged);

		//Config! We don't use the adder. so it needs to get, erm, grounded.
		manager.SetPin(_adder.CarryIn, WireSignal.Low);
	}
	/// <summary>
	/// Set all input data once so we only propogate changes once.
	/// </summary>
	/// <param name="f">High for Add, Low for And</param>
	/// <param name="zx">Zero the X</param>
	/// <param name="nx">Invert the X Input before calculation</param>
	/// <param name="zy">Zero the Y</param>
	/// <param name="ny">Invert the Y</param>
	/// <param name="no">Invert the output</param>
	public void SetInputs(WireSignal f, WireSignal zx, WireSignal nx, WireSignal zy, WireSignal ny, WireSignal no)
	{
		ZX.Set(zx);
		NX.Set(nx);
		ZY.Set(zy);
		NY.Set(ny);
		NO.Set(no);
		F.Set(f);
	}

	private void ExternalInputChange(ISystem system)
	{
		if (system == X)
		{
			var val = X.Value;
			if (ZX.Signal == WireSignal.High)
			{
				val = new byte[X.Value.Length];
			}else if (NX.Signal == WireSignal.High)//else if b/c we don't have to bother inverting 0's one way or the other.
			{
				val = PinUtility.Invert(val);
			}
			
			_adder.A.Set(val);
			//_andBank.A.Set(val);
		}else if (system == Y)
		{
			var val = Y.Value;
			if (ZY.Signal == WireSignal.High)
			{
				val = new byte[Y.Value.Length];
			}
			else if (NY.Signal == WireSignal.High)
			{
				val = PinUtility.Invert(val);
			}

			_adder.B.Set(val);
			//_andBank.B.Set(val);
		}
	}

	private void ALUTypeChanged(ISystem pin)
	{
		if (F.Signal == WireSignal.High)
		{
			//enable adder, disable and-er
		}
		else if(F.Signal == WireSignal.Low)
		{
			throw new NotImplementedException("& operation not yet implemented");
			//enable and-er, disable adder
		}
		//now directly call the follow-up, because I don't want to deal with having to worry about the order of SystemActions.
		Trigger(pin);
	}

	void RawOutChanged(ISystem system)
	{
		if (NO.Signal == WireSignal.High)
		{
			Out.Set(PinUtility.Invert(_rawOut.Value));
		}
		else
		{
			Out.Set(_rawOut.Value);
		}
		
		//we read Out because inverted.
		var r = PinUtility.ByteArrayToInt(Out.Value);
		if (r == 0)
		{
			ZR.Set(WireSignal.High);
		}
		else
		{
			ZR.Set(WireSignal.Low);
		}

		if (r < 0)
		{
			NG.Set(WireSignal.High);
		}
		else
		{
			NG.Set(WireSignal.Low);
		}
		
		Overflow.Set(_adder.CarryOut.Value);
	}

	public void Trigger(ISystem pin)
	{
		//todo: these modifiers.
		// if (ZX.Value.IsHigh())
		// {
		// 	X.ZeroOutSilent();
		// }
		//
		// if (ZY.Value.IsHigh())
		// {
		// 	Y.ZeroOutSilent();
		// }
		// if(NX.Value.IsHigh())
		// {
		// 	X.InvertSilent();
		// }
		//
		// if (NY.Value.IsHigh())
		// {
		// 	Y.InvertSilent();
		// }
		
		if (F.Value.IsHigh())
		{
			_rawOut.Set(_adder.Out.Value);
		}
		else if(F.Signal == WireSignal.Low)
		{
			throw new NotImplementedException("And operation not yet implemented");
			//RawOut.Set(_andBank.Out.Value);
		}//else it's floating. ignore.
	}
}
