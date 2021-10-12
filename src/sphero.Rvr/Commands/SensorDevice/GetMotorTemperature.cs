using sphero.Rvr.Protocol;

namespace sphero.Rvr.Commands.SensorDevice
{
    [Command(CommandId, DeviceId)]
    public class GetMotorTemperature : Command
    {
        private readonly MotorIndex _motorIndex;
        public const byte CommandId = 0x42;

        public const DeviceIdentifier DeviceId = DeviceIdentifier.Sensor;

        public GetMotorTemperature(MotorIndex motorIndex)
        {
            _motorIndex = motorIndex;
        }

        public override Message ToMessage()
        {
            var header = new Header(
                commandId: CommandId,
                targetId: 0x02,
                deviceId: DeviceId,
                sourceId: ApiTargetsAndSources.ServiceSource,
                sequence: GetSequenceNumber(),
                flags: Flags.DefaultRequestWithResponseFlags);
            return new Message(header,
                new[] { (byte)_motorIndex });
        }
    }
}