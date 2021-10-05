using System;
using shpero.Rvr.Commands.ConnectionDevice;
using shpero.Rvr.Protocol;

namespace shpero.Rvr.Responses.ConnectionDevice
{
    [OriginatingCommand(typeof(GetBluetoothAdvertisingName))]
    public class BluetoothAdvertisingName : Response
    {
        public BluetoothAdvertisingName(Message message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            Name = BitConverter.ToString(message.Data);
        }

        public string Name { get; }
    }
}
