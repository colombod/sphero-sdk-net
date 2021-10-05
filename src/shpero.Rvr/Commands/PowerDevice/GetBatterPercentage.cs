using shpero.Rvr.Protocol;

namespace shpero.Rvr.Commands.PowerDevice
{
    [Command(CommandId, DeviceId)]
    public class GetBatterPercentage : Command
    {
        public const byte CommandId = 0x10;

        public const DeviceIdentifier DeviceId = DeviceIdentifier.Power;
        public override Message ToMessage()
        {
            var header = new Header(
                commandId: CommandId,
                targetId: 0x01,
                deviceId: DeviceId,
                sourceId: ApiTargetsAndSources.ServiceSource,
                sequence: GetSequenceNumber(),
                flags: Flags.DefaultRequestWithResponseFlags);
            return new Message(header);
        }
    }
}