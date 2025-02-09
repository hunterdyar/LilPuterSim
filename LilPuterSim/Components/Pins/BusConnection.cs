namespace LilPuter;

public class BusConnection
{
	public string Name;
	public bool Enabled { get; private set; }
	public bool IsInput;
	public required Pin Pin;
	public Pin? LoadPin;//gets set before the input pin gets set, if it's an input pin. (or output)
	public bool InvertedLoad;
	public int Index;

	/// <summary>
	/// Sets enabled and updates the given load pin.
	/// </summary>
	public void SetEnabled(bool enabled)
	{
		if (enabled != Enabled)
		{
			Enabled = enabled;
			SetLoadPin(enabled);
		}
	}
	/// <summary>
	/// Sets or Unsets the load pin, inverting if neccesary and ignoring if null.
	/// </summary>
	private void SetLoadPin(bool enabled)
	{
		if (LoadPin != null)
		{
			if (!InvertedLoad)
			{
				LoadPin.Set(enabled? WireSignal.High : WireSignal.Low);
			}
			else
			{
				LoadPin.Set(enabled? WireSignal.Low : WireSignal.High);
			}
		}
		
	}
}