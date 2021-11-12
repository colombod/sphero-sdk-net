using System;
using System.Linq;
using sphero.Rvr.Protocol;
using UnitsNet;

namespace sphero.Rvr.Commands.DriveDevice;

[Command(CommandId, DeviceId)]
public class DriveTo : Command
{
    public Length X { get; }
    public Length Y { get; }
    public Angle Yaw { get; }
    public Speed Speed { get; }
    public DriveFlags DriveFlags { get; }

    public DriveTo(Length x, Length y, Angle yaw, Speed speed, DriveFlags flags)
    {
        X = x;
        Y = y;
        Yaw = yaw;
        Speed = speed;
        DriveFlags = flags;
    }

    public const byte CommandId = 0x38;

    public const DeviceIdentifier DeviceId = DeviceIdentifier.Drive;

    public override Message ToMessage()
    {
        var header = new Header(
            commandId: CommandId,
            targetId: 0x02,
            deviceId: DeviceId,
            sourceId: ApiTargetsAndSources.ServiceSource,
            sequence: GetSequenceNumber(),
            flags: Flags.DefaultRequestWithNoResponseFlags);

        var yawBytes = BitConverter.GetBytes((float)Yaw.Degrees);
        var xBytes = BitConverter.GetBytes((float)X.Meters);
        var yBytes = BitConverter.GetBytes((float)Y.Meters);
        var speedBytes = BitConverter.GetBytes((float)Speed.MetersPerSecond);

        if (BitConverter.IsLittleEndian)
        {
            yawBytes = yawBytes.Reverse().ToArray();
            xBytes = xBytes.Reverse().ToArray();
            yBytes = yBytes.Reverse().ToArray();
            speedBytes = speedBytes.Reverse().ToArray();
        }

        var rawData = new byte[17];
        rawData[16] = (byte)DriveFlags;

        Buffer.BlockCopy(yawBytes, 0, rawData, 0, 4);
        Buffer.BlockCopy(xBytes, 0, rawData, 4, 4);
        Buffer.BlockCopy(yBytes, 0, rawData, 8, 4);
        Buffer.BlockCopy(speedBytes, 0, rawData, 12, 4);

        return new Message(header, rawData);
    }

}