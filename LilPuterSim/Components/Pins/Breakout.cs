namespace LilPuter;

public class Breakout : SubscriberBase<int>
{
	public Pin Input;
	public Pin[] OutPins => _outs;
	public readonly Pin[] _outs;
	
	public readonly int Width = 1;
	public Breakout(WireManager wireManager,string name, int width)
	{
		Input = new Pin(wireManager,"Breakout In", width);
		Width = width;
		
		_outs = new Pin[width];
		for (int i = 0; i < width; i++)
		{
			_outs[i] = new Pin(wireManager, "Breakout_" + i);
			_outs[i].DependsOn(Input);
		}
		wireManager.RegisterSystemAction(Input, OnChange);
	}

	private void OnChange(ISystem obj)
	{
		var val = Input.Value;
		for (int i = 0; i < Width; i++)
		{
			int bitVal = (val >> i) & 1;
			_outs[i].Set(bitVal);
		}
	}

	public override int ReadValue()
	{
		return Input.Value;
	}
}