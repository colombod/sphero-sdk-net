using System;
using shpero.Rvr.Commands.SystemInfoDevice;
using shpero.Rvr.Protocol;

namespace shpero.Rvr.Responses.SystemInfoDevice
{
    [OriginatingCommand(typeof(GetBootLoaderVersion))]
    public class BootLoaderVersion: Response
    {
        public BootLoaderVersion(Message message)
        {
            message = message ?? throw new ArgumentNullException(nameof(message));
            Version = message.Data.ToVersion();
        }

        public Version Version { get; }
    }
}