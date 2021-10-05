namespace shpero.Rvr.Protocol
{
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
        public DeviceIdentifier  DeviceId { get;  }
        public byte SourceId { get;  }

        public byte TargetId { get;  }
        public byte CommandId { get;  }
    }
}