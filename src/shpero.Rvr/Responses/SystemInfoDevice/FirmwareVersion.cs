using System;
using shpero.Rvr.Commands.SystemInfoDevice;
using shpero.Rvr.Protocol;

namespace shpero.Rvr.Responses.SystemInfoDevice
{
    [OriginatingCommand(typeof(GetFirmwareVersion))]
    public class FirmwareVersion: Response
    {
        public FirmwareVersion(Message message)
        {
            message = message ?? throw new ArgumentNullException(nameof(message));
            Version = message.Data.ToVersion();
        }

        public Version Version { get; }
    }
}