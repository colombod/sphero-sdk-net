using sphero.Rvr.Protocol;
using System;

namespace sphero.Rvr.Notifications.SensorDevice
{
    public class ColorDetectionNotification : Event
    {
        public ColorDetectionNotification()
        {

        }
        public ColorDetectionNotification(Message message): this(message?.Data)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));
            FromMessage(message);
        }

        public ColorDetectionNotification(byte[] rawData)
        {
            if (rawData == null) throw new ArgumentNullException(nameof(rawData));
            FromRawData(rawData, 0);
        }

        public void FromMessage(Message message)
        {
            FromRawData(message.Data, 0);
        }

        public int FromRawData(byte[] rawData, int offset)
        {
            if (rawData == null) throw new ArgumentNullException(nameof(rawData));

            Color = new Color(rawData[offset+0], rawData[offset+1], rawData[offset+2]);

            Confidence = ((float)rawData[offset+3]) / 255.0f;
            ColorClassificationId = rawData[offset+4];
            return 5;
        }

        public byte ColorClassificationId { get; private set; }

        public float Confidence { get; private set; }

        public Color Color { get; private set; }
    }
}
