using sphero.Rvr.Protocol;

namespace sphero.Rvr.Commands.SystemInfoDevice
{
    [Command(CommandId, DeviceId)]
    public class GetProcessorName : Command
    {
        public const byte CommandId = 0x1F;

        public const DeviceIdentifier DeviceId = DeviceIdentifier.SystemInfo;

        public byte ProcessorId { get; }

        public GetProcessorName(byte processorId)
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
}