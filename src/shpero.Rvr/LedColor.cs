using System.Collections.Generic;

namespace shpero.Rvr
{
    public class LedColor
    {
        public static IReadOnlyDictionary<LedColorNames, LedColor> Colors = new Dictionary<LedColorNames, LedColor>
        {
            [LedColorNames.Red] = new LedColor(0xFF, 0x00, 0x00),
            [LedColorNames.Green] = new LedColor(0x00, 0xFF, 0x00),
            [LedColorNames.Blue] = new LedColor(0x00, 0x00, 0xFF),
            [LedColorNames.Off] = new LedColor(0x00, 0x00, 0x00),
            [LedColorNames.White] = new LedColor(0xFF, 0xFF, 0xFF),
            [LedColorNames.Yellow] = new LedColor(0xFF, 0x90, 0x00),
            [LedColorNames.Purple] = new LedColor(0xFF, 0x00, 0xFF),
            [LedColorNames.Orange] = new LedColor(0xFF, 0x20, 0x00),
            [LedColorNames.Pink] = new LedColor(0xFF, 0x66, 0xB2)
        };

        public byte Red { get; }
        public byte Green { get; }
        public byte Blue { get; }

        public LedColor(byte red, byte green, byte blue)
        {
            Red = red;
            Green = green;
            Blue = blue;
        }
    }
}