using System;
using sphero.Rvr.Commands.SystemInfoDevice;
using sphero.Rvr.Protocol;

namespace sphero.Rvr.Responses.SystemInfoDevice;

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

    public ushort Id { get; }
}