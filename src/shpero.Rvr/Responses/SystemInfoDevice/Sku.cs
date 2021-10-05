using System;
using shpero.Rvr.Commands.SystemInfoDevice;
using shpero.Rvr.Protocol;

namespace shpero.Rvr.Responses.SystemInfoDevice
{
    [OriginatingCommand(typeof(GetSku))]
    public class Sku : Response
    {
        public Sku(Message message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }
            Value = BitConverter.ToString(message.Data);
        }

        public string Value { get;  }
    }
}