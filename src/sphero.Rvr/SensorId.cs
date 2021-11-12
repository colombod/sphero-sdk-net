namespace sphero.Rvr;

public enum SensorId : ushort
{
    Quaternion = 0x00,
    Attitude = 0x01,
    Accelerometer = 0x02,
    ColorDetection = 0x03,
    Gyroscope = 0x04,
    Locator = 0x06,
    Velocity = 0x07,
    Speed = 0x08,
    CoreTimeLower = 0x05,
    CoreTimeUpper = 0x09,
    AmbientLight = 0x0A,
    Encoders = 0x0B
}