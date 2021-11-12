using System;
using sphero.Rvr.Protocol;
using UnitsNet;

namespace sphero.Rvr.Notifications.SensorDevice;

public class LocatorNotification : Event
{
    public int FromRawData(byte[] rawData, int offset)
    {
        if (rawData == null)
        {
            throw new ArgumentNullException(nameof(rawData));
        }
            
        X = Length.FromMeters( rawData[offset..(offset + sizeof(uint))].ToUInt().ToFloatInRange(-16000, 16000));
        offset += sizeof(uint);
        Y = Length.FromMeters(rawData[offset..(offset + sizeof(uint))].ToUInt().ToFloatInRange(-16000, 16000));

        return 2 * sizeof(uint);
    }
        
    public Length X { get; private set; }
    public Length Y { get; private set; }
}