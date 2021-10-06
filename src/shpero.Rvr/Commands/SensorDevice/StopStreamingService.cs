using shpero.Rvr.Protocol;

namespace shpero.Rvr.Commands.SensorDevice
{
    [Command(CommandId, DeviceId)]
    public class StopStreamingService : Command
    {
        private readonly byte _targetId;

        public const byte CommandId = 0x3B;

        public const DeviceIdentifier DeviceId = DeviceIdentifier.Sensor;

        public StopStreamingService(byte targetId)
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