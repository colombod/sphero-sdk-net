using System;
using sphero.Rvr.Protocol;

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

            Pitch = rawData[offset..(offset + sizeof(ushort))].ToUshort().ToFloatInRange(-180, 180);
            offset += sizeof(ushort);
            Roll = rawData[offset..(offset + sizeof(ushort))].ToUshort().ToFloatInRange(-90, 90);
            offset += sizeof(ushort);
            Yaw = rawData[offset..(offset + sizeof(ushort))].ToUshort().ToFloatInRange(-180, 180);
            

            return 3 * sizeof(ushort);
        }

        public float Pitch { get; private set; }
        public float Roll { get; private set; }
        public float Yaw { get; private set; }
    }
}