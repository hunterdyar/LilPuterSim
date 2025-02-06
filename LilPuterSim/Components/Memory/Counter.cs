using LilPuter.Clock;

namespace LilPuter;

/// <summary>
/// Todo: I don't know the design of my inputs: Load Enable, Count Enable, and Clear (?).
/// But it can be one pin, where it loads if low and counts if high. clear can just be loading 0.
/// </summary>
public class Counter
{
	public ClockPin _ClockPin;
	public readonly Pin CountEnable;
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
		
		CountEnable = new Pin(comp.WireManager, "Counter Count Enable");
		_ClockPin = new ClockPin(comp.Clock);
		Input = new Pin(comp.WireManager, "Counter Input", width);
		Out = new Pin(comp.WireManager, "Counter Out", width);
		
		Out.DependsOn(Input);
		Out.DependsOn(CountEnable);
		
		_ClockPin.OnTick += OnTick;
		_ClockPin.OnTock += OnTock;

	
		//zero the output to start.
		Out.SetSilently(new byte[width]);
	}

	//set internals from inputs
	private void OnTick()
	{
		if (CountEnable.Signal == WireSignal.High)
		{
			_value++;
			if (_value >= _max)
			{
				_value = 0;
			}
		}
		else if (CountEnable.Signal == WireSignal.Low)
		{
			//todo: check if this value is greater than out bit-width, and then set to 0.
			_value = PinUtility.ByteArrayToInt(Input.Value);
		}
	}

	//set output from internals
	private void OnTock()
	{
		Out.Set(PinUtility.IntToByteArray(_value, _width));
	}
}