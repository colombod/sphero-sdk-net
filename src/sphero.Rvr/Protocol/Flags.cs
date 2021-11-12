using System;

namespace sphero.Rvr.Protocol;

[Flags]
public enum Flags : byte
{
    IsResponse = 1,
    RequestsResponse = 2,
    RequestOnlyErrorResponse = 4,
    ResetInactivityTimeout = 8,
    PacketHasTargetId = 16,
    PacketHasSourceId = 32,
    Unused = 64,
    ExtendedFlags = 128,
    DefaultRequestWithResponseFlags = RequestsResponse | ResetInactivityTimeout | PacketHasTargetId |
                                      PacketHasSourceId,
    DefaultRequestWithNoResponseFlags = ResetInactivityTimeout | PacketHasTargetId | PacketHasSourceId,
    DefaultResponseFlags = IsResponse | PacketHasTargetId | PacketHasSourceId
}