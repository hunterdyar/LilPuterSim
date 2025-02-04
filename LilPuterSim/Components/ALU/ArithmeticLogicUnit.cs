namespace LilPuter;

public class ArithmeticLogicUnit
{
	
	public Pin X;
	private Pin _x;
	public Pin Y;
	private Pin _y;
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
	private Pin RawOut;
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
	
	private AndBank _andBank;
	private Adder _adder;
	private WireManager _manager;

	public ArithmeticLogicUnit(WireManager manager, int bitWidth)
	{
		_manager = manager;
		//ins
		X = new Pin(manager, "ALU X", bitWidth);
		_x = new Pin(manager, "ALU Internal X", bitWidth);
		ZX = new Pin(manager, "ALU ZX");
		NX = new Pin(manager, "ALU NX");
		
		Y = new Pin(manager, "ALU Y", bitWidth);
		_y = new Pin(manager, "ALU Internal Y", bitWidth);

		ZY = new Pin(manager, "ALU ZY");
		NY = new Pin(manager, "ALU NY");
		
		F = new Pin(manager, "ALU F");
		NO = new Pin(manager, "ALU NO");
		//outs
		Out = new Pin(manager, "ALU Out", bitWidth);
		RawOut = new Pin(manager, "ALU Raw Out", bitWidth);
		ZR = new Pin(manager, "ALU ZR");
		NG = new Pin(manager, "ALU NG");
		Overflow = new Pin(manager, "ALU Overflow");
		//
		_adder = new Adder(manager, bitWidth);
		_andBank = new AndBank(manager, bitWidth);
		//
		RawOut.DependsOn(_x);
		RawOut.DependsOn(_y);
		RawOut.DependsOn(NX);
		RawOut.DependsOn(ZX);
		RawOut.DependsOn(NY);
		RawOut.DependsOn(ZY);
		RawOut.DependsOn(F);
		RawOut.DependsOn(NO);

		Out.DependsOn(RawOut);
		
		Overflow.DependsOn(Out);
		NG.DependsOn(Out);
		ZR.DependsOn(Out);
		//
		_x.ConnectTo(_adder.A);
		_x.ConnectTo(_andBank.A);
		_y.ConnectTo(_adder.B);
		_y.ConnectTo(_andBank.B);
		
		_x.DependsOn(X);
		_y.DependsOn(Y);
		manager.RegisterSystemAction(X,ExternalInputChange);
		manager.RegisterSystemAction(Y, ExternalInputChange);
		//connect both and only update one of them internally?
		_adder.Out.ConnectTo(RawOut);
		_andBank.Out.ConnectTo(RawOut);
		
		//todo: we need some kind of clock or pooling setup. Breath-First is the engine that should stop this, we should update all of the xy,etc;
		//but the duplicates aren't getting checked.
		//Currently the pinChange checks if it's already in the list, but that's not enough.
		manager.RegisterSystemAction(ZX, Trigger);
		manager.RegisterSystemAction(ZY, Trigger);
		manager.RegisterSystemAction(NX, Trigger);
		manager.RegisterSystemAction(NY, Trigger);
		manager.RegisterSystemAction(F, ALUTypeChanged);
		manager.RegisterSystemAction(F, Trigger);
		//manager.RegisterSystemAction(X, Trigger);
		//manager.RegisterSystemAction(Y, Trigger);
		
		manager.RegisterSystemAction(RawOut, RawOutChanged);

		//Config!
		_adder.CarryIn.Set(WireSignal.Low);
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
		F.Set(f);
		NO.Set(no);
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
			
			_x.Set(val);
			
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

			_y.Set(val);
		}
	}

	private void ALUTypeChanged(ISystem pin)
	{
		if (F.Signal == WireSignal.High)
		{
			//enable adder, disable and-er
		}
		else
		{
			//enable and-er, disable adder
		}
		
	}

	void RawOutChanged(ISystem system)
	{
		if (NO.Signal == WireSignal.High)
		{
			Out.Set(PinUtility.Invert(RawOut.Value));
		}
		else
		{
			Out.Set(RawOut.Value);
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
	// Todo: ... just do it!
	public void Trigger(ISystem pin)
	{
		if (ZX.Value.IsHigh())
		{
			X.ZeroOutSilent();
		}

		if (ZY.Value.IsHigh())
		{
			Y.ZeroOutSilent();
		}
		if(NX.Value.IsHigh())
		{
			X.InvertSilent();
		}

		if (NY.Value.IsHigh())
		{
			Y.InvertSilent();
		}
		
		if (F.Value.IsHigh())
		{
			RawOut.Set(_adder.Out.Value);
		}
		else
		{
			RawOut.Set(_andBank.Out.Value);
		}
		
		
	}
}
