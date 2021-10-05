using shpero.Rvr.Protocol;
using UnitsNet;

namespace shpero.Rvr.Commands.DriveDevice
{
    [Command(CommandId, DeviceId)]
    public class DriveWithHeading : Command
    {
        private readonly byte _speed;
        private readonly Angle _heading;
        private readonly DriveFlags _flags;

        public const byte CommandId = 0x07;

        public const DeviceIdentifier DeviceId = DeviceIdentifier.Drive;

        public DriveWithHeading(byte speed, UnitsNet.Angle heading, DriveFlags flags)
        {
            _speed = speed;
            _heading = heading;
            _flags = flags;
        }

        public override Message ToMessage()
        {
            var header = new Header(
                commandId: CommandId,
                targetId: 0x02,
                deviceId: DeviceId,
                sourceId: ApiTargetsAndSources.ServiceSource,
                sequence: GetSequenceNumber(),
                flags: Flags.DefaultRequestWithNoResponseFlags);

            var rawData = new byte[]{_speed,0,0,(byte)_flags};
            
            var value = (ushort)_heading.Degrees;
            for (var i = 2; i >= 1; i--)
            {
                var byteValue = value & 0xFF;
                rawData[i] = (byte)byteValue;
                value = (ushort)((value - byteValue) / 256);
            }

            return new Message(header, rawData);
        }
    }
}