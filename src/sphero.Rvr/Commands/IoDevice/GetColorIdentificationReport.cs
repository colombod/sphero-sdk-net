using sphero.Rvr.Protocol;

namespace sphero.Rvr.Commands.IoDevice;

[Command(CommandId, DeviceId)]
public class GetColorIdentificationReport : Command
{

    public const byte CommandId = 0x46;

    public const DeviceIdentifier DeviceId = DeviceIdentifier.IO;

    private readonly byte _red;
    private readonly byte _green;
    private readonly byte _blue;
    private readonly byte _confidenceThreshold;

    public GetColorIdentificationReport(byte red, byte green, byte blue, byte confidenceThreshold)
    {
        _red = red;
        _green = green;
        _blue = blue;
        _confidenceThreshold = confidenceThreshold;
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
        return new Message(header, new[] { _red, _green, _blue, _confidenceThreshold });
    }
}