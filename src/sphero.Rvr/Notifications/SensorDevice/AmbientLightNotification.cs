using System;
using sphero.Rvr.Protocol;

namespace sphero.Rvr.Notifications.SensorDevice
{
    public class AmbientLightNotification : Event
    {
        public int FromRawData(byte[] rawData, int offset)
        {
            if (rawData == null)
            {
                throw new ArgumentNullException(nameof(rawData));
            }

            Light = rawData[offset..(offset + sizeof(ushort))].ToUshort().ToFloatInRange(0, 120000);
            return 1 * sizeof(ushort);
        }

        public float Light { get; private set; }
    }
}