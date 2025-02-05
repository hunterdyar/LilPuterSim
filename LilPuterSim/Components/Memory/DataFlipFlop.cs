namespace LilPuter;

/// <summary>
/// We do bot simulate the DFF with nand gates because I do not currently support acyclic graphs.
/// So, DFF will be one of our fundamental building blocks.
/// I could, however, still build a visualizer for one - it would just only work in reverse, showing the nand gates and assigning their values from the outputs.
/// So it isn't part of the simulation, just a hack for the visualizer.
/// </summary>
public class DataFlipFlop
{
	public Pin In;
	public Pin Clock; //we will change clock from a clock to it's own class.
	
	public Pin Out;
	
	public DataFlipFlop(WireManager manager)
	{
		
	}
}