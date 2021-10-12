using System;
using sphero.Rvr.Commands.SensorDevice;
using sphero.Rvr.Protocol;

namespace sphero.Rvr.Responses.SensorDevice
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

        public float Value { get; }
    }
}