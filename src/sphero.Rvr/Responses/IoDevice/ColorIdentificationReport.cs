using System;
using sphero.Rvr.Commands.IoDevice;
using sphero.Rvr.Protocol;

namespace sphero.Rvr.Responses.IoDevice
{
    [OriginatingCommand(typeof(GetColorIdentificationReport))]
    public class ColorIdentificationReport : Response
    {
        public ColorIdentificationReport(Message message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            IndexConfidence = new byte[message.Data.Length];
            Buffer.BlockCopy(message.Data, 0, IndexConfidence, 0, message.Data.Length);
        }

        public byte[] IndexConfidence { get; }
    }
}