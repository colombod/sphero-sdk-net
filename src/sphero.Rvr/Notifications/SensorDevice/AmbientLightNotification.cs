﻿using System;
using sphero.Rvr.Protocol;
using UnitsNet;

namespace sphero.Rvr.Notifications.SensorDevice;

public class AmbientLightNotification : Event
{
    public int FromRawData(byte[] rawData, int offset)
    {
        if (rawData == null)
        {
            throw new ArgumentNullException(nameof(rawData));
        }

        Light = Illuminance.FromLux( rawData[offset..(offset + sizeof(ushort))].ToUshort().ToFloatInRange(0, 120000));
        return 1 * sizeof(ushort);
    }

    public Illuminance Light { get; private set; }
}