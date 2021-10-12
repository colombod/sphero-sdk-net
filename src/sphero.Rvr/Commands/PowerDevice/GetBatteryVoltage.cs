using sphero.Rvr.Protocol;

namespace sphero.Rvr.Commands.PowerDevice
{
    [Command(CommandId, DeviceId)]
    public class GetBatteryVoltage : Command
    {
        private readonly BatteryVoltageReadings _readings;

        public const byte CommandId = 0x25;

        public const DeviceIdentifier DeviceId = DeviceIdentifier.Power;

        public GetBatteryVoltage(BatteryVoltageReadings readings)
        {
            _readings = readings;
        }
        public override Message ToMessage()
        {
            var header = new Header(
                commandId: CommandId,
                targetId: 0x01,
                deviceId: DeviceId,
                sourceId: ApiTargetsAndSources.ServiceSource,
                sequence: GetSequenceNumber(),
                flags: Flags.DefaultRequestWithResponseFlags);
            return new Message(header, new byte[] { (byte)_readings });
        }
    }
}