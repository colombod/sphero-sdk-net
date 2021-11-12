using System;
using sphero.Rvr.Protocol;

namespace sphero.Rvr.Commands.SensorDevice;

[Command(CommandId, DeviceId)]
public class SendInfraredMessage : Command
{
    private readonly byte _infraredCode;
    private readonly byte _frontStrength;
    private readonly byte _leftStrength;
    private readonly byte _rightStrength;
    private readonly byte _rearStrength;

    public const byte CommandId = 0x3F;

    public const DeviceIdentifier DeviceId = DeviceIdentifier.Sensor;

    public SendInfraredMessage(byte infraredCode, byte frontStrength, byte leftStrength, byte rightStrength,
        byte rearStrength)
    {
        _infraredCode = infraredCode;
        _frontStrength = frontStrength;
        _leftStrength = leftStrength;
        _rightStrength = rightStrength;
        _rearStrength = rearStrength;

        if (infraredCode > 7)
        {
            throw new ArgumentOutOfRangeException(nameof(infraredCode), "Value 0-7.");
        }

        if (_frontStrength > 64)
        {
            throw new ArgumentOutOfRangeException(nameof(_frontStrength), "Value 0-64 : 0 no message sent, 64 longest available rage.");
        }

        if (_leftStrength > 64)
        {
            throw new ArgumentOutOfRangeException(nameof(_leftStrength), "Value 0-64 : 0 no message sent, 64 longest available rage.");
        }

        if (_rightStrength > 64)
        {
            throw new ArgumentOutOfRangeException(nameof(_rightStrength), "Value 0-64 : 0 no message sent, 64 longest available rage.");
        }

        if (_rearStrength > 64)
        {
            throw new ArgumentOutOfRangeException(nameof(_rearStrength), "Value 0-64 : 0 no message sent, 64 longest available rage.");
        }
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
        return new Message(header,
            new byte[] { _infraredCode, _frontStrength, _leftStrength, _rightStrength, _rearStrength });
    }
}