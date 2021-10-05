using System;
using shpero.Rvr.Protocol;

namespace shpero.Rvr.Commands.SystemInfoDevice
{
    [Command(CommandId, DeviceId)]
    public class GetFirmwareVersion : Command
    {
        public const byte CommandId = 0x00;

        public const DeviceIdentifier DeviceId = DeviceIdentifier.SystemInfo;

        public byte ProcessorId { get; }

        public GetFirmwareVersion(byte processorId)
        {
            ProcessorId = processorId;
        }

        public override Message ToMessage()
        {
            var header = new Header(
                commandId:CommandId,
                targetId:ProcessorId,
                deviceId: DeviceId,
                sourceId: ApiTargetsAndSources.ServiceSource,
                sequence:GetSequenceNumber(),
                flags: Flags.DefaultRequestWithResponseFlags);
            return new Message(header);
        }
    }
}