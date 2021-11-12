using sphero.Rvr.Protocol;

namespace sphero.Rvr.Commands.IoDevice;

[Command(CommandId, DeviceId)]
public class ReleaseLed : Command
{
    public const byte CommandId = 0x4E;

    public const DeviceIdentifier DeviceId = DeviceIdentifier.IO;

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