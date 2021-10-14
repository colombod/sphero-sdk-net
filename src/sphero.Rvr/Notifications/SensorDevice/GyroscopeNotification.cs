using System;
using sphero.Rvr.Protocol;
using UnitsNet;

namespace sphero.Rvr.Notifications.SensorDevice
{
    public class GyroscopeNotification : Event
    {
        public int FromRawData(byte[] rawData, int offset)
        {
            if (rawData == null)
            {
                throw new ArgumentNullException(nameof(rawData));
            }

            X = RotationalSpeed.FromDegreesPerSecond( rawData[offset..(offset + sizeof(ushort))].ToUshort().ToFloatInRange(-2000, 2000));
            offset += sizeof(ushort);
            Y = RotationalSpeed.FromDegreesPerSecond(rawData[offset..(offset + sizeof(ushort))].ToUshort().ToFloatInRange(-2000, 2000));
            offset += sizeof(ushort);
            Z = RotationalSpeed.FromDegreesPerSecond(rawData[offset..(offset + sizeof(ushort))].ToUshort().ToFloatInRange(-2000, 2000));


            return 3 * sizeof(ushort);
        }

        public RotationalSpeed X { get; private set; }
        public RotationalSpeed Y { get; private set; }
        public RotationalSpeed Z { get; private set; }
    }
}