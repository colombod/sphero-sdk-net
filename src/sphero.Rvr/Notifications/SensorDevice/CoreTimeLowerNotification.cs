using System;
using sphero.Rvr.Protocol;

namespace sphero.Rvr.Notifications.SensorDevice
{
    public class CoreTimeLowerNotification : Event
    {
        public int FromRawData(byte[] rawData, int offset)
        {
            if (rawData == null)
            {
                throw new ArgumentNullException(nameof(rawData));
            }

            Time = rawData[offset..(offset + sizeof(uint))].ToUInt();
            return 1 * sizeof(uint);
        }

        public uint Time { get; private set; }
    }
}