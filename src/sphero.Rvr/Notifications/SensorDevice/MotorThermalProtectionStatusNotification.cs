using sphero.Rvr.Protocol;
using System;
using UnitsNet;

namespace sphero.Rvr.Notifications.SensorDevice
{
    public class MotorThermalProtectionStatusNotification : Event
    {

        public MotorThermalProtectionStatusNotification(Message message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }
            LeftMotorTemperature = Temperature.FromDegreesCelsius(message.Data[0..4].ToFloat());
            LeftMotorStatus = message.Data[4];
            RightMotorTemperature = Temperature.FromDegreesCelsius(message.Data[5..9].ToFloat());
            RightMotorStatus = message.Data[9];
        }

        public byte RightMotorStatus { get; private set; }

        public Temperature RightMotorTemperature { get; private set; }

        public byte LeftMotorStatus { get; private set; }

        public Temperature LeftMotorTemperature { get; private set; }
    }
}
