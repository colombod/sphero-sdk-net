using System;
using shpero.Rvr.Commands.SystemInfoDevice;
using shpero.Rvr.Protocol;

namespace shpero.Rvr.Responses.SystemInfoDevice
{
    [OriginatingCommand(typeof(GetProcessorName))]
    public class ProcessorName : Response
    {
        public ProcessorName(Message message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            Name = BitConverter.ToString(message.Data);
        }

        public string Name { get; }
    }
}