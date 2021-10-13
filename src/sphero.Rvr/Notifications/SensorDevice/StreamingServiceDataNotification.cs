using sphero.Rvr.Protocol;
using System;

namespace sphero.Rvr.Notifications.SensorDevice
{
    public class StreamingServiceDataNotification : Event
    {
        public StreamingServiceDataNotification(Message message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            SourceId = message.Header.SourceId;
            Token = message.Data[0];
            SensorData = message.Data.Length > 1 ? message.Data[1..] : Array.Empty<byte>();
        }

        public byte SourceId { get; }

        public byte[] SensorData { get; }

        public byte Token { get;}
    }
}
