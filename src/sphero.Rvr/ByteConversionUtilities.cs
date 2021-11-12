using System;
using System.Linq;
using System.Text;

namespace sphero.Rvr;

internal static class ByteConversionUtilities
{
    public static byte NibblesToByte(int[] nibbles)
    {
        var value = 0;

        if (nibbles is null)
        {
            return (byte)value;
        }

        for (var i = nibbles.Length - 1; i >= 0; i--)
        {
            value = (value * 16) + nibbles[i];
        }

        return (byte)value;
    }

    public static Version ToVersion(this byte[] rawBytes)
    {
        if (rawBytes.Length < 3 * sizeof(ushort))
        {
            throw new InvalidOperationException("not enough data");
        }

        var major = rawBytes[..2].ToUshort();

        var minor = rawBytes[2..4].ToUshort();

        var revision = rawBytes[4..6].ToUshort();

        return new Version(major, minor, 0, revision);
    }

    public static ushort ToUshort(this byte[] rawBytes)
    {
        if (rawBytes.Length < 1 * sizeof(ushort))
        {
            throw new InvalidOperationException("not enough data");
        }

        return BitConverter.ToUInt16(rawBytes.Reverse().ToArray(), 0);
    }

    public static float ToFloatInRange(this ushort value, float min, float max)
    {
        var normalized = ((float)value) / ushort.MaxValue;
        return ((max - min) * normalized) + min;
    }

    public static float ToFloatInRange(this byte value, float min, float max)
    {
        var normalized = ((float)value) / byte.MaxValue;
        return ((max - min) * normalized) + min;
    }

    public static float ToFloatInRange(this uint value, float min, float max)
    {
        var normalized = ((float)value) / uint.MaxValue;
        return ((max - min) * normalized) + min;
    }

    public static uint ToUInt(this byte[] rawBytes)
    {
        if (rawBytes.Length < 1 * sizeof(uint))
        {
            throw new InvalidOperationException("not enough data");
        }

        return BitConverter.ToUInt32(rawBytes.Reverse().ToArray(), 0);
    }

    public static long ToLong(this byte[] rawBytes)
    {
        if (rawBytes.Length < 1 * sizeof(long))
        {
            throw new InvalidOperationException("not enough data");
        }

        return BitConverter.ToInt64(rawBytes.Reverse().ToArray(), 0);
    }

    public static float ToFloat(this byte[] rawBytes)
    {
        if (rawBytes.Length < 1 * sizeof(float))
        {
            throw new InvalidOperationException("not enough data");
        }

        return BitConverter.ToSingle(rawBytes.Reverse().ToArray(), 0);
    }

    public static string ToStringFromNullTerminated(this byte[] rawBytes, bool terminatorOptional = false)
    {
        var nullPos = Array.IndexOf(rawBytes, (byte)0x00);
        if (nullPos >= 0)
        {
            return Encoding.ASCII.GetString(rawBytes, 0, nullPos);
        }
        if (!terminatorOptional)
        {
            throw new InvalidOperationException("no terminating null found in string");
        }
        return Encoding.ASCII.GetString(rawBytes);
    }
}