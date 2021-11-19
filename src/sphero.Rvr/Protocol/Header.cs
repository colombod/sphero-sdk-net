using System;

namespace sphero.Rvr.Protocol;

public class Header
{
    public Header(byte commandId, byte targetId, byte sourceId, DeviceIdentifier deviceId, byte sequence, Flags flags, ErrorCode? errorCode = null)
    {
        CommandId = commandId;
        TargetId = targetId;
        SourceId = sourceId;
        DeviceId = deviceId;
        Sequence = sequence;
        Flags = flags;
        ErrorCode = errorCode;
    }

    public byte Sequence { get; }
    public Flags Flags { get; }
    public ErrorCode? ErrorCode { get; }
    public DeviceIdentifier DeviceId { get; }
    public byte SourceId { get; }

    public byte TargetId { get; }
    public byte CommandId { get; }

    public override string ToString()
    {
        var errorCodeMessage = ErrorCode.HasValue ? $" ErrorCode {(byte)ErrorCode.Value} ({ErrorCode.Value.getApiErrorMessageFromCode()})" : string.Empty;

        return $"CommandId {CommandId:X} TargetId {TargetId:X} SourceId {SourceId:X} DeviceId {DeviceId} ({((byte)DeviceId):X}) Sequence {Sequence} Flags { Convert.ToString(((byte)Flags),2)}{errorCodeMessage}";
    }
}