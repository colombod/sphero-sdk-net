using System;
using sphero.Rvr.Commands.PowerDevice;
using sphero.Rvr.Protocol;
using UnitsNet;

namespace sphero.Rvr.Responses.PowerDevice
{
    [OriginatingCommand(typeof(GetBatteryVoltageStateThresholds))]
    public class BatteryVoltageStateThresholds : Response
    {
        public BatteryVoltageStateThresholds(Message message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            CriticalThreshold = ElectricPotential.FromVolts(message.Data[..4].ToFloat());
            LowThreshold = ElectricPotential.FromVolts(message.Data[4..8].ToFloat());
            Hysteresis = message.Data[8..12].ToFloat();
        }

        public float Hysteresis { get; }

        public ElectricPotential LowThreshold { get; }

        public ElectricPotential CriticalThreshold { get; }
    }
}