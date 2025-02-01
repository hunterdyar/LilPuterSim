namespace LilPuter;

/// <summary>
/// A system is some number of inputs and outputs that should be simulated as an isolated element.
/// </summary>
public abstract class SimSystem
{
    public Pin[] Inputs
    {
        get;
        protected set;
    }
    public Pin[] Outputs { get; protected set; }

    private bool _needsSimulation = false;

    public void SetNeedsSimulation()
    {
        _needsSimulation = true;
    }
    /// <summary>
    /// Update Outputs (presumably, considering inputs)
    /// </summary>
    public virtual void Simulate()
    {
        _needsSimulation = true;
    }

}