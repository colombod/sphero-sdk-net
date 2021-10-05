using System;
using shpero.Rvr.Commands.PowerDevice;
using shpero.Rvr.Protocol;
using UnitsNet;

namespace shpero.Rvr.Responses.PowerDevice
{
    [OriginatingCommand(typeof(GetBatteryVoltage))]
    public class BatterVoltage : Response
    {
        public BatterVoltage(Message message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            var voltageRaw = message.Data[..4].ToFloat();
            Voltage = ElectricPotential.FromVolts(voltageRaw);

        }

        public ElectricPotential Voltage { get; }
    }
}