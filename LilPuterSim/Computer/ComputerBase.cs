namespace LilPuter
{
    public class ComputerBase
    {
        //meta things
        public WireManager WireManager => _wireManager;
        private WireManager _wireManager;
        public ClockManager Clock => _clock;
        private ClockManager _clock;
        private NotGate _invertHaltGate;
        //Memory Components. 
    
        //CPU Instruction memory is a wrapper for an 'EEPROM' - the truth table of instruction+microcode offset => control code. 
    
        //TODO: Rewrite InstructionMemory so it splits the registers into Ins and Op. Operator and Operand on different pins, but internally stored as some x width byte.
    
        public CPU CPU => _cpu;
        private CPU _cpu;

        public int Width => _width;
        private int _width;
        public ComputerBase()
        {
            int width = 8;
            _wireManager = new WireManager(this);
            _clock = new ClockManager(this);
            _cpu = new CPU(this, width);
            _invertHaltGate = new NotGate(_wireManager);
            _cpu.HaltLine.ConnectTo(_invertHaltGate.A);
            _invertHaltGate.Out.ConnectTo(_clock.EnableClock);
        }

        public void ExecuteOneInstruction()
        {
            if (_cpu.MicrocodeDecoder.Counter.Out.Value == 0)
            {
                _clock.Cycle();
            }

            while (_cpu.MicrocodeDecoder.Counter.Out.Value != 0)
            {
                _clock.Cycle();
            }
        }
        
    }
}