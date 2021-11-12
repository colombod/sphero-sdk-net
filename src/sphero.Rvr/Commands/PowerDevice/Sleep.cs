﻿using sphero.Rvr.Protocol;

namespace sphero.Rvr.Commands.PowerDevice;

[Command(CommandId, DeviceId)]
public class Sleep : Command
{
    public const byte CommandId = 0x01;

    public const DeviceIdentifier DeviceId = DeviceIdentifier.Power;
    public override Message ToMessage()
    {
        var header = new Header(
            commandId: CommandId,
            targetId: 0x01,
            deviceId: DeviceId,
            sourceId: ApiTargetsAndSources.ServiceSource,
            sequence: GetSequenceNumber(),
            flags: Flags.DefaultRequestWithNoResponseFlags);
        return new Message(header);
    }
}