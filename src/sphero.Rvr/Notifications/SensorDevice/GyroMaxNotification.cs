using sphero.Rvr.Protocol;

using System;

namespace sphero.Rvr.Notifications.SensorDevice;

public class GyroMaxNotification : Event
{
    public GyroMaxNotification(Message message)
    {
        if (message == null)
        {
            throw new ArgumentNullException(nameof(message));
        }

        Flags = (GyroMaxFlag)message.Data[0];
    }

    public GyroMaxFlag Flags
    {
        get;
    }
}