using System;
using sphero.Rvr.Commands.IoDevice;
using sphero.Rvr.Protocol;

namespace sphero.Rvr.Responses.IoDevice;

[OriginatingCommand(typeof(GetActiveColorPalette))]
public class ActiveColorPalette : Response
{
    public ActiveColorPalette(Message message)
    {
        if (message == null)
        {
            throw new ArgumentNullException(nameof(message));
        }

        RgbIndexBytesValues = new byte[message.Data.Length];
        Buffer.BlockCopy(message.Data, 0, RgbIndexBytesValues, 0, message.Data.Length);
    }

    public byte[] RgbIndexBytesValues { get; }
}