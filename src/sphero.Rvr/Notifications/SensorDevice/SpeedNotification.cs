using System;
using sphero.Rvr.Protocol;
using UnitsNet;

namespace sphero.Rvr.Notifications.SensorDevice
{
    public class SpeedNotification : Event
    {
        public int FromRawData(byte[] rawData, int offset)
        {
            if (rawData == null)
            {
                throw new ArgumentNullException(nameof(rawData));
            }

            Speed = Speed.FromMetersPerSecond(rawData[offset..(offset + sizeof(uint))].ToUInt().ToFloatInRange(0, 5));
            return 1 * sizeof(uint);
        }

        public Speed Speed { get; private set; }
    }
}