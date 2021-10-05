using System.Linq;

namespace shpero.Rvr
{
    public static class ApiTargetsAndSources
    {
        public static byte RobotNordicTarget { get; } =
            ByteConversionUtilities.NibblesToByte(new[] { 1, 1 }.Reverse().ToArray());

        public static byte RobotStTarget { get; } = ByteConversionUtilities.NibblesToByte(new[]
        {
            1, 2
        }.Reverse().ToArray());

        public static byte ServiceSource { get; } =
            ByteConversionUtilities.NibblesToByte(new[] { 0, 1 }.Reverse().ToArray());
    }
}