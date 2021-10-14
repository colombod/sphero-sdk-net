using System;
using System.Reactive.Disposables;
using sphero.Rvr.Devices;

namespace sphero.Rvr
{
    public class Rover : IDisposable
    {
        private readonly CompositeDisposable _disposables = new();
        private readonly Driver _driver;
        private readonly SensorDevice _sensorDevice;
        private readonly DriveDevice _driveDevice;
        private readonly IoDevice _ioDevice;
        private readonly PowerDevice _powerDevice;
        private readonly ConnectionDevice _connectionDevice;
        private readonly SystemInfoDevice _systemInfoDevice;

        public Rover(string serialPort)
        {
            _driver = new Driver(serialPort);
            _sensorDevice = new SensorDevice(_driver);
            _driveDevice = new DriveDevice(_driver);
            _ioDevice = new IoDevice(_driver);
            _powerDevice = new PowerDevice(_driver);
            _connectionDevice = new ConnectionDevice(_driver);
            _systemInfoDevice = new SystemInfoDevice(_driver);
        }


        public void Dispose()
        {
            _disposables?.Dispose();
            _driver.Dispose();
        }
    }
}
