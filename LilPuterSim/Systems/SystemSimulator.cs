namespace LilPuter;

public class SystemSimulator
{
    //A calling of Simulate for each system. Is it BF or DF or topological sorted? Breath for now!

    private Queue<SimSystem> _toSimulate = new Queue<SimSystem>();

    public void Simulate(SimSystem simSystem)
    {
        foreach (var inputPin in simSystem.Inputs)
        {
            // inputPin
        }
    }
}