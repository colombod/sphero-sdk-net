using System;
using System.Linq;
using sphero.Rvr.Protocol;
using UnitsNet;

namespace sphero.Rvr.Commands.DriveDevice
{
    [Command(CommandId, DeviceId)]
    public class DriveAsTank : Command
    {
        public Speed LeftThreadSpeed { get; }
        public Speed RightThreadSpeed { get; }

        public DriveAsTank(Speed leftThreadSpeed, Speed rightThreadSpeed)
        {
            LeftThreadSpeed = leftThreadSpeed;
            RightThreadSpeed = rightThreadSpeed;
        }

        public const byte CommandId = 0x32;

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

            var leftBytes = BitConverter.GetBytes((float)LeftThreadSpeed.MetersPerSecond);
            var rightBytes = BitConverter.GetBytes((float)RightThreadSpeed.MetersPerSecond);

            if (BitConverter.IsLittleEndian)
            {
                leftBytes = leftBytes.Reverse().ToArray();
                rightBytes = rightBytes.Reverse().ToArray();
            }
            
            var rawData = new byte[8];

            Buffer.BlockCopy(leftBytes,0,rawData,0,4);
            Buffer.BlockCopy(rightBytes, 0, rawData, 4, 4);

            return new Message(header, rawData);
        }
    }
}