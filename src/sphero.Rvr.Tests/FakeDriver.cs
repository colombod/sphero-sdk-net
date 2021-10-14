using sphero.Rvr.Protocol;
using System;
using System.Collections.Generic;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;

namespace sphero.Rvr.Tests
{
    public class FakeDriver : IDriver
    {
        public readonly Subject<Message> MessagesSubject = new();
        public readonly List<byte[]> MessagesSent = new();

        public Task SendAsync(Message message, CancellationToken cancellationToken)
        {
            MessagesSent.Add(message.ToRawBytes());
            return Task.CompletedTask;
        }

        public IDisposable Subscribe(IObserver<Message> observer)
        {
            return MessagesSubject.Subscribe(observer);
        }
    }
}
