using System;
using sphero.Rvr.Protocol;
using UnitsNet;

namespace sphero.Rvr.Notifications.SensorDevice
{
    public class AccelerometerNotification : Event
    {
        public int FromRawData(byte[] rawData, int offset)
        {
            if (rawData == null)
            {
                throw new ArgumentNullException(nameof(rawData));
            }

            X = Acceleration.FromMetersPerSecondSquared( rawData[offset..(offset + sizeof(ushort))].ToUshort().ToFloatInRange(-16, 16));
            offset += sizeof(ushort);
            Y = Acceleration.FromMetersPerSecondSquared(rawData[offset..(offset + sizeof(ushort))].ToUshort().ToFloatInRange(-16, 16));
            offset += sizeof(ushort);
            Z = Acceleration.FromMetersPerSecondSquared(rawData[offset..(offset + sizeof(ushort))].ToUshort().ToFloatInRange(-16, 16));


            return 3 * sizeof(ushort);
        }

        public Acceleration X { get; private set; }
        public Acceleration Y { get; private set; }
        public Acceleration Z { get; private set; }
    }
}