using sphero.Rvr.Commands.SensorDevice;
using sphero.Rvr.Notifications.SensorDevice;
using sphero.Rvr.Responses.SensorDevice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace sphero.Rvr.Devices
{
    public class SensorDevice
    {
        private readonly IDriver _driver;
        private readonly NotificationManager _notificationManager;
        private readonly StreamingService _streamingService;

        public SensorDevice(IDriver driver)
        {
            _driver = driver ?? throw new ArgumentNullException(nameof(driver));
            _notificationManager = new NotificationManager(_driver);
            _streamingService = new StreamingService(_driver);
        }

        public Task EnableGyroMaxNotificationsAsync(bool enable, CancellationToken cancellationToken)
        {
            var enableGyroMaxNotifications = new EnableGyroMaxNotifications(enable);
            return _driver.SendAsync(enableGyroMaxNotifications.ToMessage(), cancellationToken);
        }

        public Task ResetLocatorXAndYAsync(CancellationToken cancellationToken)
        {
            var resetLocatorXAndY = new ResetLocatorXAndY();
            return _driver.SendAsync(resetLocatorXAndY.ToMessage(), cancellationToken);
        }

        public Task SetLocatorFlagsAsync(LocatorFlag locatorFlag, CancellationToken cancellationToken)
        {
            var setLocatorFlags = new SetLocatorFlags(locatorFlag);
            return _driver.SendAsync(setLocatorFlags.ToMessage(), cancellationToken);
        }

        public async Task<BotToBotInfraredReadings> GetBotToBotInfraredReadingsAsync(
            CancellationToken cancellationToken)
        {
            var getBotToBotInfraredReadings = new GetBotToBotInfraredReadings();
            var response = await _driver.SendRequestAsync(getBotToBotInfraredReadings.ToMessage(), cancellationToken);
            return new BotToBotInfraredReadings(response);
        }

        public async Task<RgbcSensorValues> GetRgbcSensorValuesAsync(CancellationToken cancellationToken)
        {
            var getRgbcSensorValues = new GetRgbcSensorValues();
            var response = await _driver.SendRequestAsync(getRgbcSensorValues.ToMessage(), cancellationToken);
            return new RgbcSensorValues(response);
        }

        public Task StartRobotToRobotInfraredBroadcastingAsync(byte nearCode, byte farCode, CancellationToken cancellationToken)
        {
            var startRobotToRobotInfraredBroadcasting = new StartRobotToRobotInfraredBroadcasting(nearCode, farCode);
            return _driver.SendAsync(startRobotToRobotInfraredBroadcasting.ToMessage(), cancellationToken);
        }

        public Task StopRobotToRobotInfraredBroadcastingAsync(CancellationToken cancellationToken)
        {
            var stopRobotToRobotInfraredBroadcasting = new StopRobotToRobotInfraredBroadcasting();
            return _driver.SendAsync(stopRobotToRobotInfraredBroadcasting.ToMessage(), cancellationToken);
        }

        public Task StartRobotToRobotInfraredFollowingAsync(byte nearCode, byte farCode, CancellationToken cancellationToken)
        {
            var startRobotToRobotInfraredFollowing = new StartRobotToRobotInfraredFollowing(nearCode, farCode);
            return _driver.SendAsync(startRobotToRobotInfraredFollowing.ToMessage(), cancellationToken);
        }

        public Task StopRobotToRobotInfraredFollowingAsync(CancellationToken cancellationToken)
        {
            var stopRobotToRobotInfraredFollowing = new StopRobotToRobotInfraredFollowing();
            return _driver.SendAsync(stopRobotToRobotInfraredFollowing.ToMessage(), cancellationToken);
        }

        public Task StartRobotToRobotInfraredEvadingAsync(byte nearCode, byte farCode, CancellationToken cancellationToken)
        {
            var startRobotToRobotInfraredEvading = new StartRobotToRobotInfraredEvading(nearCode, farCode);
            return _driver.SendAsync(startRobotToRobotInfraredEvading.ToMessage(), cancellationToken);
        }

        public Task StopRobotToRobotInfraredEvadingAsync(CancellationToken cancellationToken)
        {
            var stopRobotToRobotInfraredEvading = new StopRobotToRobotInfraredEvading();
            return _driver.SendAsync(stopRobotToRobotInfraredEvading.ToMessage(), cancellationToken);
        }

        public async Task<AmbientLightSensorValue> GetAmbientLightSensorValueAsync(CancellationToken cancellationToken)
        {
            var getAmbientLightSensorValue = new GetAmbientLightSensorValue();
            var response = await _driver.SendRequestAsync(getAmbientLightSensorValue.ToMessage(), cancellationToken);
            return new AmbientLightSensorValue(response);
        }

        public Task EnableColorDetectionNotificationsAsync(bool enable, TimeSpan interval, byte minimumConfidenceThreshold, CancellationToken cancellationToken)
        {
            var enableColorDetectionNotifications = new EnableColorDetectionNotifications(enable, interval, minimumConfidenceThreshold);
            return _driver.SendAsync(enableColorDetectionNotifications.ToMessage(), cancellationToken);
        }

        public Task EnableColorDetectionAsync(bool enable, CancellationToken cancellationToken)
        {
            var enableColorDetection = new EnableColorDetection(enable);
            return _driver.SendAsync(enableColorDetection.ToMessage(), cancellationToken);
        }

        public Task GetCurrentDetectedColorReadingAsync(CancellationToken cancellationToken)
        {
            var getCurrentDetectedColorReading = new GetCurrentDetectedColorReading();
            return _driver.SendAsync(getCurrentDetectedColorReading.ToMessage(), cancellationToken);
        }

        public Task ConfigureStreamingServiceAsync(byte targetId, byte token, byte[] configuration, CancellationToken cancellationToken)
        {
            var startStreamingService = new ConfigureStreamingService(targetId, token, configuration);
            return _driver.SendAsync(startStreamingService.ToMessage(), cancellationToken);
        }

        public Task StartStreamingServiceAsync(byte targetId, TimeSpan interval, CancellationToken cancellationToken)
        {
            var startStreamingService = new StartStreamingService(targetId, interval);
            return _driver.SendAsync(startStreamingService.ToMessage(), cancellationToken);
        }

        public Task StopStreamingServiceAsync(byte targetId, CancellationToken cancellationToken)
        {
            var stopStreamingService = new StopStreamingService(targetId);
            return _driver.SendAsync(stopStreamingService.ToMessage(), cancellationToken);
        }

        public Task ClearStreamingServiceAsync(byte targetId, CancellationToken cancellationToken)
        {
            var clearStreamingService = new ClearStreamingService(targetId);
            return _driver.SendAsync(clearStreamingService.ToMessage(), cancellationToken);
        }

        public Task EnableRobotInfraredMessageNotificationsAsync(bool enable, CancellationToken cancellationToken)
        {
            var enableRobotInfraredMessageNotifications = new EnableRobotInfraredMessageNotifications(enable);
            return _driver.SendAsync(enableRobotInfraredMessageNotifications.ToMessage(), cancellationToken);
        }

        public Task SendInfraredMessageAsync(byte infraredCode, byte frontStrength, byte leftStrength, byte rightStrength, byte rearStrength, CancellationToken cancellationToken)
        {
            var sendInfraredMessage = new SendInfraredMessage(infraredCode, frontStrength, leftStrength, rightStrength, rearStrength);
            return _driver.SendAsync(sendInfraredMessage.ToMessage(), cancellationToken);
        }

        public async Task<MotorTemperature> GetMotorTemperatureAsync(MotorIndex motorIndex,
            CancellationToken cancellationToken)
        {
            var getMotorTemperature = new GetMotorTemperature(motorIndex);
            var response = await _driver.SendRequestAsync(getMotorTemperature.ToMessage(), cancellationToken);
            return new MotorTemperature(response);
        }

        public async Task<MotorThermalProtectionStatus> GetMotorThermalProtectionStatusAsync(CancellationToken cancellationToken)
        {
            var getMotorThermalProtectionStatus = new GetMotorThermalProtectionStatus();
            var response = await _driver.SendRequestAsync(getMotorThermalProtectionStatus.ToMessage(), cancellationToken);
            return new MotorThermalProtectionStatus(response);
        }

        public Task EnableMotorThermalProtectionStatusNotificationsAsync(bool enable, CancellationToken cancellationToken)
        {
            var enableMotorThermalProtectionStatusNotifications = new EnableMotorThermalProtectionStatusNotifications(enable);
            return _driver.SendAsync(enableMotorThermalProtectionStatusNotifications.ToMessage(), cancellationToken);
        }

        public async Task StartStreamingServiceAsync(TimeSpan interval, CancellationToken cancellationToken)
        {
            var startNordic = new StartStreamingService(1, interval);
            var startSt = new StartStreamingService(2, interval);
            await _driver.SendAsync(startNordic.ToMessage(), cancellationToken);
            await _driver.SendAsync(startSt.ToMessage(), cancellationToken);
        }

        public async Task StopStreamingServiceAsync(CancellationToken cancellationToken)
        {
            var stopNordic = new StopStreamingService(1);
            var stopSt = new StopStreamingService(2);

            await _driver.SendAsync(stopNordic.ToMessage(), cancellationToken);
            await _driver.SendAsync(stopSt.ToMessage(), cancellationToken);
        }

        public async Task ConfigureSensorStreamingAsync(IReadOnlyCollection<SensorId> sensors,
            CancellationToken cancellationToken)
        {
            var configurationMessages = _streamingService.Configure(sensors);

            if (sensors.Contains(SensorId.ColorDetection))
            {
                await EnableColorDetectionAsync(true, cancellationToken);
            }

            foreach (var configureStreamingService in configurationMessages)
            {
                await _driver.SendAsync(configureStreamingService.ToMessage(), cancellationToken);
            }
        }

        public IDisposable SubscribeToColorDetectionNotifications(Action<ColorDetectionNotification> onNotification)
        {
            return _notificationManager.Subscribe(onNotification);
        }

        public IDisposable SubscribeToStream(Action<AccelerometerNotification> onNotification)
        {
            return _streamingService.Subscribe(onNotification);
        }

        public IDisposable SubscribeToStream(Action<AmbientLightNotification> onNotification)
        {
            return _streamingService.Subscribe(onNotification);
        }

        public IDisposable SubscribeToStream(Action<AttitudeNotification> onNotification)
        {
            return _streamingService.Subscribe(onNotification);
        }

        public IDisposable SubscribeToStream(Action<ColorDetectionNotification> onNotification)
        {
            return _streamingService.Subscribe(onNotification);
        }

        public IDisposable SubscribeToStream(Action<CoreTimeLowerNotification> onNotification)
        {
            return _streamingService.Subscribe(onNotification);
        }

        public IDisposable SubscribeToStream(Action<CoreTimeUpperNotification> onNotification)
        {
            return _streamingService.Subscribe(onNotification);
        }

        public IDisposable SubscribeToStream(Action<GyroscopeNotification> onNotification)
        {
            return _streamingService.Subscribe(onNotification);
        }

        public IDisposable SubscribeToStream(Action<LocatorNotification> onNotification)
        {
            return _streamingService.Subscribe(onNotification);
        }

        public IDisposable SubscribeToStream(Action<QuaternionNotification> onNotification)
        {
            return _streamingService.Subscribe(onNotification);
        }

        public IDisposable SubscribeToStream(Action<SpeedNotification> onNotification)
        {
            return _streamingService.Subscribe(onNotification);
        }

        public IDisposable SubscribeToStream(Action<VelocityNotification> onNotification)
        {
            return _streamingService.Subscribe(onNotification);
        }

    }
}