using sphero.Rvr.Devices;

using System;
using System.Diagnostics;
using System.Reactive.Disposables;
using System.Threading;
using System.Threading.Tasks;

using UnitsNet;

namespace sphero.Rvr.Console;

internal class Program
{
    static async Task Main(string[] args)
    {
        var port = args.Length > 0 ? args[0] : "COM5";

        var rover = new Rover(port);
      //  rover.EnableLogging();
        var systemInfo = await rover.WakeAsync(CancellationToken.None);

        System.Console.WriteLine(systemInfo);

        await rover.ConfigureRoverAsync(CancellationToken.None);

        System.Console.WriteLine(nameof(TestDriverWithYaw));
        await TestDriverWithYaw(rover, CancellationToken.None);

        System.Console.WriteLine(nameof(TestLeds));
        await TestLeds(rover, CancellationToken.None);

        System.Console.WriteLine(nameof(TestSensorSubscriptions));
        await TestSensorSubscriptions(rover, CancellationToken.None);

        await rover.SleepAsync(CancellationToken.None);

        await Task.Delay(1000);

    }

    private static async Task TestDriverWithYaw(Rover rover, CancellationToken cancellationToken)
    {
        rover.DriveWithYaw(Angle.Zero, Speed.FromMetersPerSecond(1));

        await Task.Delay(3000, cancellationToken);

        rover.DriveWithYaw(Angle.Zero, Speed.FromMetersPerSecond(2));

        await Task.Delay(3000, cancellationToken);

        rover.DriveWithYaw(Angle.Zero, Speed.FromMetersPerSecond(0.2));

        await Task.Delay(3000, cancellationToken);

        rover.Stop();
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

            rover.LocatorStream.Subscribe(notification => System.Console.WriteLine($"[{nameof(rover.LocatorStream)}] => {notification}")),

            rover.SpeedStream.Subscribe(notification => System.Console.WriteLine($"[{nameof(rover.SpeedStream)}] => {notification}")),

            rover.CoreTimeLowerStream.Subscribe(notification => System.Console.WriteLine($"[{nameof(rover.CoreTimeLowerStream)}] => {notification}")),

            rover.CoreTimeUpperStream.Subscribe(notification => System.Console.WriteLine($"[{nameof(rover.CoreTimeUpperStream)}] => {notification}")),
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

}