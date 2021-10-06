using System;
using shpero.Rvr.Commands.SensorDevice;
using shpero.Rvr.Protocol;
using UnitsNet;

namespace shpero.Rvr.Responses.SensorDevice
{
    [OriginatingCommand(typeof(GetMotorTemperature))]
    public class MotorTemperature : Response
    {
        public MotorTemperature(Message message)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));
            WindingCoilTemperature = Temperature.FromDegreesCelsius( message.Data[..4].ToFloat());
            CaseTemperature = Temperature.FromDegreesCelsius(message.Data[4..8].ToFloat());
        }

        public Temperature CaseTemperature { get; }

        public Temperature WindingCoilTemperature { get; }
    }

    

    [OriginatingCommand(typeof(GetMotorThermalProtectionStatus))]
    public class MotorThermalProtectionStatus : Response
    {
        public MotorThermalProtectionStatus(Message message)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));
            LeftMotorTemperature = Temperature.FromDegreesCelsius(message.Data[..4].ToFloat());
            RightMotorTemperature = Temperature.FromDegreesCelsius(message.Data[5..9].ToFloat());
            LeftMotorStatus = message.Data[4];
            RightMotorStatus = message.Data[9];
        }

        public byte RightMotorStatus { get;  }

        public byte LeftMotorStatus { get;  }

        public Temperature LeftMotorTemperature { get; }

        public Temperature RightMotorTemperature { get; }
    }
}