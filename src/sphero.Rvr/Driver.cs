using Pocket;
using sphero.Rvr.Protocol;
using System;
using System.Buffers;
using System.IO.Pipelines;
using System.IO.Ports;
using System.Linq;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;
using CompositeDisposable = System.Reactive.Disposables.CompositeDisposable;
using Disposable = System.Reactive.Disposables.Disposable;

namespace sphero.Rvr
{
    public class Driver : IDriver
    {
        private readonly SerialPort _serialPort;
        private readonly Pipe _pipe;
        private readonly CompositeDisposable _disposables = new();
        private readonly Subject<Message> _messageChannel = new();

        public Driver(string serialPort)
        {
            _serialPort = new SerialPort(serialPort, 115200);
            _serialPort.Open();
            _serialPort.DataReceived += SerialPortDataReceived;
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
                _serialPort.DataReceived -= SerialPortDataReceived;
                _serialPort.Close();
                _serialPort.Dispose();
                _pipe.Writer.Complete();
                _pipe.Reader.Complete();
            }));
        }

        private void SerialPortDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            var buffer = ArrayPool<byte>.Shared.Rent(_serialPort.BytesToRead);

            var read = _serialPort.Read(buffer, 0, _serialPort.BytesToRead);

            Logger.Log.Info($"Serial data revc: [{string.Join(", ", buffer[..read].Select(b => b.ToString("X")))}]");

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
                var messages = _pipe.Reader.ReadMessages();
                foreach (var message in messages.TakeWhile(_ => !cancellationToken.IsCancellationRequested))
                {
                    _messageChannel.OnNext(message);
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
            Logger.Log.Info($"Serial data send: [{ string.Join(", ", rawBytes.Select(b => b.ToString("X")))}]");
            _serialPort.Write(rawBytes, 0, rawBytes.Length);
            return Task.CompletedTask;
        }

        public IDisposable Subscribe(IObserver<Message> observer) => _messageChannel.Subscribe(observer);
    }
}
