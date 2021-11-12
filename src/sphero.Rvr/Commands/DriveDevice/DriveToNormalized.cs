using System;
using System.Runtime.InteropServices;
using sphero.Rvr.Protocol;

namespace sphero.Rvr.Commands.DriveDevice;

[Command(CommandId, DeviceId)]
public class DriveToNormalized : Command
{
    public sbyte X { get; }
    public sbyte Y { get; }
    public sbyte Yaw { get; }
    public sbyte Speed { get; }
    public DriveFlags DriveFlags { get; }

    public DriveToNormalized(sbyte x, sbyte y, sbyte yaw, sbyte speed, DriveFlags flags)
    {
        X = x;
        Y = y;
        Yaw = yaw;
        Speed = speed;
        DriveFlags = flags;
    }

    public const byte CommandId = 0x39;

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

 

        sbyte[] signed = { Yaw, X,Y,Speed, 0 };
        ReadOnlySpan<sbyte> sbytesBuffer = signed;
        ReadOnlySpan<byte> bytesBuffer = MemoryMarshal.Cast<sbyte, byte>(sbytesBuffer);
        var rawData = bytesBuffer.ToArray();

        rawData[4] = (byte)DriveFlags;
            

        return new Message(header, rawData);
    }

}