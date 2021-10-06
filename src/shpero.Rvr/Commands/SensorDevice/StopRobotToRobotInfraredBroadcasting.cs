using shpero.Rvr.Protocol;

namespace shpero.Rvr.Commands.SensorDevice
{
    [Command(CommandId, DeviceId)]
    public class StopRobotToRobotInfraredBroadcasting : Command
    {
        public const byte CommandId = 0x29;

        public const DeviceIdentifier DeviceId = DeviceIdentifier.Sensor;

        public override Message ToMessage()
        {
            var header = new Header(
                commandId: CommandId,
                targetId: 0x02,
                deviceId: DeviceId,
                sourceId: ApiTargetsAndSources.ServiceSource,
                sequence: GetSequenceNumber(),
                flags: Flags.DefaultRequestWithNoResponseFlags);
            return new Message(header);
        }
    }
}