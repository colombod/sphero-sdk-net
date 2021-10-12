using System;
using sphero.Rvr.Commands.ConnectionDevice;
using sphero.Rvr.Protocol;

namespace sphero.Rvr.Responses.ConnectionDevice
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
