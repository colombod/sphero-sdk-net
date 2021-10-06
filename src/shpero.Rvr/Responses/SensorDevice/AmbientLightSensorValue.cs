using System;
using shpero.Rvr.Commands.SensorDevice;
using shpero.Rvr.Protocol;

namespace shpero.Rvr.Responses.SensorDevice
{
    [OriginatingCommand(typeof(GetAmbientLightSensorValue))]
    public class AmbientLightSensorValue : Response
    {
        public AmbientLightSensorValue(Message message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            Value = message.Data[..4].ToFloat();
        }

        public float Value { get;  }
    }
}