using System;
using shpero.Rvr.Commands.SystemInfoDevice;
using shpero.Rvr.Protocol;

namespace shpero.Rvr.Responses.SystemInfoDevice
{
    [OriginatingCommand(typeof(GetBoardRevision))]
    public class BoardRevision : Response
    {
        public BoardRevision(Message message)
        {
            message = message ?? throw new ArgumentNullException(nameof(message));
            if (message.Data.Length < 1 * sizeof(byte))
            {
                throw new InvalidOperationException("not enough data");
            }

            Revision = message.Data[0];
        }

        public byte Revision { get; }
    }
}