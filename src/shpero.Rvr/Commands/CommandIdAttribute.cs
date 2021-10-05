using System;

namespace shpero.Rvr.Commands
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public sealed class CommandAttribute : Attribute
    {
        public CommandAttribute(byte id, DeviceIdentifier deviceIdentifier)
        {
            Id = id;
            DeviceIdentifier = deviceIdentifier;
        }

        public byte Id { get;  }
        public DeviceIdentifier DeviceIdentifier { get; }
    }
}