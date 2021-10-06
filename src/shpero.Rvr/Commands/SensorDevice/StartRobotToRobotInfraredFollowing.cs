using System;
using shpero.Rvr.Protocol;

namespace shpero.Rvr.Commands.SensorDevice
{
    [Command(CommandId, DeviceId)]
    public class StartRobotToRobotInfraredFollowing : Command
    {
        private readonly byte _nearCode;
        private readonly byte _farCode;
        public const byte CommandId = 0x28;

        public const DeviceIdentifier DeviceId = DeviceIdentifier.Sensor;

        public StartRobotToRobotInfraredFollowing(byte nearCode, byte farCode)
        {
            if (nearCode > 7)
            {
                throw new ArgumentOutOfRangeException(nameof(nearCode), $"{nameof(nearCode)} should be between 0 and 7.");
            }

            if (farCode > 7)
            {
                throw new ArgumentOutOfRangeException(nameof(farCode), $"{nameof(farCode)} should be between 0 and 7.");
            }

            _nearCode = nearCode;
            _farCode = farCode;
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
            return new Message(header, new[] { _farCode, _nearCode });
        }
    }
}