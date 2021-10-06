using shpero.Rvr.Protocol;

namespace shpero.Rvr.Commands.SensorDevice
{
    [Command(CommandId, DeviceId)]
    public class EnableColorDetectionNotifications : Command
    {
        private readonly bool _enable;

        public const byte CommandId = 0x35;

        public const DeviceIdentifier DeviceId = DeviceIdentifier.Sensor;

        public EnableColorDetectionNotifications(bool enable)
        {
            _enable = enable;
        }

        public override Message ToMessage()
        {
            var header = new Header(
                commandId: CommandId,
                targetId: 0x01,
                deviceId: DeviceId,
                sourceId: ApiTargetsAndSources.ServiceSource,
                sequence: GetSequenceNumber(),
                flags: Flags.DefaultRequestWithNoResponseFlags);
            return new Message(header, _enable ? new byte[] { 0x01 } : new byte[] { 0x00 });
        }
    }
}