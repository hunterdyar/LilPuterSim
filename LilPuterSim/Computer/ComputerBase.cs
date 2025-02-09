namespace LilPuter;

public class ComputerBase
{
    public WireManager WireManager => _wireManager;
    private WireManager _wireManager;
    public ClockManager Clock => _clock;
    private ClockManager _clock;

    public CPUInstructionManager MicrocodeDecoder => _microcodeDecoder;
    private CPUInstructionManager _microcodeDecoder;
    
    //TODO: Rewrite InstructionMemory so it splits the registers into Ins and Op
    public RAM InstructionMemory =>_instructionMemory;
    private RAM _instructionMemory;
    
    public Bus Bus => _bus;
    private Bus _bus;
    public CPU CPU => _cpu;
    private CPU _cpu;

    public int Width => _width;
    private int _width;
    public ComputerBase()
    {
        int width = 8;
        _wireManager = new WireManager(this);
        _clock = new ClockManager(this);
        _microcodeDecoder = new CPUInstructionManager(this);
        _instructionMemory = new RAM(this, width, 256);
        _bus = new Bus(this, width);
        _cpu = new CPU(this, width);

        _instructionMemory.Out.ConnectTo(_microcodeDecoder.Instruction);
    }
    
}