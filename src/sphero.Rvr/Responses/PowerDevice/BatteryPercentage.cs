using System;
using sphero.Rvr.Commands.PowerDevice;
using sphero.Rvr.Protocol;

namespace sphero.Rvr.Responses.PowerDevice;

[OriginatingCommand(typeof(GetBatterPercentage))]
public class BatteryPercentage : Response
{
    public BatteryPercentage(Message message)
    {
        if (message == null)
        {
            throw new ArgumentNullException(nameof(message));
        }

        Percentage = message.Data[0];
    }

    public byte Percentage { get; set; }
}