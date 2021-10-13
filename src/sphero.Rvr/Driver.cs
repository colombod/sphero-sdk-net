using System;
using System.Buffers;
using System.IO.Pipelines;
using System.IO.Ports;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;
using sphero.Rvr.Notifications.PowerDevice;
using sphero.Rvr.Protocol;

namespace sphero.Rvr
{
    public class Driver : IDisposable, IObservable<Message>
    {
        private readonly SerialPort _serialPort;
        private readonly Pipe _pipe;
        private readonly CompositeDisposable _disposables = new();
        private readonly Subject<Message> _messageChannel = new();

        public Driver(string serialPort)
        {
            _serialPort = new SerialPort(serialPort, 115200);
            _serialPort.Open();
            _serialPort.DataReceived += _serialPort_DataReceived;
            _pipe = new Pipe();
            var cs = new CancellationTokenSource();
            Task.Run(() =>
            {
                ReadMessages(cs.Token);
            }, cs.Token);
            _disposables.Add(Disposable.Create(() =>
            {
                cs.Cancel();
                cs.Dispose();
                _messageChannel.OnCompleted();
                _serialPort.DataReceived -= _serialPort_DataReceived;
                _serialPort.Close();
                _serialPort.Dispose();
                _pipe.Writer.Complete();
                _pipe.Reader.Complete();
            }));
        }

        private void _serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            var buffer = ArrayPool<byte>.Shared.Rent(_serialPort.BytesToRead);

            var read = _serialPort.Read(buffer, 0, _serialPort.BytesToRead);
            Console.WriteLine($"Received [{string.Join(", ", buffer[..read].Select(b => b.ToString("X")))}]");
            _pipe.Writer.Write(buffer[..read]);
            _pipe.Writer.FlushAsync().GetAwaiter().OnCompleted(() =>
            {
                ArrayPool<byte>.Shared.Return(buffer);
            });
        }

        private void ReadMessages(CancellationToken cancellationToken)
        {

            while (!cancellationToken.IsCancellationRequested)
            {
                if (_pipe.Reader.TryRead(out var readerResult))
                {
                    var start = readerResult.Buffer.PositionOf(Message.StartOfPacket);
                    if (start is not null)
                    {
                        var end = readerResult.Buffer.PositionOf(Message.EndOfPacket);
                        if (end is not null)
                        {
                            var endIncluded = new SequencePosition(end.Value.GetObject(), end.Value.GetInteger() + 1);
                            var rawBytes = readerResult.Buffer.Slice(start.Value, endIncluded).ToArray();

                            var message = Message.FromRawBytes(rawBytes);
                            _pipe.Reader.AdvanceTo(endIncluded);
                            _pipe.Reader.CancelPendingRead();
                            _messageChannel.OnNext(message);
                            continue;

                        }
                        else // no end found
                        {
                            _pipe.Reader.CancelPendingRead();
                            continue;
                        }
                    }
                    else // no start found
                    {
                        _pipe.Reader.CancelPendingRead();
                        continue;
                    }
                }
                else
                {
                    // bug, should not need to call this
                    _pipe.Reader.CancelPendingRead();
                    continue;
                }
            }
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }

        public Task SendAsync(Message message, CancellationToken cancellationToken)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            var rawBytes = message.ToRawBytes();
            Console.WriteLine($"Sending [{string.Join(", ", rawBytes.Select(b => b.ToString("X")))}]");
            _serialPort.Write(rawBytes, 0, rawBytes.Length);
            return Task.CompletedTask;
        }

        public IDisposable Subscribe(IObserver<Message> observer) => _messageChannel.Subscribe(observer);

        public IDisposable SubscribeToNotification(Action<BatteryVoltageStateChangeNotification> onNext)
        {
            throw new NotImplementedException();
        }
    }

}
