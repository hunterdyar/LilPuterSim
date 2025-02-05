namespace LilPuter;

public class Bit : Register
{
	public Bit(WireManager manager, ClockManager clock) : base(manager, clock, 1)
	{
	}
}