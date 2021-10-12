using sphero.Rvr.Protocol;

namespace sphero.Rvr.Commands.SensorDevice
{
    [Command(CommandId, DeviceId)]
    public class SetLocatorFlags : Command
    {
        private readonly LocatorFlag _locatorFlag;

        public const byte CommandId = 0x17;

        public const DeviceIdentifier DeviceId = DeviceIdentifier.Sensor;

        public SetLocatorFlags(LocatorFlag locatorFlag)
        {
            _locatorFlag = locatorFlag;
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
            return new Message(header, new[] { (byte)_locatorFlag });
        }
    }
}
