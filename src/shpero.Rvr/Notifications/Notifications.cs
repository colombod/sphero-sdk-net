﻿using shpero.Rvr.Protocol;
using System;

namespace shpero.Rvr.Notifications{
    internal static class NotificationExtensions {
        public static Event ToNotification(this Message message){
            var key = (message.Header.SourceId,
                       (byte)message.Header.DeviceId,
                       message.Header.CommandId);
            switch (key){
                                case (2, 22, 38): 
                    return new shpero.Rvr.Notifications.DriveDevice.MotorStallNotification(message);
                                case (2, 22, 40): 
                    return new shpero.Rvr.Notifications.DriveDevice.MotorFaultNotification(message);
                                case (2, 24, 16): 
                    return new shpero.Rvr.Notifications.SensorDevice.GyroMaxNotification(message);
                                case (2, 24, 44): 
                    return new shpero.Rvr.Notifications.SensorDevice.RobotToRobotInfraredMessageReceivedNotification(message);
                                case (1, 24, 54): 
                    return new shpero.Rvr.Notifications.SensorDevice.ColorDetectionNotification(message);
                                case (1, 24, 61): 
                    return new shpero.Rvr.Notifications.SensorDevice.StreamingServiceDataNotification(message);
                                case (2, 24, 61): 
                    return new shpero.Rvr.Notifications.SensorDevice.StreamingServiceDataNotification(message);
                                case (2, 24, 77): 
                    return new shpero.Rvr.Notifications.SensorDevice.MotorThermalProtectionStatusNotification(message);
                                case (1, 19, 28): 
                    return new shpero.Rvr.Notifications.PowerDevice.BatteryVoltageStateChangeNotification(message);
                                default:
                    throw new ArgumentException($"Could not process message {key}", nameof(message));
            }
        }
    }
}