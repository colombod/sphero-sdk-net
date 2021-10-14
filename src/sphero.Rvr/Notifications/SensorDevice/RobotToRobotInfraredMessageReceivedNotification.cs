using sphero.Rvr.Protocol;
using System;

namespace sphero.Rvr.Notifications.SensorDevice
{
    public class RobotToRobotInfraredMessageReceivedNotification : Event
    {
        public RobotToRobotInfraredMessageReceivedNotification(Message message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            Code = message.Data[0];
        }

        public byte Code { get;  }
    }
}
