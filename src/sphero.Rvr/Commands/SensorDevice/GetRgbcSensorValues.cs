using sphero.Rvr.Protocol;

namespace sphero.Rvr.Commands.SensorDevice;

[Command(CommandId, DeviceId)]
public class GetRgbcSensorValues : Command
{
    public const byte CommandId = 0x23;

    public const DeviceIdentifier DeviceId = DeviceIdentifier.Sensor;

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