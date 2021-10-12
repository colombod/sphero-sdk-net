using System;
using sphero.Rvr.Protocol;

namespace sphero.Rvr.Commands.IoDevice
{
    [Command(CommandId, DeviceId)]
    public class SetAllLeds : Command
    {
        private readonly LedBitMask _ledBitMask;
        private readonly byte[] _brightnessValues;

        public const byte CommandId = 0x1A;

        public const DeviceIdentifier DeviceId = DeviceIdentifier.IO;


        public SetAllLeds(LedBitMask ledBitMask, byte[] brightnessValues)
        {
            _ledBitMask = ledBitMask;
            _brightnessValues = brightnessValues ?? throw new ArgumentNullException(nameof(brightnessValues));

            var size = _ledBitMask.GetMaxDataSize();
            if (_brightnessValues.Length > size)
            {
                throw new InvalidOperationException($"Max brightness values size is {size}.");
            }
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
            return new Message(header, GetRawData());
        }

        private byte[] GetRawData()
        {
            var data = new byte[sizeof(uint) + (_brightnessValues.Length * sizeof(byte))];

            var value = (uint)_ledBitMask;
            for (var i = 3; i >= 0; i--)
            {
                var byteValue = value & 0xFF;
                data[i] = (byte)byteValue;
                value = (value - byteValue) / 256;
            }

            for (var i = 0; i < _brightnessValues.Length; i++)
            {
                data[4 + i] = _brightnessValues[i];
            }

            return data;
        }
    }
}
