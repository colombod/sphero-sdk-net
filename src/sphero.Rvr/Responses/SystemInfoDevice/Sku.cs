using System;
using sphero.Rvr.Commands.SystemInfoDevice;
using sphero.Rvr.Protocol;

namespace sphero.Rvr.Responses.SystemInfoDevice
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
            Value = message.Data.ToStringFromNullTerminated();
        }

        public string Value { get; }
    }
}