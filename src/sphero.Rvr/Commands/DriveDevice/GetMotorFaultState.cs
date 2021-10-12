using sphero.Rvr.Protocol;

namespace sphero.Rvr.Commands.DriveDevice
{
    [Command(CommandId, DeviceId)]
    public class GetMotorFaultState : Command
    {
        public const byte CommandId = 0x29;

        public const DeviceIdentifier DeviceId = DeviceIdentifier.Drive;
        public override Message ToMessage()
        {
            var header = new Header(
                commandId: CommandId,
                targetId: 0x02,
                deviceId: DeviceId,
                sourceId: ApiTargetsAndSources.ServiceSource,
                sequence: GetSequenceNumber(),
                flags: Flags.DefaultRequestWithResponseFlags);
            return new Message(header);
        }
    }
}