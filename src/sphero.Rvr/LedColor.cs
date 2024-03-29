﻿using System.Collections.Generic;

namespace sphero.Rvr;

public class Color
{
    public static IReadOnlyDictionary<ColorNames, Color> Colors = new Dictionary<ColorNames, Color>
    {
        [ColorNames.Red] = new(0xFF, 0x00, 0x00),
        [ColorNames.Green] = new(0x00, 0xFF, 0x00),
        [ColorNames.Blue] = new(0x00, 0x00, 0xFF),
        [ColorNames.Off] = new(0x00, 0x00, 0x00),
        [ColorNames.White] = new(0xFF, 0xFF, 0xFF),
        [ColorNames.Yellow] = new(0xFF, 0x90, 0x00),
        [ColorNames.Purple] = new(0xFF, 0x00, 0xFF),
        [ColorNames.Orange] = new(0xFF, 0x20, 0x00),
        [ColorNames.Pink] = new(0xFF, 0x66, 0xB2)
    };

    public byte Red { get; }
    public byte Green { get; }
    public byte Blue { get; }

    public Color(byte red, byte green, byte blue)
    {
        Red = red;
        Green = green;
        Blue = blue;
    }

    public byte[] ToRawBytes()
    {
        return new[] { Red, Green, Blue };
    }

    public override string ToString()
    {
        return $"[R {Red:X} G {Green:X} B {Blue:X}]";
    }

    public byte ToGreyScale()
    {
        double g = ((double)Green + (double)Red + (double)Blue) / 3.0;
        return (byte)(g % 255);
    }
}