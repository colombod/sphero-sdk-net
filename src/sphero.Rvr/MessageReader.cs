using sphero.Rvr.Protocol;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Pipelines;
using System.Threading.Tasks;

namespace sphero.Rvr
{
    public static class MessageReaderExtensions
    {
        public static async Task<IEnumerable<Message>> ReadMessages(this PipeReader reader)
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
                        if (readResult.Buffer.GetOffset(start.Value) > readResult.Buffer.GetOffset(end.Value))
                        {
                            reader.AdvanceTo(start.Value);
                            continue;
                        }

                        // todo - there should be a bug here, if the EOP is at the end of a BufferSegment. I think
                        // this should fix it, but it doesn't work as expected at all:
                        // var endIncluded = readResult.Buffer.GetPosition(readResult.Buffer.GetOffset(end.Value) + 1);
                        var endIncluded = new SequencePosition(end.Value.GetObject(), end.Value.GetInteger() + 1);
                        var rawBytes = readResult.Buffer.Slice(start.Value, endIncluded).ToArray();

                        if (rawBytes.Length == 0)
                        {
                            Debugger.Break();
                        }
                        var message = Message.FromRawBytes(rawBytes);
                        reader.AdvanceTo(endIncluded);
                        messages.Add(message);
                        continue;

                    }
                    else // no end found
                    {
                        reader.CancelPendingRead();
                        continue;
                    }
                }
                else // no start found
                {
                    reader.CancelPendingRead();
                    continue;
                }
            }
            return messages;
        }
    }
}