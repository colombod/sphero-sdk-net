namespace sphero.Rvr
{
    public enum BatteryVoltageReadings : byte
    {
        CalibratedAndFiltered = 0x0,
        CalibratedAndUnfiltered = 0x1,
        UncalibratedAndUnfiltered = 0x2
    }
}