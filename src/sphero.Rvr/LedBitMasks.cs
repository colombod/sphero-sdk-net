using System;

namespace sphero.Rvr
{
    [Flags]
    public enum LedBitMask
    {
        HeadLightRightRed = 0x00000001,
        HeadLightRightGreen = 0x00000002,
        HeadLightRightBlue = 0x00000004,
        HeadLightLeftRed = 0x00000008,
        HeadLightLeftGreen = 0x00000010,
        HeadLightLeftBlue = 0x00000020,
        StatusIndicationLeftRed = 0x00000040,
        StatusIndicationLeftGreen = 0x00000080,
        StatusIndicationLeftBlue = 0x00000100,
        StatusIndicationRightRed = 0x00000200,
        StatusIndicationRightGreen = 0x00000400,
        StatusIndicationRightBlue = 0x00000800,
        BatteryDoorFrontRed = 0x00001000,
        BatteryDoorFrontGreen = 0x00002000,
        BatteryDoorFrontBlue = 0x00004000,
        BatteryDoorRearRed = 0x00008000,
        BatteryDoorRearGreen = 0x00010000,
        BatteryDoorRearBlue = 0x00020000,
        PowerButtonFrontRed = 0x00040000,
        PowerButtonFrontGreen = 0x00080000,
        PowerButtonFrontBlue = 0x00100000,
        PowerButtonRearRed = 0x00200000,
        PowerButtonRearGreen = 0x00400000,
        PowerButtonRearBlue = 0x00800000,
        BrakeLightLeftRed = 0x01000000,
        BrakeLightLeftGreen = 0x02000000,
        BrakeLightLeftBlue = 0x04000000,
        BrakeLightRightRed = 0x08000000,
        BrakeLightRightGreen = 0x10000000,
        BrakeLightRightBlue = 0x20000000,
        UndercarriageWhite = 0x40000000,

        HeadLightRight = HeadLightRightRed | HeadLightRightGreen | HeadLightRightBlue,
        HeadLightLeft = HeadLightLeftRed | HeadLightLeftGreen | HeadLightLeftBlue,
        StatusIndicationLeft = StatusIndicationLeftRed | StatusIndicationLeftGreen | StatusIndicationLeftBlue,
        StatusIndicationRight = StatusIndicationRightRed | StatusIndicationRightGreen | StatusIndicationRightBlue,
        BatteryDoorFront = BatteryDoorFrontRed | BatteryDoorFrontGreen | BatteryDoorFrontBlue,
        BatteryDoorRear = BatteryDoorRearRed | BatteryDoorRearGreen | BatteryDoorRearBlue,
        PowerButtonFront = PowerButtonFrontRed | PowerButtonFrontGreen | PowerButtonFrontBlue,
        PowerButtonRear = PowerButtonRearRed | PowerButtonRearGreen | PowerButtonRearBlue,
        BrakeLightLeft = BrakeLightLeftRed | BrakeLightLeftGreen | BrakeLightLeftBlue,
        BrakeLightRight = BrakeLightRightRed | BrakeLightRightGreen | BrakeLightRightBlue,
        All = HeadLightRight | HeadLightLeft | StatusIndicationLeft | StatusIndicationRight | BatteryDoorFront | BatteryDoorRear | PowerButtonFront | PowerButtonRear | BrakeLightLeft | BrakeLightRight
    }
}