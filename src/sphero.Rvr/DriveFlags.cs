using System;

namespace sphero.Rvr
{
    [Flags]
    public enum DriveFlags : byte
    {
        NoFlags = 0x00,
        DriveReverse = 0x01,
        Boost = 0x02,
        FastTurn = 0x04,
        LeftDirection = 0x08,
        RightDirection = 0x10,
        EnableDrift = 0x20
    }
}