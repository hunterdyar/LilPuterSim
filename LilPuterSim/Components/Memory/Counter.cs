using LilPuter.Clock;

namespace LilPuter;

/// <summary>
/// By default, we connect counters to the clock. The load pin is for setting the clock to a value.
/// </summary>
public class Counter
{
	public ClockPin _ClockPin;
	//Counts when High.
	public readonly Pin LoadInput;
	public readonly Pin LoadEnable;

	public readonly Pin CountEnable;
	
	public readonly Pin Reset;
	
	public readonly Pin Out;//Current counter value.
	
	private int _value;
	private readonly int _width;
	private readonly int _max;

	///	<param name="max">Exclusive Upper end of counter before looping to zero. If set as negative, will use 2^width.</param>
	public Counter(ComputerBase comp, int width, int max = -1)
	{
		_width = width;
		if (max < 0)
		{
			this._max = (int)Math.Pow(2, width); //8 bits is 255.
		}
		else
		{
			this._max = max;
		}

		_value = 0;

		Reset = new Pin(comp.WireManager, "PC Counter Reset");
		LoadInput = new Pin(comp.WireManager, "PC Load Data");
		LoadEnable = new Pin(comp.WireManager, "PC Load Enabled");
		CountEnable = new Pin(comp.WireManager, "PC Count Enable");
		
		_ClockPin = new ClockPin(comp.Clock);
		Out = new Pin(comp.WireManager, "Counter Out", width);
		
		Out.DependsOn(LoadInput);
		LoadInput.DependsOn(Reset);
		Out.DependsOn(LoadEnable);
		Out.DependsOn(Reset);
		Out.DependsOn(CountEnable);

		_ClockPin.OnTick += OnTick;
		_ClockPin.OnTock += OnTock;

		comp.WireManager.RegisterSystemAction(Reset, OnResetChange);
		comp.WireManager.RegisterSystemAction(LoadInput, OnInputChange);
		comp.WireManager.RegisterSystemAction(LoadEnable, OnInputChange);
		
		//zero the output to start.
		Out.SetSilently(0);
		Reset.SetSilently(WireSignal.Low);
	}

	private void OnTock()
	{
		Out.Set(_value);
	}

	private void OnTick()
	{
		if (Reset.Signal == WireSignal.High)
		{
			_value = 0;
			Out.Set(0);
		}else if (LoadEnable.Signal == WireSignal.Low)
		{
			if (CountEnable.Signal == WireSignal.High)
			{
				_value++;
				if (_value >= _max)
				{
					_value = 0;
				}
			}
		}
	}
	

	private void OnInputChange(ISystem obj)
	{
		if (Reset.Signal == WireSignal.High)
		{
			_value = 0;
			Out.Set(0);
		}else if (LoadEnable.Signal == WireSignal.High)
		{
			_value = LoadInput.Value;
			Out.Set(_value);
		}
	}

	//Resets the clock. Independent from clock.
	private void OnResetChange(ISystem obj)
	{
		if (Reset.Signal == WireSignal.High)
		{
			_value = 0;
			Out.Set(0);
		}
	}

}