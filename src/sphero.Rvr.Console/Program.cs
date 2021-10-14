using System;
using sphero.Rvr;

using System.Threading;
using System.Threading.Tasks;
using sphero.Rvr.Devices;
using sphero.Rvr.Notifications.SensorDevice;
using sphero.Rvr.Responses.SensorDevice;

namespace sphero.Rvr.Console
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var port = args.Length > 0 ? args[0] : "COM5";
            using var driver = new Driver(port);

            var power = new PowerDevice(driver);

            power.Subscribe(notification =>
            {
                System.Console.WriteLine($" Battery voltage state : {notification.State}");
            });

            await power.EnableBatteryVoltageStateChangeNotificationsAsync(true, CancellationToken.None);

            await power.WakeAsync(CancellationToken.None);

            await TestDevice(new SensorDevice(driver));

            await TestDevice(new SystemInfoDevice(driver));

            await TestDevice(new IoDevice(driver));

            System.Console.ReadLine();

            await power.SleepAsync(CancellationToken.None);

            await Task.Delay(1000);

        }

        private static async Task TestDevice(SensorDevice sensorDevice)
        {
            var ambientLightSensorValue = await sensorDevice.GetAmbientLightSensorValueAsync(CancellationToken.None);
            System.Console.WriteLine(ambientLightSensorValue.Value);

            // await sensorDevice.EnableColorDetectionAsync(true, CancellationToken.None);

            //sensorDevice.SubscribeToColorDetectionNotifications(notification =>
            //{
            //    System.Console.WriteLine($"{notification.GetType().Name} : {notification.Color}, ClassificationId {notification.ColorClassificationId}, Confidence {notification.Confidence:F2}");
            //});

            //await sensorDevice.EnableColorDetectionNotificationsAsync(true, TimeSpan.FromSeconds(1), 0,  CancellationToken.None);


            await sensorDevice.ConfigureSensorStreamingAsync(
                new[]
                {
                    SensorId.AmbientLight,
                    SensorId.Accelerometer,
                    SensorId.Attitude,
                    SensorId.ColorDetection,
                    SensorId.CoreTimeLower,
                    SensorId.CoreTimeUpper,
                    SensorId.Gyroscope,
                    SensorId.Locator,
                    SensorId.Quaternion,
                    SensorId.Speed,
                    SensorId.Velocity
                }, CancellationToken.None);

            await sensorDevice.StartStreamingServiceAsync(TimeSpan.FromSeconds(1), CancellationToken.None);

            SubscribeToStStreams();

            SubscribeToNordicStreams();

            await Task.Delay(TimeSpan.FromSeconds(100));

            await sensorDevice.StopStreamingServiceAsync(CancellationToken.None);

            void SubscribeToNordicStreams()
            {
                sensorDevice.SubscribeToStream((ColorDetectionNotification notification) =>
                {
                    System.Console.WriteLine(
                        $"{notification.GetType().Name} : [{notification.Color}, {notification.Confidence}, {notification.ColorClassificationId}]");
                });

                sensorDevice.SubscribeToStream((CoreTimeLowerNotification notification) =>
                {
                    System.Console.WriteLine($"{notification.GetType().Name} : [{notification.Time}]");
                });

                sensorDevice.SubscribeToStream((CoreTimeUpperNotification notification) =>
                {
                    System.Console.WriteLine($"{notification.GetType().Name} : [{notification.Time}]");
                });

                sensorDevice.SubscribeToStream((AmbientLightNotification notification) =>
                {
                    System.Console.WriteLine($"{notification.GetType().Name} : [{notification.Light}]");
                });
            }

            void SubscribeToStStreams()
            {
                sensorDevice.SubscribeToStream((GyroscopeNotification notification) =>
                {
                    System.Console.WriteLine(
                        $"{notification.GetType().Name} : [{notification.X}, {notification.Y}, {notification.Z}]");
                });

                sensorDevice.SubscribeToStream((AttitudeNotification notification) =>
                {
                    System.Console.WriteLine(
                        $"{notification.GetType().Name} : [{notification.Pitch}, {notification.Roll}, {notification.Yaw}]");
                });

                sensorDevice.SubscribeToStream((AccelerometerNotification notification) =>
                {
                    System.Console.WriteLine(
                        $"{notification.GetType().Name} : [{notification.X}, {notification.Y}, {notification.Z}]");
                });

                sensorDevice.SubscribeToStream((VelocityNotification notification) =>
                {
                    System.Console.WriteLine($"{notification.GetType().Name} : [{notification.X}, {notification.Y}]");
                });

                sensorDevice.SubscribeToStream((SpeedNotification notification) =>
                {
                    System.Console.WriteLine($"{notification.GetType().Name} : [{notification.Speed}]");
                });

                sensorDevice.SubscribeToStream((QuaternionNotification notification) =>
                {
                    System.Console.WriteLine(
                        $"{notification.GetType().Name} : [{notification.W} {notification.X} {notification.Y} {notification.Z}]");
                });

                sensorDevice.SubscribeToStream((LocatorNotification notification) =>
                {
                    System.Console.WriteLine($"{notification.GetType().Name} : [{notification.X} {notification.Y}]");
                });
            }
        }
        private static async Task TestDevice(IoDevice io)
        {
            await io.SetAllLedsOffAsync(CancellationToken.None);

            await Task.Delay(1000);

            await io.SetLedsAsync(LedBitMask.HeadLightRight, Color.Colors[ColorNames.Orange].ToRawBytes(), CancellationToken.None);

            await Task.Delay(1000);

            await io.SetAllLedsAsync(Color.Colors[ColorNames.Blue], CancellationToken.None);

            await Task.Delay(1000);

            await io.SetAllLedsAsync(Color.Colors[ColorNames.Pink], CancellationToken.None);

            await Task.Delay(1000);

            await io.SetAllLedsAsync(Color.Colors[ColorNames.Green], CancellationToken.None);

            await Task.Delay(1000);

            await io.SetAllLedsAsync(Color.Colors[ColorNames.White], CancellationToken.None);

            await Task.Delay(1000);
        }

        private static async Task TestDevice(SystemInfoDevice systemInfo)
        {
            var nordicProcessorVersion = await systemInfo.GetFirmwareVersionForNordicProcessorAsync(CancellationToken.None);

            System.Console.WriteLine(nordicProcessorVersion.Version);

            var stProcessorVersion = await systemInfo.GetFirmwareVersionForSTProcessorAsync(CancellationToken.None);

            System.Console.WriteLine(stProcessorVersion.Version);

            var boardRevision = await systemInfo.GetBoardRevisionAsync(CancellationToken.None);

            System.Console.WriteLine(boardRevision.Revision);

            var macAddress = await systemInfo.GetMacAddressAsync(CancellationToken.None);

            System.Console.WriteLine(macAddress.Address);

            var statsId = await systemInfo.GetStatsIdAsync(CancellationToken.None);

            System.Console.WriteLine(statsId.Id);

            var proc1Name = await systemInfo.GetProcessorNameAsync(1, CancellationToken.None);

            System.Console.WriteLine(proc1Name.Name);

            var proc2Name = await systemInfo.GetProcessorNameAsync(2, CancellationToken.None);

            System.Console.WriteLine(proc2Name.Name);

            var sku = await systemInfo.GetSkuAsync(CancellationToken.None);

            System.Console.WriteLine(sku.Value);

            var coreUpTimeInMilliseconds = await systemInfo.GetCoreUpTimeInMillisecondsAsync(CancellationToken.None);

            System.Console.WriteLine(coreUpTimeInMilliseconds.Milliseconds);
        }
    }
}
