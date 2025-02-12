namespace LilPuter
{
	public class BusConnection
	{
		public string Name;
		public bool Enabled { get; private set; }
		public bool SetFromBus;
		public bool SetToBus;
		public Pin? Pin;
	
		/// <summary>
		/// Separate from the data, is this high when the bus is enabled.
		/// </summary>
		public Pin? LoadPin;
		public bool InvertedLoad;
		public int Index;

		public BusConnection()
		{
		
		}
		/// <summary>
		/// Sets enabled and updates the given load pin.
		/// </summary>
		public void SetEnabled(bool enabled)
		{
			Enabled = enabled;
			SetLoadPin(enabled);
		}
		/// <summary>
		/// Sets or Unsets the load pin, inverting if neccesary and ignoring if null.
		/// </summary>
		private void SetLoadPin(bool enabled)
		{
			if (LoadPin != null)
			{
				if (!InvertedLoad)
				{
					LoadPin.SetAndImpulse(enabled? WireSignal.High : WireSignal.Low);
				}
				else
				{
					LoadPin.SetAndImpulse(enabled? WireSignal.Low : WireSignal.High);
				}
			}
		}

		public override string ToString()
		{
			return $"BusConn: {Name} - {Enabled}";
		}
	}
}