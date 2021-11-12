using System;
using sphero.Rvr.Commands.SensorDevice;
using sphero.Rvr.Protocol;

namespace sphero.Rvr.Responses.SensorDevice;

[OriginatingCommand(typeof(GetBotToBotInfraredReadings))]
public class BotToBotInfraredReadings : Response
{
    public BotToBotInfraredReadings(Message message)
    {
        if (message == null)
        {
            throw new ArgumentNullException(nameof(message));
        }

        var raw = message.Data[..sizeof(uint)].ToUInt();

        var frontLeft = (byte)(raw & (uint)InfraredSensorLocation.FrontLeft);
        var frontRight = (byte)((raw & (uint)InfraredSensorLocation.FrontRight) >> 8);
        var backLeft = (byte)((raw & (uint)InfraredSensorLocation.BackLeft) >> 16);
        var backRight = (byte)((raw & (uint)InfraredSensorLocation.BackRight) >> 24);

        FrontLeft = frontLeft >= 255 ? null : frontLeft;
        FrontRight = frontRight >= 255 ? null : frontRight;
        BackLeft = backLeft >= 255 ? null : backLeft;
        BackRight = backRight >= 255 ? null : backRight;
    }

    public byte? FrontLeft { get; }
    public byte? FrontRight { get; }
    public byte? BackLeft { get; }
    public byte? BackRight { get; }
}