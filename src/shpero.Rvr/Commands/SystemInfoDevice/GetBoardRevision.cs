using shpero.Rvr.Protocol;

namespace shpero.Rvr.Commands.SystemInfoDevice
{
    [Command(CommandId, DeviceId)]
    public class GetBoardRevision: Command
    {
        public const byte CommandId = 0x03;

        public const DeviceIdentifier DeviceId = DeviceIdentifier.SystemInfo;

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