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

namespace sphero.Rvr;

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
        Task.Factory.StartNew(() =>
        {
            while (!cs.IsCancellationRequested)
            {
                var messages = _pipe.Reader.ReadMessages().ToList();
                if (messages.Count > 0)
                {
                    Task.Factory.StartNew(() =>
                    {
                        foreach (var message in messages)
                        {
                            _messageChannel.OnNext(message);
                        }
                    });
                }
            }
        }, cs.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default);

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
        var bytesToRead = _serialPort.BytesToRead;
        var buffer = ArrayPool<byte>.Shared.Rent(bytesToRead);

        var read = _serialPort.Read(buffer, 0, bytesToRead);

        Logger.Log.Info($"Serial data received: [{string.Join(", ", buffer[..read].Select(b => b.ToString("X")))}]");

        _pipe.Writer.Write(buffer[..read]);
        _pipe.Writer.FlushAsync().GetAwaiter().OnCompleted(() =>
        {
            ArrayPool<byte>.Shared.Return(buffer);
        });
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