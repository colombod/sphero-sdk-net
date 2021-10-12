using System;
using sphero.Rvr.Commands.SystemInfoDevice;
using sphero.Rvr.Protocol;

namespace sphero.Rvr.Responses.SystemInfoDevice
{
    [OriginatingCommand(typeof(GetBootLoaderVersion))]
    public class BootLoaderVersion : Response
    {
        public BootLoaderVersion(Message message)
        {
            message = message ?? throw new ArgumentNullException(nameof(message));
            Version = message.Data.ToVersion();
        }

        public Version Version { get; }
    }
}