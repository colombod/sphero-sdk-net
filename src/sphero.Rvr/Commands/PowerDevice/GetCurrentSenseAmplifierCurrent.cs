using sphero.Rvr.Protocol;

namespace sphero.Rvr.Commands.PowerDevice;

[Command(CommandId, DeviceId)]
public class GetCurrentSenseAmplifierCurrent : Command
{
    private readonly AmplifierId _amplifierId;

    public const byte CommandId = 0x27;

    public const DeviceIdentifier DeviceId = DeviceIdentifier.Power;

    public GetCurrentSenseAmplifierCurrent(AmplifierId amplifierId)
    {
        _amplifierId = amplifierId;
    }

    public override Message ToMessage()
    {
        var header = new Header(
            commandId: CommandId,
            targetId: 0x01,
            deviceId: DeviceId,
            sourceId: ApiTargetsAndSources.ServiceSource,
            sequence: GetSequenceNumber(),
            flags: Flags.DefaultRequestWithResponseFlags);
        return new Message(header, new byte[] { (byte)_amplifierId });
    }
}