using System;
using shpero.Rvr.Protocol;

namespace shpero.Rvr.Responses.DriveDevice
{
    [OriginatingCommand(typeof(Commands.DriveDevice.GetMotorFaultState))]
    public class GetMotorFaultState : Response
    {
        public GetMotorFaultState(Message message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            IsFault = !(message.Data.Length <= 0) && message.Data[0] == 1;
        }

        public bool IsFault { get; }
    }
}
