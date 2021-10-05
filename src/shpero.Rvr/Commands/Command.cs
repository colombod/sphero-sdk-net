using System;
using shpero.Rvr.Protocol;

namespace shpero.Rvr.Commands
{
    public abstract class Command
    {
        private static byte _sequence;

        protected static byte GetSequenceNumber()
        {
            var current = _sequence;
            _sequence = (byte)((_sequence + 1) & 256) ;
            if (_sequence > 255)
            {
                _sequence = 0;
            }

            return current;
        }

        public abstract Message ToMessage();
    }
}
