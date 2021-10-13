using System;
using sphero.Rvr.Protocol;

namespace sphero.Rvr.Notifications.SensorDevice
{
    public class LocatorNotification : Event
    {
        public int FromRawData(byte[] rawData, int offset)
        {
            if (rawData == null)
            {
                throw new ArgumentNullException(nameof(rawData));
            }
            
            X = rawData[offset..(offset + sizeof(uint))].ToUInt().ToFloatInRange(-16000, 16000);
            offset += sizeof(uint);
            Y = rawData[offset..(offset + sizeof(uint))].ToUInt().ToFloatInRange(-16000, 16000);

            return 2 * sizeof(uint);
        }
        
        public float X { get; private set; }
        public float Y { get; private set; }
    }
}