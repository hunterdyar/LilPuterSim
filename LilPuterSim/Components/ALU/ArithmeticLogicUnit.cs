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
		Y = new Pin(manager, "ALU Y", bitWidth);
		ZX = new Pin(manager, "ALU ZX");
		NX = new Pin(manager, "ALU NX");
		ZY = new Pin(manager, "ALU ZY");
		NY = new Pin(manager, "ALU NY");
		F = new Pin(manager, "ALU F");
		NO = new Pin(manager, "ALU NO");
		//outs
		Out = new Pin(manager, "ALU Out", bitWidth);
		ZR = new Pin(manager, "ALU ZR");
		NG = new Pin(manager, "ALU NG");
		Overflow = new Pin(manager, "ALU Overflow");
		//
		_adder = new Adder(manager, bitWidth);
		_andBank = new AndBank(manager, bitWidth);
		
		//
		_adder.A.DependsOn(X);
		_andBank.A.DependsOn(X);
		_adder.A.DependsOn(Y);
		_andBank.A.DependsOn(Y);
		
		//connect both and only update one of them internally?
		_adder.Out.ConnectTo(Out);
		_andBank.Out.ConnectTo(Out);
		
		//todo: we need some kind of clock or pooling setup. Breath-First is the engine that should stop this, we should update all of the xy,etc;
		//but the duplicates aren't getting checked.
		//Currently the pinChange checks if it's already in the list, but that's not enough.
		manager.RegisterSystemAction(ZX, Trigger);
		manager.RegisterSystemAction(ZY, Trigger);
		manager.RegisterSystemAction(NX, Trigger);
		manager.RegisterSystemAction(NY, Trigger);
		manager.RegisterSystemAction(F, ALUTypeChanged);
		manager.RegisterSystemAction(F, Trigger);
		manager.RegisterSystemAction(X, Trigger);
		manager.RegisterSystemAction(Y, Trigger);
		
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
		
		//Assume the internals of X and Y have already propogated.
		_manager.Impulse(F);
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
			Out.Set(_adder.Out.Value);
		}
		else
		{
			Out.Set(_andBank.Out.Value);
		}
		
		if(NO.Value.IsHigh())
		{
			Out.InvertSilent();
		}
	}
}
