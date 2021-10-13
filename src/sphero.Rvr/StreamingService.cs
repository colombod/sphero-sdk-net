using System;
using System.Collections.Generic;
using System.Linq;
using sphero.Rvr.Commands.SensorDevice;
using sphero.Rvr.Commands.SystemInfoDevice;
using sphero.Rvr.Notifications.SensorDevice;

namespace sphero.Rvr
{
    internal class StreamingService
    {
        private readonly NotificationManager _notificationManager;
        private IReadOnlyCollection<SensorId> _activeSensors;
        private Dictionary<byte, Dictionary<byte, Slot>> _processorToSlots = new();

        public StreamingService(NotificationManager notificationManager)
        {
            _notificationManager = notificationManager ?? throw new ArgumentNullException(nameof(notificationManager));
            notificationManager.Subscribe((Action<StreamingServiceDataNotification>)OnStreamingData);
        }

        private void OnStreamingData(StreamingServiceDataNotification streamingServiceDataNotification)
        {
            if (_processorToSlots.TryGetValue(streamingServiceDataNotification.SourceId, out var processorSlots))
            {
                if (processorSlots.TryGetValue(streamingServiceDataNotification.Token, out var slot))
                {
                    // unpack
                }
                else
                {
                    Console.WriteLine($"Cannot find slot {streamingServiceDataNotification.Token} for processor {streamingServiceDataNotification.SourceId} in current configuration.");
                }
            }
            else
            {
                Console.WriteLine($"Cannot find processor {streamingServiceDataNotification.SourceId} in current configuration.");
            }
        }

        public IEnumerable<ConfigureStreamingService> Configure(IReadOnlyCollection<SensorId> sensors)
        {
            _activeSensors = sensors;
            _processorToSlots = new Dictionary<byte, Dictionary<byte, Slot>>();
            foreach (var sensorId in sensors)
            {
                var (processorId, slotId) = SensorToProcessorAndSlotId(sensorId);
                var sensorDataSize = SensorToDataSize(sensorId);

                var slot = GetOrCreateSlot(processorId, slotId);

                slot.Sensors.Add(new (sensorId, SensorToDataSize(sensorId)));
            }

            var messages = new List<ConfigureStreamingService>();
            foreach (var (processorId, slots) in _processorToSlots)
            {
                foreach (var (slotId, slot) in slots)
                {
                    var rawConfiguration = slot.Sensors.Select(s => new byte[] { 0x00,(byte)s.sensorId, (byte) s.dataSize}).SelectMany(raw => raw).ToArray();
                    messages.Add( new ConfigureStreamingService(processorId, slotId,rawConfiguration));
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
                    return (1,1);
                case SensorId.Locator:
                case SensorId.Velocity:
                case SensorId.Speed:
                    return (1,2);
                case SensorId.ColorDetection:
                    return (2,1);
                case SensorId.CoreTimeLower:
                case SensorId.CoreTimeUpper:
                    return (2,2);
                case SensorId.AmbientLight:
                    return (2,3);
                default:
                    throw new ArgumentOutOfRangeException(nameof(sensorId), sensorId, null);
            }
        }
    }
}