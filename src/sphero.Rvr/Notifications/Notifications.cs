using sphero.Rvr.Protocol;

using System;
using System.Collections.Generic;

namespace sphero.Rvr.Notifications;

internal static class NotificationExtensions
{
    private static readonly Dictionary<Type, (byte SourceId, byte DeviceId, byte CommandId)> TypeToKey = new()
    {
        [typeof(sphero.Rvr.Notifications.DriveDevice.MotorStallNotification)] = (2, 22, 38),
        [typeof(sphero.Rvr.Notifications.DriveDevice.MotorFaultNotification)] = (2, 22, 40),
        [typeof(sphero.Rvr.Notifications.DriveDevice.ActiveControllerHasStopped)] = (2, 22, 63),
        [typeof(sphero.Rvr.Notifications.DriveDevice.ReachedTargetXYPosition)] = (2, 22, 58),
        [typeof(sphero.Rvr.Notifications.SensorDevice.GyroMaxNotification)] = (2, 24, 16),
        [typeof(sphero.Rvr.Notifications.SensorDevice.RobotToRobotInfraredMessageReceivedNotification)] = (2, 24, 44),
        [typeof(sphero.Rvr.Notifications.SensorDevice.ColorDetectionNotification)] = (1, 24, 54),
        [typeof(sphero.Rvr.Notifications.SensorDevice.StreamingServiceDataNotification)] = (1, 24, 61),
        [typeof(sphero.Rvr.Notifications.SensorDevice.StreamingServiceDataNotification)] = (2, 24, 61),
        [typeof(sphero.Rvr.Notifications.SensorDevice.MotorThermalProtectionStatusNotification)] = (2, 24, 77),
        [typeof(sphero.Rvr.Notifications.PowerDevice.BatteryVoltageStateChangeNotification)] = (1, 19, 28),
    };

    private static readonly Dictionary<Type, SensorId> TypeToSensorId = new()
    {
        [typeof(sphero.Rvr.Notifications.SensorDevice.QuaternionNotification)] = SensorId.Quaternion,
        //[typeof(sphero.Rvr.Notifications.SensorDevice.SpeedNotification)] = SensorId.Speed,
        [typeof(sphero.Rvr.Notifications.SensorDevice.VelocityNotification)] = SensorId.Velocity,
        [typeof(sphero.Rvr.Notifications.SensorDevice.GyroscopeNotification)] = SensorId.Gyroscope,
        [typeof(sphero.Rvr.Notifications.SensorDevice.ColorDetectionNotification)] = SensorId.ColorDetection,
        [typeof(sphero.Rvr.Notifications.SensorDevice.AmbientLightNotification)] = SensorId.AmbientLight,
        [typeof(sphero.Rvr.Notifications.SensorDevice.AttitudeNotification)] = SensorId.Attitude,
        [typeof(sphero.Rvr.Notifications.SensorDevice.AccelerometerNotification)] = SensorId.Accelerometer,
        [typeof(sphero.Rvr.Notifications.SensorDevice.LocatorNotification)] = SensorId.Locator,
        [typeof(sphero.Rvr.Notifications.SensorDevice.CoreTimeLowerNotification)] = SensorId.CoreTimeLower,
        [typeof(sphero.Rvr.Notifications.SensorDevice.CoreTimeUpperNotification)] = SensorId.CoreTimeUpper,
        [typeof(sphero.Rvr.Notifications.SensorDevice.EncodersNotification)] = SensorId.Encoders
    };

    public static bool TryGetKey(Type messageType, out (byte SourceId, byte DeviceId, byte CommandId) key)
    {
        return TypeToKey.TryGetValue(messageType, out key);
    }

    public static bool TryGetSensorId(Type messageType, out SensorId sensorId)
    {
        return TypeToSensorId.TryGetValue(messageType, out sensorId);
    }

    public static Event ToNotification(this Message message)
    {
        var key = (message.Header.SourceId,
            (byte)message.Header.DeviceId,
            message.Header.CommandId);
        switch (key)
        {
            case (2, 22, 38):
                return new sphero.Rvr.Notifications.DriveDevice.MotorStallNotification(message);
            case (2, 22, 40):
                return new sphero.Rvr.Notifications.DriveDevice.MotorFaultNotification(message);
            case (2, 22, 63):
                return new sphero.Rvr.Notifications.DriveDevice.ActiveControllerHasStopped(message);
            case (2, 22, 58):
                return new sphero.Rvr.Notifications.DriveDevice.ActiveControllerHasStopped(message);
            case (2, 24, 16):
                return new sphero.Rvr.Notifications.SensorDevice.GyroMaxNotification(message);
            case (2, 24, 44):
                return new sphero.Rvr.Notifications.SensorDevice.RobotToRobotInfraredMessageReceivedNotification(message);
            case (1, 24, 54):
                return new sphero.Rvr.Notifications.SensorDevice.ColorDetectionNotification(message);
            case (1, 24, 61):
                return new sphero.Rvr.Notifications.SensorDevice.StreamingServiceDataNotification(message);
            case (2, 24, 61):
                return new sphero.Rvr.Notifications.SensorDevice.StreamingServiceDataNotification(message);
            case (2, 24, 77):
                return new sphero.Rvr.Notifications.SensorDevice.MotorThermalProtectionStatusNotification(message);
            case (1, 19, 17):
                return new sphero.Rvr.Notifications.PowerDevice.BatteryVoltageStateChangeNotification(message);


            default:
                throw new ArgumentException($"Could not process message {key}", nameof(message));
        }
    }
}