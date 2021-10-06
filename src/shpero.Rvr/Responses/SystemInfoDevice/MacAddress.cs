using shpero.Rvr.Commands.SystemInfoDevice;
using shpero.Rvr.Protocol;
using System;
using System.Net.NetworkInformation;
using System.Text;

namespace shpero.Rvr.Responses.SystemInfoDevice
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