namespace LilPuter
{
	public static class WireUtility
	{
		public static bool IsHigh(this byte[] data)
		{
			return ByteToSignal(data) == WireSignal.High;
		}

		public static bool IsLow(this byte[] data)
		{
			return ByteToSignal(data) == WireSignal.Low;
		}
		public static WireSignal ByteToSignal(byte[] data)
		{
			if (data.Length != 1)
			{
				throw new ArgumentException("Data must be a single byte");
			}

			return (WireSignal)data[0];
		}

		public static WireSignal Invert(int data)
		{
			//todo: this needs to know the width. (note: currently masked out by the pin, instead of here. Move that to here because we want our system to be blind to the underlying int data type.
			return (WireSignal)~data;
		}

		public static WireSignal Invert(WireSignal input)
		{
			switch (input)
			{
				case WireSignal.Low:
					return WireSignal.High;
				case WireSignal.High:
					return WireSignal.Low;
				case WireSignal.Floating:
					return WireSignal.Floating;
			}

			throw new ArgumentException("Invalid Input");
		}
	}
}