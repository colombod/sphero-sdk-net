using sphero.Rvr.Protocol;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Pipelines;

namespace sphero.Rvr
{
    public static class MessageReaderExtensions
    {
        public static IEnumerable<Message> ReadMessages(this PipeReader reader)
        {
            var messages = new List<Message>();
            while (reader.TryRead(out var readResult))
            {
                if (readResult.IsCompleted || readResult.IsCanceled)
                {
                    break;
                }

                var start = readResult.Buffer.PositionOf(Message.StartOfPacket);
                var end = readResult.Buffer.PositionOf(Message.EndOfPacket);

                if (start is not null)
                {
                    if (end is not null)
                    {

                        if (readResult.Buffer.GetOffset(start.Value) >= readResult.Buffer.GetOffset(end.Value))
                        {
                            reader.AdvanceTo(start.Value);
                            continue;
                        }

                        var dataSize = (readResult.Buffer.GetOffset(end.Value) - readResult.Buffer.GetOffset(start.Value)) + 1;
                        var rawBytes = readResult.Buffer.Slice(start.Value, dataSize).ToArray();

                        var consumedDataPosition = readResult.Buffer.GetPosition(rawBytes.Length, start.Value);
                        var message = Message.FromRawBytes(rawBytes);
                        reader.AdvanceTo(consumedDataPosition);
                        messages.Add(message);
                    }
                    else // no end found
                    {
                        reader.AdvanceTo(readResult.Buffer.Start);
                        break;
                    }
                }
                else // no start found
                {
                    reader.AdvanceTo(readResult.Buffer.Start);
                    break;
                }
            }
            return messages;
        }
    }
}