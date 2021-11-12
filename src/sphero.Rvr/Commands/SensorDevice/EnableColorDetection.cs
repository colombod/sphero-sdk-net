using sphero.Rvr.Protocol;

namespace sphero.Rvr.Commands.SensorDevice;

[Command(CommandId, DeviceId)]
public class EnableColorDetection : Command
{
    private readonly bool _enable;

    public const byte CommandId = 0x38;

    public const DeviceIdentifier DeviceId = DeviceIdentifier.Sensor;

    public EnableColorDetection(bool enable)
    {
        _enable = enable;
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
        return new Message(header, _enable ? new byte[] { 0x01 } : new byte[] { 0x00 });
    }
}