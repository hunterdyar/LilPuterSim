namespace LilPuter;

public static class WireUtility
{
	public static WireSignal ByteToSignal(byte[] data)
	{
		if (data.Length != 1)
		{
			throw new ArgumentException("Data must be a single byte");
		}

		return (WireSignal)data[0];
	}

	public static WireSignal Invert(byte[] data)
	{
		return Invert(ByteToSignal(data));
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