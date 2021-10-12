using shpero.Rvr.Protocol;
using System;

namespace shpero.Rvr.Notifications.SensorDevice
{
    public class ColorDetectionNotification : Event
    {
        public ColorDetectionNotification(Message message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            Color = new Color(message.Data[0], message.Data[1], message.Data[2]);

            Confidence = message.Data[3] / 255f;
            ColorClassificationId = message.Data[4];
        }

        public byte ColorClassificationId { get; }

        public float Confidence { get; }

        public Color Color { get; }
    }
}
