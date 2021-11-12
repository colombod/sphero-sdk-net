using System;
using sphero.Rvr.Protocol;

namespace sphero.Rvr.Commands.SensorDevice;

[Command(CommandId, DeviceId)]
public class EnableColorDetectionNotifications : Command
{
    private readonly bool _enable;
    private readonly TimeSpan _interval;
    private readonly byte _minimumConfidenceThreshold;

    public const byte CommandId = 0x35;

    public const DeviceIdentifier DeviceId = DeviceIdentifier.Sensor;

    public EnableColorDetectionNotifications(bool enable, TimeSpan interval, byte minimumConfidenceThreshold)
    {
        _enable = enable;
        _interval = interval;
        _minimumConfidenceThreshold = minimumConfidenceThreshold;
    }

    public override Message ToMessage()
    {
        var value = (ushort)(_interval.TotalMilliseconds % ushort.MaxValue);
        var rawData = new[] { _enable ? (byte)0x01 : (byte)0x00, (byte)((value >> 8) & 0xFF), (byte)(value & 0xFF), _minimumConfidenceThreshold };
        var header = new Header(
            commandId: CommandId,
            targetId: 0x01,
            deviceId: DeviceId,
            sourceId: ApiTargetsAndSources.ServiceSource,
            sequence: GetSequenceNumber(),
            flags: Flags.DefaultRequestWithNoResponseFlags);
        return new Message(header,  rawData);
    }
}