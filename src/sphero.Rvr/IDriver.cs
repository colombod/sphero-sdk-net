using sphero.Rvr.Protocol;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace sphero.Rvr;

public interface IDriver : IObservable<Message>, IDisposable
{
    Task SendAsync(Message message, CancellationToken cancellationToken);
}