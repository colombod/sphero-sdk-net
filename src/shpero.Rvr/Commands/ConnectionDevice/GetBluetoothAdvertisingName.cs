using shpero.Rvr.Protocol;

namespace shpero.Rvr.Commands.ConnectionDevice
{
    [Command(CommandId, DeviceId)]
    public class GetBluetoothAdvertisingName: Command
    {
        public const byte CommandId = 0x05;

        public const DeviceIdentifier DeviceId = DeviceIdentifier.Connection;

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
