using sphero.Rvr.Commands.SensorDevice;
using sphero.Rvr.Notifications;
using sphero.Rvr.Notifications.SensorDevice;
using sphero.Rvr.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Pocket;
using sphero.Rvr.Commands.SensorDevice;
using sphero.Rvr.Notifications;
using sphero.Rvr.Notifications.SensorDevice;
using sphero.Rvr.Protocol;

namespace sphero.Rvr.Devices
{
    public class StreamingService
    {
        private readonly IDriver _driver;
        private IReadOnlyCollection<SensorId> _activeSensors;
        private Dictionary<byte, Dictionary<byte, Slot>> _processorToSlots = new();
        private Dictionary<SensorId, Subject<Event>> _channels;

        public StreamingService(IDriver driver)
        {
            _driver = driver ?? throw new ArgumentNullException(nameof(driver));

            _driver.Where(m => m.Header.CommandId == 61 && m.Header.DeviceId == DeviceIdentifier.Sensor)
                .Select(m => new StreamingServiceDataNotification(m))
                .Subscribe(OnStreamingData);
        }

        private void OnStreamingData(StreamingServiceDataNotification streamingServiceDataNotification)
        {
            using (var operation = Logger.Log.OnEnterAndExit())
            {
                if (_processorToSlots.TryGetValue(streamingServiceDataNotification.SourceId, out var processorSlots))
                {
                    if (processorSlots.TryGetValue(streamingServiceDataNotification.Token, out var slot))
                    {
                        // unpack
                        var position = 0;
                        foreach (var (sensorId, _) in slot.Sensors)
                        {
                            Subject<Event> channel;
                            Event notification;
                            switch (sensorId)
                            {
                                case SensorId.Quaternion:
                                    channel = _channels[sensorId];
                                    var qn = new QuaternionNotification();
                                    position += qn.FromRawData(streamingServiceDataNotification.SensorData, position);
                                    notification = qn;
                                    break;
                                case SensorId.Attitude:
                                    channel = _channels[sensorId];
                                    var an = new AttitudeNotification();
                                    position += an.FromRawData(streamingServiceDataNotification.SensorData, position);
                                    notification = an;
                                    break;
                                case SensorId.Accelerometer:
                                    channel = _channels[sensorId];
                                    var axn = new AccelerometerNotification();
                                    position += axn.FromRawData(streamingServiceDataNotification.SensorData, position);
                                    notification = axn;
                                    break;
                                case SensorId.ColorDetection:
                                    channel = _channels[sensorId];
                                    var cdn = new ColorDetectionNotification();
                                    position += cdn.FromRawData(streamingServiceDataNotification.SensorData, position);
                                    notification = cdn;
                                    break;
                                case SensorId.Gyroscope:
                                    channel = _channels[sensorId];
                                    var gn = new GyroscopeNotification();
                                    position += gn.FromRawData(streamingServiceDataNotification.SensorData, position);
                                    notification = gn;
                                    break;
                                case SensorId.Locator:
                                    channel = _channels[sensorId];
                                    var ln = new LocatorNotification();
                                    position += ln.FromRawData(streamingServiceDataNotification.SensorData, position);
                                    notification = ln;
                                    break;
                                case SensorId.Velocity:
                                    channel = _channels[sensorId];
                                    var vn = new VelocityNotification();
                                    position += vn.FromRawData(streamingServiceDataNotification.SensorData, position);
                                    notification = vn;
                                    break;
                                case SensorId.Speed:
                                    channel = _channels[sensorId];
                                    var sn = new SpeedNotification();
                                    position += sn.FromRawData(streamingServiceDataNotification.SensorData, position);
                                    notification = sn;
                                    break;
                                case SensorId.CoreTimeLower:
                                    channel = _channels[sensorId];
                                    var ctln = new CoreTimeLowerNotification();
                                    position += ctln.FromRawData(streamingServiceDataNotification.SensorData, position);
                                    notification = ctln;
                                    break;
                                case SensorId.CoreTimeUpper:
                                    channel = _channels[sensorId];
                                    var ctun = new CoreTimeUpperNotification();
                                    position += ctun.FromRawData(streamingServiceDataNotification.SensorData, position);
                                    notification = ctun;
                                    break;
                                case SensorId.AmbientLight:
                                    channel = _channels[sensorId];
                                    var aln = new AmbientLightNotification();
                                    position += aln.FromRawData(streamingServiceDataNotification.SensorData, position);
                                    notification = aln;
                                    break;
                                default:
                                    throw new ArgumentOutOfRangeException();
                            }

                            channel.OnNext(notification);
                        }
                    }
                    else
                    {
                        operation.Warning(
                            $"Cannot find slot {streamingServiceDataNotification.Token} for processor {streamingServiceDataNotification.SourceId} in current configuration.");
                    }
                }
                else
                {
                    operation.Warning(
                        $"Cannot find processor {streamingServiceDataNotification.SourceId} in current configuration.");
                }
            }
        }

        public IEnumerable<ConfigureStreamingService> Configure(IReadOnlyCollection<SensorId> sensors)
        {
            _activeSensors = sensors;
            _processorToSlots = new Dictionary<byte, Dictionary<byte, Slot>>();
            _channels = new Dictionary<SensorId, Subject<Event>>();
            foreach (var sensorId in sensors)
            {
                _channels[sensorId] = new Subject<Event>();
                var (processorId, slotId) = SensorToProcessorAndSlotId(sensorId);

                var slot = GetOrCreateSlot(processorId, slotId);

                slot.Sensors.Add(new(sensorId, SensorToDataSize(sensorId)));
            }

            var messages = new List<ConfigureStreamingService>();
            foreach (var (processorId, slots) in _processorToSlots)
            {
                foreach (var (slotId, slot) in slots)
                {
                    var rawConfiguration = slot.Sensors.Select(s => new byte[] { 0x00, (byte)s.sensorId, (byte)s.dataSize }).SelectMany(raw => raw).ToArray();
                    messages.Add(new ConfigureStreamingService(processorId, slotId, rawConfiguration));
                }
            }

            return messages;
        }

        private Slot GetOrCreateSlot(byte processorId, byte slotId)
        {
            if (!_processorToSlots.TryGetValue(processorId, out var storage))
            {
                storage = new Dictionary<byte, Slot>();
                _processorToSlots[processorId] = storage;
            }

            if (!storage.TryGetValue(slotId, out var slot))
            {
                slot = new Slot(processorId, slotId, new List<(SensorId sensorId, SensorDataSize dataSize)>());
                storage[slotId] = slot;
            }

            return slot;
        }

        private record Slot(byte ProcessorId, byte SlotId, List<(SensorId sensorId, SensorDataSize dataSize)> Sensors);

        private SensorDataSize SensorToDataSize(SensorId sensorId)
        {
            switch (sensorId)
            {
                case SensorId.Quaternion:
                    return SensorDataSize.ThirtyTwoBit;
                case SensorId.Attitude:
                case SensorId.Accelerometer:
                    return SensorDataSize.SixteenBit;
                case SensorId.ColorDetection:
                    return SensorDataSize.EightBit;
                case SensorId.Gyroscope:
                    return SensorDataSize.SixteenBit;
                case SensorId.Locator:
                case SensorId.Velocity:
                case SensorId.Speed:
                    return SensorDataSize.ThirtyTwoBit;
                case SensorId.CoreTimeLower:
                case SensorId.CoreTimeUpper:
                    return SensorDataSize.ThirtyTwoBit;
                case SensorId.AmbientLight:
                    return SensorDataSize.SixteenBit;
                default:
                    throw new ArgumentOutOfRangeException(nameof(sensorId), sensorId, null);
            }
        }
        private (byte processorId, byte slotId) SensorToProcessorAndSlotId(SensorId sensorId)
        {
            switch (sensorId)
            {
                case SensorId.Quaternion:
                case SensorId.Gyroscope:
                case SensorId.Attitude:
                case SensorId.Accelerometer:
                    return (2, 1);
                case SensorId.Locator:
                case SensorId.Velocity:
                case SensorId.Speed:
                    return (2, 2);
                case SensorId.ColorDetection:
                    return (1, 1);
                case SensorId.CoreTimeLower:
                case SensorId.CoreTimeUpper:
                    return (1, 2);
                case SensorId.AmbientLight:
                    return (1, 3);
                default:
                    throw new ArgumentOutOfRangeException(nameof(sensorId), sensorId, null);
            }
        }

        public IDisposable Subscribe<TNotification>(Action<TNotification> onNext) where TNotification : Event
        {
            if (NotificationExtensions.TryGetSensorId(typeof(TNotification), out var key) && _channels.TryGetValue(key, out var channel))
            {
                return channel.OfType<TNotification>().Subscribe(onNext);
            }

            throw new InvalidOperationException(
                $"Notifications of type {typeof(TNotification).FullName} are not supported in current configuration");
        }
    }
}