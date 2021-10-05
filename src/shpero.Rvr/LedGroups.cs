using System;

namespace shpero.Rvr
{
    [Flags]
    public enum LedGroup
    {
        StatusIndicationLeft = 0,
        StatusIndicationRight = 1,
        HeadLightLeft = 2,
        HeadLightRight = 3,
        BatteryDoorFront = 4,
        BatteryDoorRear = 5,
        PowerButtonFront =6,
        PowerButtonRear = 7,
        BrakeLightLeft = 8,
        BrakeLightRight = 9,
        UndercarriageWhite = 10 
    }
}