namespace LilPuter;

public static class PinUtility
{
	public static PinType GetPinType(Pin pin)
	{
		//if (Enum.IsDefined(typeof(PinType),pin.DataCount))
		return (PinType)pin.Width;
		//throw new Exception("Invalid Pin Type. It's just ... data?");
	}

	public static byte[] IntToByteArray(int i, int width)
	{
		byte[] data = new byte[width];
		for (int j = 0; j < width; j++)
		{
			data[j] = (byte)((i & (1 << j)) >> j);
		}

		return data;
	}

	public static int ByteArrayToInt(byte[] value)
	{
		//We need to ensure that the value is not floating.
		int result = 0;
		for (int i = 0; i < value.Length; i++)
		{
			result |= value[i] << i;
		}

		return result;
	}

	public static byte[] Invert(byte[] value)
	{
		for (int i = 0; i < value.Length; i++)
		{
			value[i] = (byte)(value[i] == 0 ? (byte)1 : (byte)0);
		}

		return value;
	}

	public static int SizeToRequiredBits(int size)
	{
		int bits = 0;
		while (size > 0)
		{
			size >>= 1;
			bits++;
		}

		return bits;
	}
}