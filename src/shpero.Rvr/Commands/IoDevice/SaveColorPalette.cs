using shpero.Rvr.Protocol;

namespace shpero.Rvr.Commands.IoDevice
{
    [Command(CommandId, DeviceId)]
    public class SaveColorPalette : Command
    {
        private readonly byte _paletteIndex;
        public const byte CommandId = 0x48;

        public const DeviceIdentifier DeviceId = DeviceIdentifier.IO;

        public SaveColorPalette(byte paletteIndex)
        {
            _paletteIndex = paletteIndex;
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
            return new Message(header, new[] { _paletteIndex });
        }
    }
}