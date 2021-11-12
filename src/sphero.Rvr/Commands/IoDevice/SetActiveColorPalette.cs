using System;
using sphero.Rvr.Protocol;

namespace sphero.Rvr.Commands.IoDevice;

[Command(CommandId, DeviceId)]
public class SetActiveColorPalette : Command
{
    public const byte CommandId = 0x45;
    public const DeviceIdentifier DeviceId = DeviceIdentifier.IO;

    private readonly byte[] _colorPaletteRaw;

    public SetActiveColorPalette(byte[] colorPaletteRaw)
    {
        _colorPaletteRaw = colorPaletteRaw ?? throw new ArgumentNullException(nameof(colorPaletteRaw));
        if (_colorPaletteRaw.Length != 48)
        {
            throw new InvalidOperationException("Raw Pixel Palette should contain 48 bytes");
        }
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
        return new Message(header, _colorPaletteRaw);
    }
}