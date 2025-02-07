namespace LilPuter;

public class Bus
{
	public int Value => Value;
	private int _value;

	
	public Pin[] Inputs;
	private bool[] InEnables;
	public Pin[] Outputs;
	private bool[] OutEnables;

	private int _width;

	public Bus(int dataWidth)
	{
		_width = dataWidth;
		///
	}

	public void SetControl(int pin, bool enabled)
	{
		// InEnables[pin] = enabled;
	}
	
	public void Trigger()
	{
		//Get the input value that is enabled and set our value to it's value.
		//error if multiple pins are enabled.
		
		//Get the output pins that are enabled and set their value to value.
	}
	
	
}