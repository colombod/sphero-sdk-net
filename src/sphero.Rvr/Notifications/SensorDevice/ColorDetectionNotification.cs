using sphero.Rvr.Protocol;
using System;

namespace sphero.Rvr.Notifications.SensorDevice
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

            Confidence = ((float)message.Data[3]) / 255.0f;
            ColorClassificationId = message.Data[4];
        }

        public byte ColorClassificationId { get; }

        public float Confidence { get; }

        public Color Color { get; }
    }
}
