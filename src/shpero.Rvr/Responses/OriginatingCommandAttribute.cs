using System;
using shpero.Rvr.Commands;

namespace shpero.Rvr.Responses
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class OriginatingCommandAttribute : Attribute
    {
        public Type CommandType { get; }

        // See the attribute guidelines at 
        //  http://go.microsoft.com/fwlink/?LinkId=85236
        public OriginatingCommandAttribute(Type commandType)
        {
            if (!commandType.IsAssignableTo(typeof(Command)))
            {
                throw new ArgumentOutOfRangeException(nameof(commandType),
                    $"Type {commandType.FullName} is not a valid command type");
            }
            CommandType = commandType;
        }
    }
}