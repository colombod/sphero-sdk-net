using System;
using shpero.Rvr.Commands.SystemInfoDevice;
using shpero.Rvr.Protocol;

namespace shpero.Rvr.Responses.SystemInfoDevice
{
    [OriginatingCommand(typeof(GetStatsId))]
    public class StatsId : Response
    {
        public StatsId(Message message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }
            Id = message.Data[..2].ToUshort();
        }

        public ushort Id { get;  }
    }
}