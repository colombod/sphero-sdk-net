using System;
using System.Runtime.InteropServices;
using sphero.Rvr.Protocol;

namespace sphero.Rvr.Commands.DriveDevice
{
    [Command(CommandId, DeviceId)]
    public class DriveWithYawNormalized : Command
    {
        public sbyte Yaw { get; }
        public sbyte Speed { get; }

        public DriveWithYawNormalized(sbyte yaw, sbyte speed)
        {
            Yaw = yaw;
            Speed = speed;
        }

        public const byte CommandId = 0x37;

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



            sbyte[] signed = { Yaw, Speed };
            ReadOnlySpan<sbyte> sbytesBuffer = signed;
            ReadOnlySpan<byte> bytesBuffer = MemoryMarshal.Cast<sbyte, byte>(sbytesBuffer);
            var rawData = bytesBuffer.ToArray();

            return new Message(header, rawData);
        }
    }
}