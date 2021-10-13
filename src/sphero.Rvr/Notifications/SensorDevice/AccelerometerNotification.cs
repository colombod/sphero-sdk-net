﻿using System;
using sphero.Rvr.Protocol;

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

            X = rawData[offset..(offset + sizeof(ushort))].ToUshort().ToFloatInRange(-16, 16);
            offset += sizeof(ushort);
            Y = rawData[offset..(offset + sizeof(ushort))].ToUshort().ToFloatInRange(-16, 16);
            offset += sizeof(ushort);
            Z = rawData[offset..(offset + sizeof(ushort))].ToUshort().ToFloatInRange(-16, 16);


            return 3 * sizeof(ushort);
        }

        public float X { get; private set; }
        public float Y { get; private set; }
        public float Z { get; private set; }
    }
}