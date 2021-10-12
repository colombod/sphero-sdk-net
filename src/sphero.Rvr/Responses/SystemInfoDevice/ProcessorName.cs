using System;
using sphero.Rvr.Commands.SystemInfoDevice;
using sphero.Rvr.Protocol;

namespace sphero.Rvr.Responses.SystemInfoDevice
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

            Name = message.Data.ToStringFromNullTerminated(true);
        }

        public string Name { get; }
    }
}