using System;
using System.Threading;
using System.Threading.Tasks;
using sphero.Rvr.Commands.DriveDevice;
using GetMotorFaultState = sphero.Rvr.Responses.DriveDevice.GetMotorFaultState;

namespace sphero.Rvr.Devices
{
    public class DriveDevice
    {
        public const DeviceIdentifier DeviceId = DeviceIdentifier.Drive;

        private readonly Driver _driver;

        public DriveDevice(Driver driver)
        {
            _driver = driver ?? throw new ArgumentNullException(nameof(driver));
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
    }
}