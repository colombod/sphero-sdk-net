using System;
using shpero.Rvr.Commands.PowerDevice;
using shpero.Rvr.Protocol;

namespace shpero.Rvr.Responses.PowerDevice
{
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
}
