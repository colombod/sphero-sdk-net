using System;
using System.Linq;
using sphero.Rvr.Protocol;
using UnitsNet;

namespace sphero.Rvr.Commands.DriveDevice
{
    [Command(CommandId, DeviceId)]
    public class DriveWithYaw : Command
    {
        public Angle Yaw { get; }
        public Speed Speed { get; }

        public DriveWithYaw(Angle yaw, Speed speed)
        {
            Yaw = yaw;
            Speed = speed;
        }

        public const byte CommandId = 0x36;

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
            var speedBytes = BitConverter.GetBytes((float)Speed.MetersPerSecond);

            if (BitConverter.IsLittleEndian)
            {
                yawBytes = yawBytes.Reverse().ToArray();
                speedBytes = speedBytes.Reverse().ToArray();
            }

            var rawData = new byte[8];

            Buffer.BlockCopy(yawBytes, 0, rawData, 0, 4);
            Buffer.BlockCopy(speedBytes, 0, rawData, 4, 4);

            return new Message(header, rawData);
        }
    }
}