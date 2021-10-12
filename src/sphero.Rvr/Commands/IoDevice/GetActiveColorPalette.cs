using sphero.Rvr.Protocol;

namespace sphero.Rvr.Commands.IoDevice
{
    [Command(CommandId, DeviceId)]
    public class GetActiveColorPalette : Command
    {
        public const byte CommandId = 0x44;

        public const DeviceIdentifier DeviceId = DeviceIdentifier.IO;

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