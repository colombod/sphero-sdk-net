﻿using Pocket;
using sphero.Rvr.Devices;
using sphero.Rvr.Notifications.SensorDevice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reactive.Subjects;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnitsNet;
using CompositeDisposable = System.Reactive.Disposables.CompositeDisposable;

namespace sphero.Rvr
{
    public class Rover : IDisposable
    {
        private readonly CompositeDisposable _disposables = new();
        private readonly IDriver _driver;
        private readonly SensorDevice _sensorDevice;
        private readonly DriveDevice _driveDevice;
        private readonly IoDevice _ioDevice;
        private readonly PowerDevice _powerDevice;
        private readonly ConnectionDevice _connectionDevice;
        private readonly SystemInfoDevice _systemInfoDevice;
        private LoggerSubscription _logSubscription;

        private Action<(byte LogLevel, DateTime TimestampUtc,
            Func<(string Message, (string Name, object Value)[] Properties)> Evaluate, Exception Exception, string
            OperationName, string Category, (string Id, bool IsStart, bool IsEnd, bool? IsSuccessful, TimeSpan? Duration
            ) Operation)> _logonEntryPosted;

        private readonly ISubject<Quaternion> _quaternionStream = new ReplaySubject<Quaternion>(1);
        private readonly ISubject<Attitude> _attitudeStream = new ReplaySubject<Attitude>(1);
        private readonly ISubject<Acceleration3D> _accelerationStream = new ReplaySubject<Acceleration3D>(1);
        private readonly ISubject<Illuminance> _ambientLightStream = new ReplaySubject<Illuminance>(1);
        private readonly ISubject<ColorDetection> _colorDetectionStream = new ReplaySubject<ColorDetection>(1);
        private readonly ISubject<RotationalSpeed3D> _gyroscopeStream = new ReplaySubject<RotationalSpeed3D>(1);
        private readonly ISubject<Speed2D> _velocityStream = new ReplaySubject<Speed2D>(1);
        private readonly ISubject<Speed> _speedStream = new ReplaySubject<Speed>(1);
        private readonly ISubject<Length2D> _locatorStream = new ReplaySubject<Length2D>(1);
        private readonly ISubject<uint> _coreTimeLowerStream = new ReplaySubject<uint>(1);
        private readonly ISubject<uint> _coreTimeUpperStream = new ReplaySubject<uint>(1);

        private HashSet<SensorId> _activeSensors = new HashSet<SensorId>();


        public Rover(string serialPort) : this(new Driver(serialPort)) { }

        public Rover(IDriver driver)
        {
            _driver = driver;
            _sensorDevice = new SensorDevice(_driver);
            _driveDevice = new DriveDevice(_driver);
            _ioDevice = new IoDevice(_driver);
            _powerDevice = new PowerDevice(_driver);
            _connectionDevice = new ConnectionDevice(_driver);
            _systemInfoDevice = new SystemInfoDevice(_driver);
        }

        public IObservable<Quaternion> QuaternionStream => _quaternionStream;

        public IObservable<Attitude> AttitudeStream => _attitudeStream;

        public IObservable<Acceleration3D> AccelerationStream => _accelerationStream;

        public IObservable<Illuminance> AmbientLightStream => _ambientLightStream;

        public IObservable<ColorDetection> ColorDetectionStream => _colorDetectionStream;

        public IObservable<RotationalSpeed3D> GyroscopeStream => _gyroscopeStream;

        public IObservable<Speed2D> VelocityStream => _velocityStream;

        public IObservable<Speed> SpeedStream => _speedStream;

        public IObservable<Length2D> LocatorStream => _locatorStream;

        public IObservable<uint> CoreTimeLowerStream => _coreTimeLowerStream;

        public IObservable<uint> CoreTimeUpperStream => _coreTimeUpperStream;

        public void EnableLogging(
            Action<(byte LogLevel, DateTime TimestampUtc,
                Func<(string Message, (string Name, object Value)[] Properties)> Evaluate, Exception Exception, string
                OperationName, string Category, (string Id, bool IsStart, bool IsEnd, bool? IsSuccessful, TimeSpan?
                Duration) Operation)> onEntryPosted = null)
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

        public async Task ConfigureRoverAsync(IReadOnlyCollection<SensorId> sensors, TimeSpan samplingInterval,
            CancellationToken cancellationToken)
        {
            _activeSensors = sensors.ToHashSet();
            await _ioDevice.SetAllLedsOffAsync(cancellationToken);
            await _sensorDevice.ConfigureSensorStreamingAsync(_activeSensors, cancellationToken);
            SetupChannels(_activeSensors);
            await _sensorDevice.StartStreamingServiceAsync(samplingInterval, cancellationToken);
        }

        private void SetupChannels(IEnumerable<SensorId> sensors)
        {
            foreach (var sensorId in sensors)
            {
                switch (sensorId)
                {
                    case SensorId.Quaternion:
                        _disposables.Add(_sensorDevice.SubscribeToStream((QuaternionNotification qn) =>
                            _quaternionStream.OnNext(new Quaternion(qn.X, qn.Y, qn.Z, qn.W))));
                        break;
                    case SensorId.Attitude:
                        _disposables.Add(_sensorDevice.SubscribeToStream((AttitudeNotification an) =>
                            _attitudeStream.OnNext(new Attitude(an.Pitch, an.Roll, an.Yaw))));
                        break;
                    case SensorId.Accelerometer:
                        _disposables.Add(_sensorDevice.SubscribeToStream((AccelerometerNotification acn) =>
                            _accelerationStream.OnNext(new Acceleration3D(acn.X, acn.Y, acn.Z))));
                        break;
                    case SensorId.ColorDetection:
                        _disposables.Add(_sensorDevice.SubscribeToStream((ColorDetectionNotification cdn) =>
                            _colorDetectionStream.OnNext(new ColorDetection(cdn.Color, cdn.Confidence))));
                        break;
                    case SensorId.Gyroscope:
                        _disposables.Add(_sensorDevice.SubscribeToStream((GyroscopeNotification gyn) =>
                            _gyroscopeStream.OnNext(new RotationalSpeed3D(gyn.X, gyn.Y, gyn.Z))));
                        break;
                    case SensorId.Locator:
                        _disposables.Add(_sensorDevice.SubscribeToStream((LocatorNotification ln) =>
                            _locatorStream.OnNext(new Length2D(ln.X, ln.Y))));
                        break;
                    case SensorId.Velocity:
                        _disposables.Add(_sensorDevice.SubscribeToStream((VelocityNotification vn) =>
                            _velocityStream.OnNext(new Speed2D(vn.X, vn.Y))));
                        break;
                    case SensorId.Speed:
                        _disposables.Add(_sensorDevice.SubscribeToStream((SpeedNotification sn) =>
                            _speedStream.OnNext(sn.Speed)));
                        break;
                    case SensorId.CoreTimeLower:
                        _disposables.Add(_sensorDevice.SubscribeToStream((CoreTimeLowerNotification ctln) =>
                            _coreTimeLowerStream.OnNext(ctln.Time)));
                        break;
                    case SensorId.CoreTimeUpper:
                        _disposables.Add(_sensorDevice.SubscribeToStream((CoreTimeUpperNotification ctun) =>
                            _coreTimeUpperStream.OnNext(ctun.Time)));
                        break;
                    case SensorId.AmbientLight:
                        _disposables.Add(_sensorDevice.SubscribeToStream((AmbientLightNotification an) =>
                            _ambientLightStream.OnNext(an.Light)));
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public Task ConfigureRoverAsync(CancellationToken cancellationToken)
        {
            var allSensors = new[]
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
            };
            return ConfigureRoverAsync(allSensors, TimeSpan.FromSeconds(0.1), cancellationToken);
        }

        public async Task SetLedAsync(IDictionary<Led, Color> ledColors, CancellationToken cancellationToken)
        {
            uint ledMask = 0;
            var brightness = new List<byte>();
            foreach (var (led, color) in ledColors.OrderBy(e => ((uint)e.Key)))
            {
                ledMask |= (uint)led;

                switch (led)
                {
                    case Led.UndercarriageWhite:
                        brightness.Add(color.ToGreyScale());
                        break;
                    default:
                        brightness.AddRange(color.ToRawBytes());
                        break;
                }
            }

            await _ioDevice.SetLedsAsync((LedBitMask)ledMask, brightness.ToArray(), cancellationToken);
        }

        public async Task SetLedAsync(Led led, Color color, CancellationToken cancellationToken)
        {
            var bytes = led == Led.UndercarriageWhite ? new byte[] { color.ToGreyScale() } : color.ToRawBytes();

            await _ioDevice.SetLedsAsync((LedBitMask)led, bytes, cancellationToken);
        }

        public async Task SetAllLedAsync(Color color, CancellationToken cancellationToken)
        {
            await _ioDevice.SetAllLedsAsync(color, cancellationToken);
        }

        public async Task SetAllLedOffAsync(CancellationToken cancellationToken)
        {
            await _ioDevice.SetAllLedsOffAsync(cancellationToken);
        }

        public void Dispose()
        {
            _disposables?.Dispose();
            _driver.Dispose();
            _logSubscription?.Dispose();
        }

        public async Task<SystemInfo> WakeAsync(CancellationToken cancellationToken)
        {
            await _powerDevice.WakeAsync(cancellationToken);
            return await GetInfoAsync(cancellationToken);
        }

        public Task SleepAsync(CancellationToken cancellationToken)
        {
            return _powerDevice.SleepAsync(cancellationToken);
        }

        public async Task<SystemInfo> GetInfoAsync(CancellationToken cancellationToken)
        {
            var boardRevision = await _systemInfoDevice.GetBoardRevisionAsync(cancellationToken);
            var p1Name = await _systemInfoDevice.GetProcessorNameAsync(1, cancellationToken);
            var p1FwVersion = await _systemInfoDevice.GetFirmwareVersionForNordicProcessorAsync(cancellationToken);

            var p2Name = await _systemInfoDevice.GetProcessorNameAsync(1, cancellationToken);
            var p2FwVersion = await _systemInfoDevice.GetFirmwareVersionForSTProcessorAsync(cancellationToken);

            return new SystemInfo(boardRevision.Revision, new ProcessorInfo[] { new(p1Name.Name, p1FwVersion.Version), new(p2Name.Name, p2FwVersion.Version) });
        }

        public Task DriveAsync(byte speed, Angle heading, CancellationToken cancellationToken)
        {
            return _driveDevice.DriveWithHeadingAsync(speed, heading,DriveFlags.NoFlags, cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return _driveDevice.DriveWithHeadingAsync(0, Angle.Zero, DriveFlags.NoFlags, cancellationToken);
        }
    }

    public record ProcessorInfo(string Name, Version FirmwareVersion);

    public record SystemInfo(byte BoardRevision, ProcessorInfo[] Processors)
    {
        protected virtual bool PrintMembers(StringBuilder builder)
        {
            builder.Append($"{nameof(BoardRevision)} = {BoardRevision}");
            if (Processors?.Length > 0)
            {
                builder.AppendJoin(", ", Processors.Select(p => p.ToString()));
            }
            return true;
        }
    }

    public record Attitude(Angle Pitch, Angle Roll, Angle Yaw);

    public record Acceleration3D(Acceleration X, Acceleration Y, Acceleration Z);

    public record Speed2D(Speed X, Speed Y);

    public record Length2D(Length X, Length Y);

    public record RotationalSpeed3D(RotationalSpeed X, RotationalSpeed Y, RotationalSpeed Z);
    public record ColorDetection(Color Color, float Confidence);
}