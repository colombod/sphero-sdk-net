namespace sphero.Rvr
{
    public enum Led : uint
    {
        HeadLightRight = LedBitMask.HeadLightRightRed | LedBitMask.HeadLightRightGreen | LedBitMask.HeadLightRightBlue,
        HeadLightLeft = LedBitMask.HeadLightLeftRed | LedBitMask.HeadLightLeftGreen | LedBitMask.HeadLightLeftBlue,
        StatusIndicationLeft = LedBitMask.StatusIndicationLeftRed | LedBitMask.StatusIndicationLeftGreen | LedBitMask.StatusIndicationLeftBlue,
        StatusIndicationRight = LedBitMask.StatusIndicationRightRed | LedBitMask.StatusIndicationRightGreen | LedBitMask.StatusIndicationRightBlue,
        BatteryDoorFront = LedBitMask.BatteryDoorFrontRed | LedBitMask.BatteryDoorFrontGreen | LedBitMask.BatteryDoorFrontBlue,
        BatteryDoorRear = LedBitMask.BatteryDoorRearRed | LedBitMask.BatteryDoorRearGreen | LedBitMask.BatteryDoorRearBlue,
        PowerButtonFront = LedBitMask.PowerButtonFrontRed | LedBitMask.PowerButtonFrontGreen | LedBitMask.PowerButtonFrontBlue,
        PowerButtonRear = LedBitMask.PowerButtonRearRed | LedBitMask.PowerButtonRearGreen | LedBitMask.PowerButtonRearBlue,
        BrakeLightLeft = LedBitMask.BrakeLightLeftRed | LedBitMask.BrakeLightLeftGreen | LedBitMask.BrakeLightLeftBlue,
        BrakeLightRight = LedBitMask.BrakeLightRightRed | LedBitMask.BrakeLightRightGreen | LedBitMask.BrakeLightRightBlue,
        UndercarriageWhite = LedBitMask.UndercarriageWhite
    }
}