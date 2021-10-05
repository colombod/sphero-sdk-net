namespace shpero.Rvr
{
    public static class LedBitMaskExtensions
    {
        public static int GetMaxDataSize(this LedBitMask ledBitMask)
        {
            var max = 0;
            if ((ledBitMask & LedBitMask.HeadLightRightRed) != 0) { max++; }
            if ((ledBitMask & LedBitMask.HeadLightRightGreen) != 0) { max++; }
            if ((ledBitMask & LedBitMask.HeadLightRightBlue) != 0) { max++; }
            if ((ledBitMask & LedBitMask.HeadLightLeftRed) != 0) { max++; }
            if ((ledBitMask & LedBitMask.HeadLightLeftGreen) != 0) { max++; }
            if ((ledBitMask & LedBitMask.HeadLightLeftBlue) != 0) { max++; }
            if ((ledBitMask & LedBitMask.StatusIndicationLeftRed) != 0) { max++; }
            if ((ledBitMask & LedBitMask.StatusIndicationLeftGreen) != 0) { max++; }
            if ((ledBitMask & LedBitMask.StatusIndicationLeftBlue) != 0) { max++; }
            if ((ledBitMask & LedBitMask.StatusIndicationRightRed) != 0) { max++; }
            if ((ledBitMask & LedBitMask.StatusIndicationRightGreen) != 0) { max++; }
            if ((ledBitMask & LedBitMask.StatusIndicationRightBlue) != 0) { max++; }
            if ((ledBitMask & LedBitMask.BatteryDoorFrontRed) != 0) { max++; }
            if ((ledBitMask & LedBitMask.BatteryDoorFrontGreen) != 0) { max++; }
            if ((ledBitMask & LedBitMask.BatteryDoorFrontBlue) != 0) { max++; }
            if ((ledBitMask & LedBitMask.BatteryDoorRearRed) != 0) { max++; }
            if ((ledBitMask & LedBitMask.BatteryDoorRearGreen) != 0) { max++; }
            if ((ledBitMask & LedBitMask.BatteryDoorRearBlue) != 0) { max++; }
            if ((ledBitMask & LedBitMask.PowerButtonFrontRed) != 0) { max++; }
            if ((ledBitMask & LedBitMask.PowerButtonFrontGreen) != 0) { max++; }
            if ((ledBitMask & LedBitMask.PowerButtonFrontBlue) != 0) { max++; }
            if ((ledBitMask & LedBitMask.PowerButtonRearRed) != 0) { max++; }
            if ((ledBitMask & LedBitMask.PowerButtonRearGreen) != 0) { max++; }
            if ((ledBitMask & LedBitMask.PowerButtonRearBlue) != 0) { max++; }
            if ((ledBitMask & LedBitMask.BrakeLightLeftRed) != 0) { max++; }
            if ((ledBitMask & LedBitMask.BrakeLightLeftGreen) != 0) { max++; }
            if ((ledBitMask & LedBitMask.BrakeLightLeftBlue) != 0) { max++; }
            if ((ledBitMask & LedBitMask.BrakeLightRightRed) != 0) { max++; }
            if ((ledBitMask & LedBitMask.BrakeLightRightGreen) != 0) { max++; }
            if ((ledBitMask & LedBitMask.BrakeLightRightBlue) != 0) { max++; }
            if ((ledBitMask & LedBitMask.UndercarriageWhite) != 0) { max++; }
            return max;
        }
    }
}