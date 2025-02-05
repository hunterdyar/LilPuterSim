using LilPuter.Clock;

namespace LilPuter;

/// <summary>
/// We do bot simulate the DFF with nand gates because I do not currently support acyclic graphs.
/// So, DFF will be one of our fundamental building blocks.
/// I could, however, still build a visualizer for one - it would just only work in reverse, showing the nand gates and assigning their values from the outputs.
/// So it isn't part of the simulation, just a hack for the visualizer.
/// </summary>
public class DataFlipFlop
{
	public readonly Pin In;
	public readonly ClockPin Clock; //we will change clock from a clock to it's own class.
	public readonly Pin Out;
	private byte[] _holdingVal;
	public DataFlipFlop(WireManager manager, ClockManager clock)
	{
		In = new Pin(manager, "DFF In");
		Out = new Pin(manager, "DFF Out");
		Clock = new ClockPin(clock, "DFF Clock In");
		Clock.OnTick += OnTick;
		Clock.OnTock += OnTock;
	}

	private void OnTick()
	{
		_holdingVal = In.Value;
	}
	private void OnTock()
	{
		Out.Set(_holdingVal);
	}
}