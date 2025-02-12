namespace LilPuter.Clock
{
	/// <summary>
	/// Clock Input receives the global clock pulse.
	/// </summary>
	public class ClockPin : ISystem
	{
		public bool Enabled { get; private set; }
		public string Name { get; }
	
		public Action OnTick;
		public Action OnTock;
		private Pin[] _connectees = [];
		private readonly ClockManager _manager;

		public void ConnectPin(Pin pin)
		{
			Array.Resize(ref _connectees, _connectees.Length + 1);
			_connectees[^1] = pin;
		}

		public void DisconnectPin()
		{
			throw new NotImplementedException("Disconnecting pins is not yet supported.");
		}

		public void SetEnabled(bool enabled)
		{
			Enabled = enabled;
		}
	 
		public ClockPin(ClockManager manager)
		{
			Name = "Clock In";
			Enabled = true;
			_manager = manager;
			_manager.RegisterClockPin(this);
		}

		public ClockPin(ClockManager manager, string name)
		{
			Name = name;
			Enabled = true;
			_manager = manager;
			_manager.RegisterClockPin(this);
		}
		public void TickSilent()
		{
			if (Enabled)
			{
				OnTick?.Invoke();
				//set all connectees high.
				for (int i = 0; i < _connectees.Length; i++)
				{
					_connectees[i].Set(WireSignal.High);
				}
			}
		}

		public void TockSilent()
		{
			if (Enabled)
			{
				OnTock?.Invoke();
				//set all connectees low.
				//set all connectees high.
				for (int i = 0; i < _connectees.Length; i++)
				{
					_connectees[i].Set(WireSignal.High);
				}
			}
		}
	}
}