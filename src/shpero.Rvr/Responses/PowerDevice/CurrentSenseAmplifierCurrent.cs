using System;
using shpero.Rvr.Commands.PowerDevice;
using shpero.Rvr.Protocol;

namespace shpero.Rvr.Responses.PowerDevice
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