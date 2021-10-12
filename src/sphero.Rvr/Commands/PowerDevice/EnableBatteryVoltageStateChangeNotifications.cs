using sphero.Rvr.Protocol;

namespace sphero.Rvr.Commands.PowerDevice
{
    [Command(CommandId, DeviceId)]
    public class EnableBatteryVoltageStateChangeNotifications : Command
    {
        private readonly bool _enable;
        public const byte CommandId = 0x1B;

        public const DeviceIdentifier DeviceId = DeviceIdentifier.Power;

        public EnableBatteryVoltageStateChangeNotifications(bool enable)
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
            var enableRawValue = _enable ? (byte)0x01 : (byte)0x00;
            return new Message(header, new[] { enableRawValue });
        }
    }
}