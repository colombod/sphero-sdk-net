using System;
using sphero.Rvr.Protocol;

namespace sphero.Rvr.Notifications.SensorDevice;

public class QuaternionNotification : Event
{
    public QuaternionNotification()
    {
            
    }

    public int FromRawData(byte[] rawData, int offset)
    {
        if (rawData == null)
        {
            throw new ArgumentNullException(nameof(rawData));
        }
            
        W = rawData[offset..(offset + sizeof(uint))].ToUInt().ToFloatInRange(-1,1);
        offset += sizeof(uint);
        X = rawData[offset..(offset + sizeof(uint))].ToUInt().ToFloatInRange(-1, 1);
        offset += sizeof(uint);
        Y = rawData[offset..(offset + sizeof(uint))].ToUInt().ToFloatInRange(-1, 1);
        offset += sizeof(uint);
        Z = rawData[offset..(offset + sizeof(uint))].ToUInt().ToFloatInRange(-1, 1);

        return 4 * sizeof(uint);
    }

    public float  W { get; private set; }
    public float X { get; private set; }
    public float Y { get; private set; }
    public float Z { get; private set; }
}