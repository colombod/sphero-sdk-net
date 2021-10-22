using sphero.Rvr.Devices;

using System;
using System.Reactive.Disposables;
using System.Threading;
using System.Threading.Tasks;

namespace sphero.Rvr.Console
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var port = args.Length > 0 ? args[0] : "COM5";

            var rover = new Rover(port);

            var systemInfo = await rover.WakeAsync(CancellationToken.None);

            await rover.ConfigureRoverAsync(CancellationToken.None);

            System.Console.WriteLine(nameof(TestLeds));
            await TestLeds(rover, CancellationToken.None);

            System.Console.WriteLine(nameof(TestSensorSubscriptions));
            await TestSensorSubscriptions(rover, CancellationToken.None);

            await rover.SleepAsync(CancellationToken.None);

            // using var driver = new Driver(port);

            //var power = new PowerDevice(driver);

            //power.Subscribe(notification =>
            //{
            //    System.Console.WriteLine($" Battery voltage state : {notification.State}");
            //});

            //await power.EnableBatteryVoltageStateChangeNotificationsAsync(true, CancellationToken.None);

            //await power.WakeAsync(CancellationToken.None);

            //await TestDevice(new SensorDevice(driver));

            //await TestDevice(new SystemInfoDevice(driver));

            //await TestDevice(new IoDevice(driver));

            //System.Console.ReadLine();

            //await power.SleepAsync(CancellationToken.None);

            await Task.Delay(1000);

        }

        private static async Task TestSensorSubscriptions(Rover rover, CancellationToken cancellationToken)
        {
            var subscriptions = new CompositeDisposable
            {
                rover.AccelerationStream.Subscribe(notification => System.Console.WriteLine($"[{nameof(rover.AccelerationStream)}] => {notification}")),

                rover.AmbientLightStream.Subscribe(notification => System.Console.WriteLine($"[{nameof(rover.AmbientLightStream)}] => {notification}")),

                rover.AttitudeStream.Subscribe(notification => System.Console.WriteLine($"[{nameof(rover.AttitudeStream)}] => {notification}")),

                rover.QuaternionStream.Subscribe(notification => System.Console.WriteLine($"[{nameof(rover.QuaternionStream)}] => {notification}")),

                rover.ColorDetectionStream.Subscribe(notification => System.Console.WriteLine($"[{nameof(rover.ColorDetectionStream)}] => {notification}")),

                rover.GyroscopeStream.Subscribe(notification => System.Console.WriteLine($"[{nameof(rover.GyroscopeStream)}] => {notification}")),

                rover.VelocityStream.Subscribe(notification => System.Console.WriteLine($"[{nameof(rover.VelocityStream)}] => {notification}")),

                rover.SpeedStream.Subscribe(notification => System.Console.WriteLine($"[{nameof(rover.SpeedStream)}] => {notification}")),
        };

            await Task.Delay(5000, cancellationToken);
            subscriptions.Dispose();
        }

        private static async Task TestLeds(Rover rover, CancellationToken cancellationToken)
        {
            await rover.SetAllLedOffAsync(cancellationToken);

            await rover.SetLedAsync(Led.HeadLightRight, Color.Colors[ColorNames.Orange], CancellationToken.None);

            await Task.Delay(1000, cancellationToken);

            await rover.SetAllLedAsync(Color.Colors[ColorNames.Blue], CancellationToken.None);

            await Task.Delay(1000, cancellationToken);

            await rover.SetAllLedAsync(Color.Colors[ColorNames.Pink], CancellationToken.None);

            await Task.Delay(1000, cancellationToken);

            await rover.SetAllLedAsync(Color.Colors[ColorNames.Green], CancellationToken.None);

            await Task.Delay(1000, cancellationToken);

            await rover.SetAllLedAsync(Color.Colors[ColorNames.White], CancellationToken.None);

            await Task.Delay(1000, cancellationToken);

            await rover.SetAllLedOffAsync(cancellationToken);
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
