using System;
using shpero.Rvr.Commands.IoDevice;
using shpero.Rvr.Protocol;

namespace shpero.Rvr.Responses.IoDevice
{
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
            Buffer.BlockCopy(message.Data, 0, RgbIndexBytesValues,0, message.Data.Length);
        }

        public byte[] RgbIndexBytesValues { get;  }
    }
}
