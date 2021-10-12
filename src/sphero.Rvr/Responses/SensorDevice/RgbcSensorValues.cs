using System;
using sphero.Rvr.Commands.SensorDevice;
using sphero.Rvr.Protocol;

namespace sphero.Rvr.Responses.SensorDevice
{
    [OriginatingCommand(typeof(GetRgbcSensorValues))]
    public class RgbcSensorValues : Response
    {
        public RgbcSensorValues(Message message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            RedChannelValue = message.Data[..2].ToUshort();
            GreenChannelValue = message.Data[2..4].ToUshort();
            BlueChannelValue = message.Data[4..6].ToUshort();
            ClearChannelValue = message.Data[6..8].ToUshort();
        }

        public ushort ClearChannelValue { get; }

        public ushort BlueChannelValue { get; }

        public ushort GreenChannelValue { get; }

        public ushort RedChannelValue { get; }
    }
}