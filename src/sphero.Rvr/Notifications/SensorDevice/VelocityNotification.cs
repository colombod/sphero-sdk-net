using System;
using sphero.Rvr.Protocol;
using UnitsNet;

namespace sphero.Rvr.Notifications.SensorDevice;

public class VelocityNotification : Event
{
    public int FromRawData(byte[] rawData, int offset)
    {
        if (rawData == null)
        {
            throw new ArgumentNullException(nameof(rawData));
        }

        X = Speed.FromMetersPerSecond( rawData[offset..(offset + sizeof(uint))].ToUInt().ToFloatInRange(-5, 5));
        offset += sizeof(uint);
        Y = Speed.FromMetersPerSecond(rawData[offset..(offset + sizeof(uint))].ToUInt().ToFloatInRange(-5, 5));

        return 2 * sizeof(uint);
    }

    public Speed X { get; private set; }
    public Speed Y { get; private set; }
}