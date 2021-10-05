using System;
using shpero.Rvr.Commands.PowerDevice;
using shpero.Rvr.Protocol;

namespace shpero.Rvr.Responses.PowerDevice
{
    [OriginatingCommand(typeof(GetBatteryVoltageState))]
    public class BatterVoltageState : Response
    {
        public BatterVoltageState(Message message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            State = (BatteryVoltageState)(message.Data[0]);
        }

        public BatteryVoltageState State { get;  }
    }
}