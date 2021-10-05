using System;
using System.Net.NetworkInformation;
using shpero.Rvr.Commands.SystemInfoDevice;
using shpero.Rvr.Protocol;

namespace shpero.Rvr.Responses.SystemInfoDevice
{
    [OriginatingCommand(typeof(GetMacAddress))]
    public class MacAddress : Response
    {
        public MacAddress(Message message)
        {
            message = message ?? throw new ArgumentNullException(nameof(message));
            var addressString = BitConverter.ToString(message.Data);
            Address = PhysicalAddress.Parse(addressString);
        }

        public PhysicalAddress Address { get; }
    }
}