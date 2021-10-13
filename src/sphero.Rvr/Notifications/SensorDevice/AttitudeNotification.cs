using System;

using sphero.Rvr.Protocol;

using UnitsNet;

namespace sphero.Rvr.Notifications.SensorDevice
{
    public class AttitudeNotification : Event
    {
        public int FromRawData(byte[] rawData, int offset)
        {
            if (rawData == null)
            {
                throw new ArgumentNullException(nameof(rawData));
            }

            Pitch = Angle.FromDegrees(rawData[offset..(offset + sizeof(ushort))].ToUshort().ToFloatInRange(-180, 180));
            offset += sizeof(ushort);
            Roll = Angle.FromDegrees(rawData[offset..(offset + sizeof(ushort))].ToUshort().ToFloatInRange(-90, 90));
            offset += sizeof(ushort);
            Yaw = Angle.FromDegrees(rawData[offset..(offset + sizeof(ushort))].ToUshort().ToFloatInRange(-180, 180));


            return 3 * sizeof(ushort);
        }

        public Angle Pitch { get; private set; }
        public Angle Roll { get; private set; }
        public Angle Yaw { get; private set; }
    }
}