using FluentAssertions;
using sphero.Rvr.Protocol;
using System;
using System.IO.Pipelines;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace sphero.Rvr.Tests
{
    public class SerializationTests
    {
        [Fact]
        public void message_with_no_data()
        {
            var header = new Header(0, 1, 1, DeviceIdentifier.SystemInfo, 0, Flags.RequestsResponse | Flags.ResetInactivityTimeout | Flags.PacketHasTargetId | Flags.PacketHasSourceId);
            var message = new Message(header);
            var raw = message.ToRawBytes();

            var expectedBytes = new byte[] {
                0x8D, //SOP
                0b_11_1010, // FLAGS: RequestResponse, IsActivity, HasTargetID, HasSourceID
                0x01, // Target: 0 1
                0x01, // Source: 0 1
                0x11, // DeviceID: 0x11 - SystemInfo
                0x00, // Command - Get App version
                0x00, // Seq num 0
                0xB2, // Checksum
                0xD8 // EOP
            };

            raw.Should().BeEquivalentTo(expectedBytes);
        }

        [Fact]
        public void message_with_errorCode()
        {
            var header = new Header(0, 1, 1, DeviceIdentifier.SystemInfo, 0, Flags.RequestsResponse | Flags.ResetInactivityTimeout | Flags.PacketHasTargetId | Flags.PacketHasSourceId, ErrorCode.not_yet_implemented);
            var message = new Message(header);
            var raw = message.ToRawBytes();

            var expectedBytes = new byte[] {
                0x8D, //SOP
                0b_11_1010, // FLAGS: RequestResponse, IsActivity, HasTargetID, HasSourceID
                0x01, // Target: 0 1
                0x01, // Source: 0 1
                0x11, // DeviceID: 0x11 - SystemInfo
                0x00, // Command - Get App version
                0x00, // Seq num 0
                0x03, // Not yet implemented error code 
                0xAF, // Checksum
                0xD8 // EOP
            };

            raw.Should().BeEquivalentTo(expectedBytes);
        }

        [Fact]
        public void serialize_response_message()
        {
            var header = new Header(0, 0x21, 1, DeviceIdentifier.SystemInfo, 0, Flags.IsResponse | Flags.ResetInactivityTimeout | Flags.PacketHasTargetId | Flags.PacketHasSourceId, ErrorCode.success);
            var message = new Message(header, new byte[]{
                0x00,
                0x08,
                0x00,
                0x03,
                0x01,
                0xB0});
            var raw = message.ToRawBytes();

            var expectedBytes = new byte[] {
                0x8D, // SOP
                0x39, // Flags
                0x21, // Target 2 1
                0x01, // Source
                0x11, // Device
                0x00, // Command
                0x00, // Sequence
                0x00, // ErrorCode
                0x00, // Data
                0x08, // Data
                0x00, // Data
                0x03, // Data
                0x01, // Data
                0xB0, // Data
                0xD7, // Checksum
                0xD8  // EOP
            };

            raw.Should().BeEquivalentTo(expectedBytes);
        }

        [Fact]
        public void deserialize_response_message()
        {
            var expectedHeader = new Header(0, 0x21, 1, DeviceIdentifier.SystemInfo, 0, Flags.IsResponse | Flags.ResetInactivityTimeout | Flags.PacketHasTargetId | Flags.PacketHasSourceId, ErrorCode.success);
            var expectedMessage = new Message(expectedHeader, new byte[]{
                0x00,
                0x08,
                0x00,
                0x03,
                0x01,
                0xB0});

            var rawBytes = new byte[] {
                0x8D, // SOP
                0x39, // Flags
                0x21, // Target 2 1
                0x01, // Source
                0x11, // Device
                0x00, // Command
                0x00, // Sequence
                0x00, // ErrorCode
                0x00, // Data
                0x08, // Data
                0x00, // Data
                0x03, // Data
                0x01, // Data
                0xB0, // Data
                0xD7, // Checksum
                0xD8  // EOP
            };

            var message = Message.FromRawBytes(rawBytes);

            message.Should().BeEquivalentTo(expectedMessage);
        }

        [Fact]
        public void awesome_deserializer_should_be_able_to_handle_big_real_life_data()
        {
            var stream = typeof(SerializationTests).Assembly.GetManifestResourceStream("sphero.Rvr.Tests.bufferData.bin");
            var pipe = new Pipe();


            stream.CopyTo(pipe.Writer.AsStream());

            var receivedMessages = pipe.Reader.ReadMessages();

            receivedMessages.Count().Should().Be(205);
        }

        [Fact]
        public async Task messages_can_be_read_in_chunks()
        {
            var msgBytes = new byte[] {
                0x8D, // SOP
                0x39, // Flags
                0x21, // Target 2 1
                0x01, // Source
                0x11, // Device
                0x00, // Command
                0x00, // Sequence
                0x00, // ErrorCode
                0x00, // Data
                0x08, // Data
                0x00, // Data
                0x03, // Data
                0x01, // Data
                0xB0, // Data
                0xD7, // Checksum
                0xD8  // EOP
            };
            var half = msgBytes.Length / 2;

            var pipe = new Pipe();
            await pipe.Writer.WriteAsync(msgBytes.AsMemory().Slice(0, half));

            var receivedMessages = pipe.Reader.ReadMessages();
            receivedMessages.Count().Should().Be(0);

            await pipe.Writer.WriteAsync(msgBytes.AsMemory().Slice(half));

            receivedMessages = pipe.Reader.ReadMessages();
            receivedMessages.Count().Should().Be(1);
        }

        [Fact]
        public async Task junk_at_start_is_ignored()
        {
            var junk = Enumerable.Repeat<byte>(0, 5000).ToArray();
            var msgBytes = new byte[] {
                0x01,
                0x02,
                0x03,
                0x05,
                0x8D, // SOP
                0x39, // Flags
                0x21, // Target 2 1
                0x01, // Source
                0x11, // Device
                0x00, // Command
                0x00, // Sequence
                0x00, // ErrorCode
                0x00, // Data
                0x08, // Data
                0x00, // Data
                0x03, // Data
                0x01, // Data
                0xB0, // Data
                0xD7, // Checksum
                0xD8  // EOP
            };

            var pipe = new Pipe();
            await pipe.Writer.WriteAsync(junk);

            pipe.Reader.ReadMessages().Count().Should().Be(0);

            await pipe.Writer.WriteAsync(msgBytes);

            var receivedMessages = pipe.Reader.ReadMessages();
            receivedMessages.Count().Should().Be(1);

            var half = msgBytes.Length / 2;

            var msgWithJunkWithEOP = msgBytes[half..].Concat(msgBytes[half..]).Concat(msgBytes).ToArray();
            await pipe.Writer.WriteAsync(msgWithJunkWithEOP);

            receivedMessages = pipe.Reader.ReadMessages();
            receivedMessages.Count().Should().Be(1);

            var twoMessagesWithJunkInBetween = msgBytes.Concat(junk).Concat(msgBytes).ToArray();
            await pipe.Writer.WriteAsync(twoMessagesWithJunkInBetween);

            receivedMessages = pipe.Reader.ReadMessages();
            receivedMessages.Count().Should().Be(2);
        }
    }
}