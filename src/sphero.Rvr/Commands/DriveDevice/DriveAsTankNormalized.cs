using System;
using System.Runtime.InteropServices;
using sphero.Rvr.Protocol;

namespace sphero.Rvr.Commands.DriveDevice;

[Command(CommandId, DeviceId)]
public class DriveAsTankNormalized : Command
{
    public sbyte LeftThreadSpeed { get; }
    public sbyte RightThreadSpeed { get; }

    public DriveAsTankNormalized(sbyte leftThreadSpeed, sbyte rightThreadSpeed)
    {
        LeftThreadSpeed = leftThreadSpeed;
        RightThreadSpeed = rightThreadSpeed;
    }

    public const byte CommandId = 0x33;

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

        sbyte[] signed = { LeftThreadSpeed, RightThreadSpeed };
        ReadOnlySpan<sbyte> sbytesBuffer = signed;
        ReadOnlySpan<byte> bytesBuffer = MemoryMarshal.Cast<sbyte, byte>(sbytesBuffer);
        var rawData = bytesBuffer.ToArray();

        return new Message(header, rawData);
    }
}