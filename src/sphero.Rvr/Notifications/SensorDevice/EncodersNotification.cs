using System;
using sphero.Rvr.Protocol;

namespace sphero.Rvr.Notifications.SensorDevice;

public class EncodersNotification : Event
{
    public int FromRawData(byte[] rawData, int offset)
    {
        if (rawData == null)
        {
            throw new ArgumentNullException(nameof(rawData));
        }

        LeftTicks = rawData[offset..(offset + sizeof(uint))].ToUInt();
        offset += sizeof(uint);
        RightTicks = rawData[offset..(offset + sizeof(uint))].ToUInt();
        return 2 * sizeof(uint);
    }

    public uint LeftTicks { get; private set; }
    public uint RightTicks { get; private set; }
}