using System;
using System.Collections.Generic;
using System.Linq;

#nullable  enable
namespace sphero.Rvr.Protocol
{
    public class Message
    {
        public const byte StartOfPacket = 0x8d;
        public const byte EndOfPacket = 0xd8;
        public const byte Escape = 0xab;
        public const byte EscapedStart = 0x05;
        public const byte EscapedEnd = 0x50;
        public const byte EscapedEscape = 0x23;

        public Message(Header header, byte[]? data = null)
        {
            Header = header ?? throw new ArgumentNullException(nameof(header));
            Data = data ?? Array.Empty<byte>();
        }

        public Header Header { get; }
        public byte[] Data { get; }


        public byte[] ToRawBytes()
        {
            int runningChecksum = 0;
            var rawBytes = new List<byte>(1024) { StartOfPacket };

            rawBytes.AddRange(EscapeBytes((byte)Header.Flags));
            runningChecksum += (byte)Header.Flags;

            if ((Header.Flags & Flags.PacketHasTargetId) > 0)
            {
                rawBytes.AddRange(EscapeBytes(Header.TargetId));
                runningChecksum += Header.TargetId;
            }

            if ((Header.Flags & Flags.PacketHasSourceId) > 0)
            {
                rawBytes.AddRange(EscapeBytes(Header.SourceId));
                runningChecksum += Header.SourceId;
            }

            rawBytes.AddRange(EscapeBytes((byte)Header.DeviceId));
            runningChecksum += (byte)Header.DeviceId;
            rawBytes.AddRange(EscapeBytes(Header.CommandId));
            runningChecksum += Header.CommandId;
            rawBytes.AddRange(EscapeBytes(Header.Sequence));
            runningChecksum += Header.Sequence;

            if (Header.ErrorCode is { })
            {
                rawBytes.AddRange(EscapeBytes((byte)Header.ErrorCode));
                runningChecksum += (byte)Header.ErrorCode;
            }

            foreach (var dataByte in Data)
            {
                rawBytes.AddRange(EscapeBytes(dataByte));
                runningChecksum += dataByte;
            }

            runningChecksum = ~(runningChecksum % 256);
            if (runningChecksum < 0)
            {
                runningChecksum = 256 + runningChecksum;
            }
            rawBytes.AddRange(EscapeBytes((byte)runningChecksum));

            rawBytes.Add(EndOfPacket);

            return rawBytes.ToArray();
        }

        private byte[] EscapeBytes(byte value)
        {
            switch (value)
            {
                case StartOfPacket:
                    return new[] { Escape, EscapedStart };

                case EndOfPacket:
                    return new[] { Escape, EscapedEnd };

                case Escape:
                    return new[] { Escape, EscapedEscape };
                default:
                    return new[] { value };
            }
        }

        public static Message FromRawBytes(byte[] rawBytes)
        {
            if (rawBytes == null)
            {
                throw new ArgumentNullException(nameof(rawBytes));
            }

            if (rawBytes[0] != StartOfPacket)
            {
                throw new InvalidOperationException("Expected Start of Packet.");
            }

            if (rawBytes[^1] != EndOfPacket)
            {
                throw new InvalidOperationException("Expected End of Packet.");
            }

            var unescabedytes = UnEscape(rawBytes[1..^1]);

            var runningChecksum = unescabedytes[..^1].Aggregate(0, (a, v) => v + a);
            runningChecksum = ~(runningChecksum % 256);
            if (runningChecksum < 0)
            {
                runningChecksum = 256 + runningChecksum;
            }

            if (runningChecksum != unescabedytes[^1])
            {
                throw new InvalidOperationException($"Checksum {unescabedytes[^1]} not matching {runningChecksum}.");
            }

            var flags = GetFlags(unescabedytes);
            var pos = 1;
            byte target = 0x0;
            byte source = 0x0;
            ErrorCode? errorCode = null;
            if ((flags & Flags.PacketHasTargetId) > 0)
            {
                target = unescabedytes[pos];
                pos++;
            }

            if ((flags & Flags.PacketHasSourceId) > 0)
            {
                source = unescabedytes[pos];
                pos++;
            }

            var deviceId = (DeviceIdentifier)unescabedytes[pos++];
            var commandId = unescabedytes[pos++];
            var sequence = unescabedytes[pos++];

            if ((flags & Flags.IsResponse) > 0)
            {
                errorCode = (ErrorCode)unescabedytes[pos];
                pos++;
            }

            var header = new Header(commandId, target, source, deviceId, sequence, flags, errorCode);

            var data = unescabedytes[pos..^1];

            return new Message(header, data);
        }

        private static Flags GetFlags(byte[] rawBytes)
        {
            return (Flags)rawBytes[0];
        }

        private static byte[] UnEscape(byte[] rawByte)
        {
            var unescapedRawBytes = new List<byte>(rawByte.Length);
            var pos = 0;
            while (pos < rawByte.Length)
            {
                if (rawByte[pos] == Escape)
                {
                    pos++;
                    switch (rawByte[pos])
                    {
                        case EscapedStart:
                            unescapedRawBytes.Add(StartOfPacket);
                            break;
                        case EscapedEnd:
                            unescapedRawBytes.Add(EndOfPacket);
                            break;

                        case EscapedEscape:
                            unescapedRawBytes.Add(Escape);
                            break;
                        default:
                            throw new InvalidOperationException("Cannot unescape this byte.");
                    }
                }
                else
                {
                    unescapedRawBytes.Add(rawByte[pos]);
                }
                pos++;
            }
            return unescapedRawBytes.ToArray();
        }
    }
}
