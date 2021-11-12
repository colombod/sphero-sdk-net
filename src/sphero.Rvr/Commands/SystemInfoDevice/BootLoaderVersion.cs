using sphero.Rvr.Protocol;

namespace sphero.Rvr.Commands.SystemInfoDevice;

[Command(CommandId, DeviceId)]
public class GetBootLoaderVersion : Command
{
    public const byte CommandId = 0x01;

    public const DeviceIdentifier DeviceId = DeviceIdentifier.SystemInfo;

    public byte ProcessorId { get; }

    public GetBootLoaderVersion(byte processorId)
    {
        ProcessorId = processorId;
    }

    public override Message ToMessage()
    {
        var header = new Header(
            commandId: CommandId,
            targetId: ProcessorId,
            deviceId: DeviceId,
            sourceId: ApiTargetsAndSources.ServiceSource,
            sequence: GetSequenceNumber(),
            flags: Flags.DefaultRequestWithResponseFlags);
        return new Message(header);
    }
}