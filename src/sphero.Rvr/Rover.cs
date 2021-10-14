using System;
using Pocket;
using sphero.Rvr.Devices;
using CompositeDisposable = System.Reactive.Disposables.CompositeDisposable;

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
        private LoggerSubscription _logSubscription;
        private Action<(byte LogLevel, DateTime TimestampUtc, Func<(string Message, (string Name, object Value)[] Properties)> Evaluate, Exception Exception, string OperationName, string Category, (string Id, bool IsStart, bool IsEnd, bool? IsSuccessful, TimeSpan? Duration) Operation)> _logonEntryPosted;

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

        public void EnableLogging(Action<(byte LogLevel, DateTime TimestampUtc, Func<(string Message, (string Name, object Value)[] Properties)> Evaluate, Exception Exception, string OperationName, string Category, (string Id, bool IsStart, bool IsEnd, bool? IsSuccessful, TimeSpan? Duration) Operation)> onEntryPosted = null)
        {
            if (_logonEntryPosted is null)
            {
                _logonEntryPosted = onEntryPosted ?? (i =>
                {
                    i.Operation.Id = "";
                    Console.WriteLine(i.ToLogString());
                });
            }
            else if (onEntryPosted is not null)
            {
                _logonEntryPosted = onEntryPosted;
            }

            _logSubscription?.Dispose();

            _logSubscription = LogEvents.Subscribe(_logonEntryPosted, new[]
            {
                typeof(Rover).Assembly
            });
        }

        public void DisableLogging()
        {
            _logSubscription?.Dispose();
            _logSubscription = null;

        }


        public void Dispose()
        {
            _disposables?.Dispose();
            _driver.Dispose();
            _logSubscription?.Dispose();
        }
    }
}
