using sphero.Rvr.Protocol;

using UnitsNet;

namespace sphero.Rvr.Commands.DriveDevice
{
    [Command(CommandId, DeviceId)]
    public class DriveWithHeading : Command
    {
        private readonly byte _motorSpeed;
        private readonly Angle _heading;
        private readonly DriveFlags _flags;

        public const byte CommandId = 0x07;

        public const DeviceIdentifier DeviceId = DeviceIdentifier.Drive;

        public DriveWithHeading(byte motorSpeed, UnitsNet.Angle heading, DriveFlags flags)
        {
            _motorSpeed = motorSpeed;
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



            var value = (ushort)_heading.Degrees;
            var rawData = new[] { _motorSpeed, (byte)((value >> 8) & 0xFF), (byte)(value & 0xFF), (byte)_flags };

            return new Message(header, rawData);
        }
    }
}