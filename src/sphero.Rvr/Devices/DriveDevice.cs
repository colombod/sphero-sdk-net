using sphero.Rvr.Commands.DriveDevice;
using System;
using System.Threading;
using System.Threading.Tasks;
using sphero.Rvr.Notifications.DriveDevice;
using UnitsNet;
using GetMotorFaultState = sphero.Rvr.Responses.DriveDevice.GetMotorFaultState;

namespace sphero.Rvr.Devices;

public class DriveDevice : IDisposable
{
    public const DeviceIdentifier DeviceId = DeviceIdentifier.Drive;

    private readonly IDriver _driver;
    private readonly NotificationManager _notificationManager;

    public DriveDevice(IDriver driver)
    {
        _driver = driver ?? throw new ArgumentNullException(nameof(driver));
        _notificationManager = new NotificationManager(_driver);
    }

    public Task SetRawMotorAsync(RawMotorMode leftRawMotor, byte leftMotorSpeed, RawMotorMode rightRawMotor, byte rightMotorSpeed, CancellationToken cancellationToken)
    {
        var setRawMotor = new SetRawMotor(leftRawMotor, leftMotorSpeed, rightRawMotor, rightMotorSpeed);
        return _driver.SendAsync(setRawMotor.ToMessage(), cancellationToken);
    }

    public Task ResetYawAsync(CancellationToken cancellationToken)
    {
        var resetYaw = new ResetYaw();
        return _driver.SendAsync(resetYaw.ToMessage(), cancellationToken);
    }

    public Task DriveWithHeadingAsync(byte motorSpeed, UnitsNet.Angle heading, DriveFlags flags,
        CancellationToken cancellationToken)
    {
        var driveWithHeading = new DriveWithHeading(motorSpeed, heading, flags);
        return _driver.SendAsync(driveWithHeading.ToMessage(), cancellationToken);
    }

    public Task DriveAsTankAsync(UnitsNet.Speed leftThreadSpeed, UnitsNet.Speed rightThreadSpeed,
        CancellationToken cancellationToken)
    {
        var driveAsTank = new DriveAsTank(leftThreadSpeed, rightThreadSpeed);
        return _driver.SendAsync(driveAsTank.ToMessage(), cancellationToken);
    }

    public Task DriveAsTankNormalizedAsync(sbyte leftThreadSpeed, sbyte rightThreadSpeed,
        CancellationToken cancellationToken)
    {
        var driveAsTank = new DriveAsTankNormalized(leftThreadSpeed, rightThreadSpeed);
        return _driver.SendAsync(driveAsTank.ToMessage(), cancellationToken);
    }

    public Task DriveWithYawAsync(Angle yaw, Speed speed, CancellationToken cancellationToken)
    {
        var driveWithYaw = new DriveWithYaw(yaw, speed);
        return _driver.SendAsync(driveWithYaw.ToMessage(), cancellationToken);
    }

    public Task DriveWithYawNormalizedAsync(sbyte yaw, sbyte speed, CancellationToken cancellationToken)
    {
        var driveWithYawNormalized = new DriveWithYawNormalized(yaw, speed);
        return _driver.SendAsync(driveWithYawNormalized.ToMessage(), cancellationToken);
    }

    public Task DriveToAsync(Length x, Length y, Angle yaw, Speed speed, DriveFlags flags, CancellationToken cancellationToken)
    {
        var driveTo = new DriveTo(x,y,yaw, speed,flags);
        return _driver.SendAsync(driveTo.ToMessage(), cancellationToken);
    }

    public Task DriveToNormalizedAsync(sbyte x, sbyte y, sbyte yaw, sbyte speed, DriveFlags flags, CancellationToken cancellationToken)
    {
        var driveToNormalized = new DriveToNormalized(x, y, yaw, speed, flags);
        return _driver.SendAsync(driveToNormalized.ToMessage(), cancellationToken);
    }

    public Task EnableMotorFaultNotificationsAsync(bool enable,
        CancellationToken cancellationToken)
    {
        var enableMotorFaultNotifications = new EnableMotorFaultNotifications(enable);
        return _driver.SendAsync(enableMotorFaultNotifications.ToMessage(), cancellationToken);
    }

    public Task EnableMotorStallNotificationsAsync(bool enable,
        CancellationToken cancellationToken)
    {
        var enableMotorStallNotifications = new EnableMotorStallNotifications(enable);
        return _driver.SendAsync(enableMotorStallNotifications.ToMessage(), cancellationToken);
    }

    public async Task<GetMotorFaultState> GetMotorFaultStateAsync(CancellationToken cancellationToken)
    {
        var getMotorFaultState = new Commands.DriveDevice.GetMotorFaultState();
        var response = await _driver.SendRequestAsync(getMotorFaultState.ToMessage(), cancellationToken);
        return new GetMotorFaultState(response);
    }

    public IDisposable Subscribe(Action<ActiveControllerHasStopped> onNext)
    {
        return _notificationManager.Subscribe(onNext);
    }

    public IDisposable Subscribe(Action<ReachedTargetXYPosition> onNext)
    {
        return _notificationManager.Subscribe(onNext);
    }

    public void Dispose()
    {
        _notificationManager.Dispose();
    }


       
}