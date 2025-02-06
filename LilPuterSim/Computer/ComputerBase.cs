namespace LilPuter;

public class ComputerBase
{
    public WireManager WireManager => _wireManager;
    private WireManager _wireManager;
    public ClockManager Clock => _clock;
    private ClockManager _clock;

    public ComputerBase()
    {
        _wireManager = new WireManager(this);
        _clock = new ClockManager(this);
    }
}