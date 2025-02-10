namespace LilPuter;

public class ComputerBase
{
    //meta things
    public WireManager WireManager => _wireManager;
    private WireManager _wireManager;
    public ClockManager Clock => _clock;
    private ClockManager _clock;
    
    //Memory Components. 
    
    //CPU Instruction memory is a wrapper for an 'EEPROM' - the truth table of instruction+microcode offset => control code. 
    public CPUInstructionManager MicrocodeDecoder => _microcodeDecoder;
    private CPUInstructionManager _microcodeDecoder;
    
    //TODO: Rewrite InstructionMemory so it splits the registers into Ins and Op. Operator and Operand on different pins, but internally stored as some x width byte.
    public RAM InstructionMemory =>_instructionMemory;
    private RAM _instructionMemory;

    public RAM Memory => _memory;
    private RAM _memory;
    
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
        _instructionMemory = new RAM(this,"Instruction Memory", width, 256);
        _memory = new RAM(this,"Data Memory", width, 1024);
        _memory.Load.Set(WireSignal.Low);
        _bus = new Bus(this, width);
        _cpu = new CPU(this, width);
        
        Bus.RegisterComponent("MI", true, false,_memory.In, _memory.Load);
        Bus.RegisterComponent("MO", false,true, _memory.Out);
    }
    
}