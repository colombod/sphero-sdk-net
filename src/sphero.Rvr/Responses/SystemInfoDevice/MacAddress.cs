using sphero.Rvr.Commands.SystemInfoDevice;
using sphero.Rvr.Protocol;
using System;
using System.Net.NetworkInformation;
using System.Text;

namespace sphero.Rvr.Responses.SystemInfoDevice
{
    [OriginatingCommand(typeof(GetMacAddress))]
    public class MacAddress : Response
    {
        public MacAddress(Message message)
        {
            message = message ?? throw new ArgumentNullException(nameof(message));
            var addressString = Encoding.ASCII.GetString(message.Data[..12]);
            Address = PhysicalAddress.Parse(addressString);
        }

        public PhysicalAddress Address { get; }
    }
}