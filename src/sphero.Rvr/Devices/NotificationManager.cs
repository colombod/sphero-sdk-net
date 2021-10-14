using System;
using System.Collections.Concurrent;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using sphero.Rvr.Notifications;
using sphero.Rvr.Protocol;

namespace sphero.Rvr.Devices
{
    internal class NotificationManager : IDisposable
    {
        private readonly ConcurrentDictionary<(byte SourceId, byte DeviceId, byte CommandId), ISubject<Event>>
            _eventChannels = new();
        private readonly CompositeDisposable _disposables = new();

        public NotificationManager(Driver driver)
        {
            if (driver == null)
            {
                throw new ArgumentNullException(nameof(driver));
            }

            _disposables.Add(driver.Subscribe(ProcessMessage));
        }

        private void ProcessMessage(Message message)
        {

            (byte SourceId, byte DeviceId, byte CommandId) key = new(message.Header.SourceId, (byte)message.Header.DeviceId,
                message.Header.CommandId);
            if (_eventChannels.TryGetValue(key, out var channel))
            {
                channel.OnNext(message.ToNotification());
            }
        }

        public IDisposable Subscribe<TNotification>(Action<TNotification> onNext) where TNotification: Event
        {
            if (NotificationExtensions.TryGetKey(typeof(TNotification), out var key))
            {
                var channel = _eventChannels.GetOrAdd(key, _ => new Subject<Event>());
                return channel.OfType<TNotification>().Subscribe(onNext);
            }

            throw new InvalidOperationException(
                $"Notifications of type {typeof(TNotification).FullName} are not supported");
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}
