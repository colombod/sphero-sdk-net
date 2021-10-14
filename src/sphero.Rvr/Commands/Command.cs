using sphero.Rvr.Protocol;

namespace sphero.Rvr.Commands
{
    internal static class SequenceGenerator
    {
        private static byte _sequence;

        public static byte GetSequenceNumber()
        {
            var current = _sequence;
            _sequence = (byte)((_sequence + 1) & 0xFF);

            return current;
        }

    }

    public abstract class Command
    {

        protected static byte GetSequenceNumber()
        {
            return SequenceGenerator.GetSequenceNumber();
        }

        public abstract Message ToMessage();
    }
}
