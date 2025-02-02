namespace LilPuter;

public class SystemSimulator
{
    //A calling of Simulate for each system. Is it BF or DF or topological sorted? Breadth for now!

    private Queue<SimSystem> _toSimulate = new Queue<SimSystem>();

    public void Simulate(SimSystem simSystem)
    {
        _toSimulate.Enqueue(simSystem);
        while (_toSimulate.Count > 0)
        {
            var next = _toSimulate.Dequeue();
            next.Simulate();
            
                
        }
    }
}