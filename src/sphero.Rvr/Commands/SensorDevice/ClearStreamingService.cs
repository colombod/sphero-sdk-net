using sphero.Rvr.Protocol;

namespace sphero.Rvr.Commands.SensorDevice
{
    [Command(CommandId, DeviceId)]
    public class ClearStreamingService : Command
    {
        private readonly byte _targetId;

        public const byte CommandId = 0x3C;

        public const DeviceIdentifier DeviceId = DeviceIdentifier.Sensor;

        public ClearStreamingService(byte targetId)
        {
            _targetId = targetId;
        }

        public override Message ToMessage()
        {
            var header = new Header(
                commandId: CommandId,
                targetId: _targetId,
                deviceId: DeviceId,
                sourceId: ApiTargetsAndSources.ServiceSource,
                sequence: GetSequenceNumber(),
                flags: Flags.DefaultRequestWithNoResponseFlags);
            return new Message(header);
        }
    }
}