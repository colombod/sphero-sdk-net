using FluentAssertions;
using sphero.Rvr;
using sphero.Rvr.Protocol;

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
    }
}
