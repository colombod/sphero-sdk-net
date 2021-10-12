using System;
using sphero.Rvr.Protocol;

namespace sphero.Rvr.Commands.SensorDevice
{
    [Command(CommandId, DeviceId)]
    public class StartStreamingService : Command
    {
        private readonly byte _targetId;
        private readonly TimeSpan _interval;

        public const byte CommandId = 0x3A;

        public const DeviceIdentifier DeviceId = DeviceIdentifier.Sensor;

        public StartStreamingService(byte targetId, TimeSpan interval)
        {
            _targetId = targetId;
            _interval = interval;
        }

        public override Message ToMessage()
        {
            var value = (ushort)(_interval.TotalMilliseconds % ushort.MaxValue);
            var rawData = new byte[2];
            for (var i = 1; i >= 0; i--)
            {
                var byteValue = value & 0xFF;
                rawData[i] = (byte)byteValue;
                value = (ushort)((value - byteValue) / 256);
            }

            var header = new Header(
                commandId: CommandId,
                targetId: _targetId,
                deviceId: DeviceId,
                sourceId: ApiTargetsAndSources.ServiceSource,
                sequence: GetSequenceNumber(),
                flags: Flags.DefaultRequestWithNoResponseFlags);
            return new Message(header, rawData);
        }
    }
}