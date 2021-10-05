using System;

namespace shpero.Rvr
{
    [Flags]
    public enum GyroMaxFlag : byte
    {
        MaxPlusX = 0x01,
        MaxMinusX = 0x02,
        MaxPlusY = 0x04,
        MaxMinusY = 0x08,
        MaxPlusZ = 0x10,
        MaxMinusZ = 0x20
    }
}