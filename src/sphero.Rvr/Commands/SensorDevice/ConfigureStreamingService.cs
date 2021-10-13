using System;
using sphero.Rvr.Protocol;

namespace sphero.Rvr.Commands.SensorDevice
{
    [Command(CommandId, DeviceId)]
    public class ConfigureStreamingService : Command
    {
        private readonly byte _targetId;
        private readonly byte _token;
        private readonly byte[] _configuration;

        public const byte CommandId = 0x39;

        public const DeviceIdentifier DeviceId = DeviceIdentifier.Sensor;

        public ConfigureStreamingService(byte targetId, byte token, byte[] configuration)
        {
            _targetId = targetId;
            _token = token;
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            if (_configuration.Length > 15)
            {
                throw new ArgumentOutOfRangeException(nameof(configuration), "Configuration cannot be longer than 15");
            }
        }

        public override Message ToMessage()
        {
            var rawData = new byte[_configuration.Length + 1];
            rawData[0] = _token;
            Buffer.BlockCopy(_configuration, 0, rawData, 1, _configuration.Length);
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