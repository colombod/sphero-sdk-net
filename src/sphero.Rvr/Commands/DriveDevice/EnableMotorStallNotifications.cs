using sphero.Rvr.Protocol;

namespace sphero.Rvr.Commands.DriveDevice;

[Command(CommandId, DeviceId)]
public class EnableMotorStallNotifications : Command
{
    public const byte CommandId = 0x25;

    public const DeviceIdentifier DeviceId = DeviceIdentifier.Drive;

    private readonly bool _enable;

    public EnableMotorStallNotifications(bool enable)
    {
        _enable = enable;
    }

    public override Message ToMessage()
    {
        var header = new Header(
            commandId: CommandId,
            targetId: 0x02,
            deviceId: DeviceId,
            sourceId: ApiTargetsAndSources.ServiceSource,
            sequence: GetSequenceNumber(),
            flags: Flags.DefaultRequestWithNoResponseFlags);
        return new Message(header, new[] { (byte)(_enable ? 1 : 0) });
    }
}