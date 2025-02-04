namespace LilPuter;

public static class PinUtility
{
	public static PinType GetPinType(Pin pin)
	{
		//if (Enum.IsDefined(typeof(PinType),pin.DataCount))
		return (PinType)pin.DataCount;
		

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
}