using sphero.Rvr.Protocol;

namespace sphero.Rvr.Commands.DriveDevice
{
    [Command(CommandId, DeviceId)]
    public class SetRawMotor : Command
    {
        private readonly RawMotorMode _leftRawMotor;
        private readonly byte _leftMotorSpeed;
        private readonly RawMotorMode _rightRawMotor;
        private readonly byte _rightMotorSpeed;

        public const byte CommandId = 0x01;

        public const DeviceIdentifier DeviceId = DeviceIdentifier.Drive;


        public SetRawMotor(RawMotorMode leftRawMotor, byte leftMotorSpeed, RawMotorMode rightRawMotor, byte rightMotorSpeed)
        {
            _leftRawMotor = leftRawMotor;
            _leftMotorSpeed = leftMotorSpeed;
            _rightRawMotor = rightRawMotor;
            _rightMotorSpeed = rightMotorSpeed;
        }

        public override Message ToMessage()
        {
            var header = new Header(
                commandId: CommandId,
                targetId: 0x02,
                deviceId: DeviceId,
                sourceId: ApiTargetsAndSources.ServiceSource,
                sequence: GetSequenceNumber(),
                flags: Flags.DefaultRequestWithNoResponseFlags);
            return new Message(header, new[] { (byte)_leftRawMotor, _leftMotorSpeed, (byte)_rightRawMotor, _rightMotorSpeed });
        }
    }
}