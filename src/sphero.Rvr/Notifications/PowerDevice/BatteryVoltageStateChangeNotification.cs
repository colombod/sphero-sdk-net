using sphero.Rvr.Protocol;
using System;

namespace sphero.Rvr.Notifications.PowerDevice
{
    public class BatteryVoltageStateChangeNotification : Event
    {
        public BatteryVoltageState State { get;  }  
        public BatteryVoltageStateChangeNotification(Message message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            State = (BatteryVoltageState)message.Data[0];
        }
    }
}
