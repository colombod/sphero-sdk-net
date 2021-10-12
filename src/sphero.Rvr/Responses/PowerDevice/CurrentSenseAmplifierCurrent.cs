using System;
using sphero.Rvr.Commands.PowerDevice;
using sphero.Rvr.Protocol;

namespace sphero.Rvr.Responses.PowerDevice
{
    [OriginatingCommand(typeof(GetCurrentSenseAmplifierCurrent))]
    public class CurrentSenseAmplifierCurrent : Response
    {
        public CurrentSenseAmplifierCurrent(Message message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            AmplifierCurrent = message.Data[..4].ToFloat();
        }

        public float AmplifierCurrent { get; }
    }
}