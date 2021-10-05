using shpero.Rvr.Protocol;

namespace shpero.Rvr.Commands.PowerDevice
{
    [Command(CommandId, DeviceId)]
    public class Wake : Command
    {
        public const byte CommandId = 0x0D;

        public const DeviceIdentifier DeviceId = DeviceIdentifier.Power;
        public override Message ToMessage()
        {
            var header = new Header(
                commandId: CommandId,
                targetId: 0x01,
                deviceId: DeviceId,
                sourceId: ApiTargetsAndSources.ServiceSource,
                sequence: GetSequenceNumber(),
                flags: Flags.DefaultRequestWithNoResponseFlags);
            return new Message(header);
        }
    }
}