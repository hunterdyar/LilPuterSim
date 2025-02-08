using LilPuter.Clock;

namespace LilPuter;

/// <summary>
/// Todo: I don't know the design of my inputs: Load Enable, Count Enable, and Clear (?).
/// But it can be one pin, where it loads if low and counts if high. clear can just be loading 0.
/// </summary>
public class Counter
{
	public ClockPin _ClockPin;
	//Counts when High.
	public readonly Pin Load;
	public readonly Pin CountEnable;
	public readonly Pin Reset;
	public readonly Pin Input;
	public readonly Pin Out;
	
	private int _value;
	private readonly int _width;
	private readonly int _max;
	public Counter(ComputerBase comp, int width)
	{
		_width = width;
		this._max = (int)Math.Pow(2,width);//8 bits is 255.
		_value = 0;

		Reset = new Pin(comp.WireManager, "Counter Reset");
		Load = new Pin(comp.WireManager, "Load");
		CountEnable = new Pin(comp.WireManager, "Counter Count Enable");
		_ClockPin = new ClockPin(comp.Clock);
		Input = new Pin(comp.WireManager, "Counter Input", width);
		Out = new Pin(comp.WireManager, "Counter Out", width);
		
		Out.DependsOn(Load);
		Input.DependsOn(Reset);
		Out.DependsOn(Input);
		Out.DependsOn(Reset);
		Out.DependsOn(CountEnable);
		
		_ClockPin.OnTick += OnTick;
		_ClockPin.OnTock += OnTock;

		comp.WireManager.RegisterSystemAction(Reset, OnReset);
		//zero the output to start.
		Out.SetSilently(0);
	}

	//Resets the clock. Independent from clock.
	private void OnReset(ISystem obj)
	{
		if (Reset.Signal == WireSignal.High)
		{
			_value = 0;
			Out.Set(0);
		}
	}

	//set internals from inputs
	private void OnTick()
	{
		//If load is high, load regardless of other pins.
		if (Load.Signal == WireSignal.High)
		{
			_value = Input.Value;
		} else if (CountEnable.Signal == WireSignal.High)
		{
			if (Reset.Signal == WireSignal.Low)
			{
				_value++;
				if (_value >= _max)
				{
					_value = 0;
				}
			}
		}
		else if (Reset.Signal == WireSignal.High)
		{
			_value = 0;
		}
	}

	//set output from internals
	private void OnTock()
	{
		Out.Set(_value);
	}
}