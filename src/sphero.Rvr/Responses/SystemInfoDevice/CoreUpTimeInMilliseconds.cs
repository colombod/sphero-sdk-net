﻿using System;
using sphero.Rvr.Commands.SystemInfoDevice;
using sphero.Rvr.Protocol;

namespace sphero.Rvr.Responses.SystemInfoDevice;

[OriginatingCommand(typeof(GetCoreUpTimeInMilliseconds))]
public class CoreUpTimeInMilliseconds : Response
{
    public CoreUpTimeInMilliseconds(Message message)
    {
        message = message ?? throw new ArgumentNullException(nameof(message));
        Milliseconds = message.Data[..sizeof(long)].ToLong();
    }
    public long Milliseconds { get; }
}