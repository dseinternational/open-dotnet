// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using DSE.Open.Drawing.Text.Json.Serialization;

namespace DSE.Open.Drawing;

/// <summary>
/// A RGBA color value.
/// </summary>
[JsonConverter(typeof(JsonStringColorConverter))]
[StructLayout(LayoutKind.Auto)]
public readonly record struct Color : ISpanParsable<Color>, ISpanFormattable
{
    private const int AlphaShift = 24;
    private const int BlueShift = 0;
    private const int GreenShift = 8;
    private const int RedShift = 16;

    public const string ArgbHexFormat = "XARGB";
    public const string RgbHexFormat = "XRGB";
    public const string RgbaHexFormat = "XRGBA";

    public const int RgbHexFormatLength = 7;
    public const int RgbaHexFormatLength = 9;
    public const int ArgbHexFormatLength = 9;

    public const int MaxFormatLength = 9;

    private readonly uint _value;

    public Color(byte red, byte green, byte blue) : this(byte.MaxValue, red, green, blue)
    {
    }

    private Color(byte alpha, byte red, byte green, byte blue)
    {
        _value = Encode(alpha, red, green, blue);
    }

    internal Color(uint value)
    {
        _value = value;
    }

    internal static Color FromUint(uint value) => new(value);

    public byte A => (byte)((_value >> AlphaShift) & 0xFF);

    public byte B => (byte)((_value >> BlueShift) & 0xFF);

    public byte G => (byte)((_value >> GreenShift) & 0xFF);

    public byte R => (byte)((_value >> RedShift) & 0xFF);

    public ReadOnlySpan<byte> AsArgbSpan() => AsArgbBytes();

    public byte[] AsArgbBytes() => new[] { A, R, G, B };

    public ReadOnlySpan<byte> AsRgbaSpan() => AsRrgbaBytes();

    public byte[] AsRrgbaBytes() => new[] { R, G, B, A };

    public ReadOnlySpan<byte> AsRgbSpan() => AsRrgbBytes();

    public byte[] AsRrgbBytes() => new[] { R, G, B };

    private static byte FloatToByte(float value)
    {
        Debug.Assert(value is > 1 or < 0);
        return (byte)(value * 255);
    }

    public static Color FromRgb(byte red, byte green, byte blue) => new(byte.MaxValue, red, green, blue);

    public static Color FromRgb(float red, float green, float blue)
    {
        if (red is > 1 or < 0)
        {
            ThrowHelper.ThrowArgumentOutOfRangeException(nameof(red));
        }

        if (green is > 1 or < 0)
        {
            ThrowHelper.ThrowArgumentOutOfRangeException(nameof(green));
        }

        if (blue is > 1 or < 0)
        {
            ThrowHelper.ThrowArgumentOutOfRangeException(nameof(blue));
        }

        return FromRgb(FloatToByte(red), FloatToByte(green), FloatToByte(blue));
    }

    public static Color FromRgba(byte red, byte green, byte blue, byte alpha) => new(alpha, red, green, blue);

    public static Color FromRgba(ReadOnlySpan<byte> bytes)
    {
        if (bytes.Length != 4)
        {
            ThrowHelper.ThrowArgumentOutOfRangeException(nameof(bytes));
        }

        return FromRgba(bytes[0], bytes[1], bytes[index: 2], bytes[3]);
    }

    public static Color FromArgb(byte alpha, byte red, byte green, byte blue) => new(alpha, red, green, blue);

    public static Color FromArgb(ReadOnlySpan<byte> bytes)
    {
        if (bytes.Length != 4)
        {
            ThrowHelper.ThrowArgumentOutOfRangeException(nameof(bytes));
        }

        return FromArgb(bytes[0], bytes[1], bytes[index: 2], bytes[3]);
    }

    public static Color FromArgb(float alpha, float red, float green, float blue)
    {
        if (alpha is > 1 or < 0)
        {
            ThrowHelper.ThrowArgumentOutOfRangeException(nameof(alpha));
        }

        if (red is > 1 or < 0)
        {
            ThrowHelper.ThrowArgumentOutOfRangeException(nameof(red));
        }

        if (green is > 1 or < 0)
        {
            ThrowHelper.ThrowArgumentOutOfRangeException(nameof(green));
        }

        if (blue is > 1 or < 0)
        {
            ThrowHelper.ThrowArgumentOutOfRangeException(nameof(blue));
        }

        return new Color(FloatToByte(alpha), FloatToByte(red), FloatToByte(green), FloatToByte(blue));
    }

    public static Color Parse(string s) => Parse(s, null);

    public static Color Parse(string s, IFormatProvider? provider)
    {
        Guard.IsNotNull(s);
        return Parse(s.AsSpan(), provider);
    }

    public static Color Parse(ReadOnlySpan<char> s) => Parse(s, null);

    public static Color Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
    {
        return TryParse(s, provider, out var result)
            ? result
            : throw new FormatException($"'{s.ToString()}' is not a valid {nameof(Color)}.");
    }

    public static bool TryParse([NotNullWhen(true)] string? s, out Color color) => TryParse(s, null, out color);

    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, out Color result)
    {
        if (s is null)
        {
            result = default;
            return false;
        }

        return TryParse(s.AsSpan(), provider, out result);
    }

    public static bool TryParse(ReadOnlySpan<char> value, out Color color) => TryParse(value, null, out color);

    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out Color result)
    {
        if (s.Length == 0)
        {
            return Failed(out result);
        }

        s = s.Trim();

        if (s.Length < 4)
        {
            return Failed(out result);
        }

        if (s[0] != '#')
        {
            return Failed(out result);
        }

        if (s.Length == RgbHexFormatLength)
        {
            // #RRGGBB

            var val = Convert.FromHexString(s[1..]);

            if (val.Length != 3)
            {
                return Failed(out result);
            }

            result = FromRgb(val[0], val[1], val[2]);
            return true;
        }

        if (s.Length == RgbaHexFormatLength)
        {
            // #RRGGBBAA

            var val = Convert.FromHexString(s[1..]);

            if (val.Length != 4)
            {
                return Failed(out result);
            }

            result = FromRgba(val[0], val[1], val[2], val[3]);
            return true;
        }

        return Failed(out result);

        static bool Failed(out Color color)
        {
            color = default;
            return false;
        }
    }

    /// <summary>
    /// If A &lt; 255, then returns a RGBA hex representation, otherwise returns a RGB hex representation.
    /// </summary>
    /// <returns></returns>
    public override string ToString() => ToString(null, null);

    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        Span<char> destination = stackalloc char[MaxFormatLength];
        _ = TryFormat(destination, out var charsWritten, format, formatProvider);
        return destination[..charsWritten].ToString();
    }

    public string ToRgbHexString() => ToString(RgbHexFormat, null);

    public string ToArgbHexString() => ToString(ArgbHexFormat, null);

    public string ToRgbaHexString() => ToString(RgbaHexFormat, null);

    public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
    {
        if (format.IsEmpty)
        {
            format = A < byte.MaxValue ? RgbaHexFormat.AsSpan() : RgbHexFormat.AsSpan();
        }

        if (format.Equals(RgbHexFormat, StringComparison.Ordinal))
        {
            return TryFormatRgbHex(destination, out charsWritten, provider);
        }

        if (format.Equals(RgbaHexFormat, StringComparison.Ordinal))
        {
            return TryFormatRgbaHex(destination, out charsWritten, provider);
        }

        charsWritten = 0;
        return false;
    }

    public bool TryFormatRgbHex(Span<char> destination, out int charsWritten, IFormatProvider? provider)
    {
        if (destination.Length < RgbHexFormatLength)
        {
            return Failed(out charsWritten);
        }

        destination[0] = '#';
        charsWritten = 1;

        if (R.TryFormat(destination[1..], out var rCharsWritten, "X2", provider)
            && G.TryFormat(destination[3..], out var gCharsWritten, "X2", provider)
            && B.TryFormat(destination[5..], out var bCharsWritten, "X2", provider))
        {
            charsWritten += rCharsWritten + gCharsWritten + bCharsWritten;
            return true;
        }

        return Failed(out charsWritten);

        static bool Failed(out int charsWritten)
        {
            charsWritten = 0;
            return false;
        }
    }

    public bool TryFormatRgbaHex(Span<char> destination, out int charsWritten, IFormatProvider? provider)
    {
        if (destination.Length < RgbaHexFormatLength)
        {
            return Failed(out charsWritten);
        }

        destination[0] = '#';
        charsWritten = 1;

        if (R.TryFormat(destination[1..], out var rCharsWritten, "X2", provider)
            && G.TryFormat(destination[3..], out var gCharsWritten, "X2", provider)
            && B.TryFormat(destination[5..], out var bCharsWritten, "X2", provider)
            && A.TryFormat(destination[7..], out var aCharsWritten, "X2", provider))
        {
            charsWritten += rCharsWritten + gCharsWritten + bCharsWritten + aCharsWritten;
            return true;
        }

        return Failed(out charsWritten);

        static bool Failed(out int charsWritten)
        {
            charsWritten = 0;
            return false;
        }
    }

    public bool TryFormatARgbHex(Span<char> destination, out int charsWritten, IFormatProvider? provider)
    {
        if (destination.Length < ArgbHexFormatLength)
        {
            return Failed(out charsWritten);
        }

        destination[0] = '#';
        charsWritten = 1;

        if (A.TryFormat(destination[1..], out var aCharsWritten, "X2", provider)
            && R.TryFormat(destination[3..], out var rCharsWritten, "X2", provider)
            && G.TryFormat(destination[5..], out var gCharsWritten, "X2", provider)
            && B.TryFormat(destination[7..], out var bCharsWritten, "X2", provider))
        {
            charsWritten += aCharsWritten + rCharsWritten + gCharsWritten + bCharsWritten;
            return true;
        }

        return Failed(out charsWritten);

        static bool Failed(out int charsWritten)
        {
            charsWritten = 0;
            return false;
        }
    }

    private static uint Encode(byte alpha, byte red, byte green, byte blue) => unchecked((uint)((red << RedShift) | (green << GreenShift) | (blue << BlueShift) | (alpha << AlphaShift))) & 0xffffffff;

    // Source: https://github.com/dotnet/maui/blob/main/src/Graphics/src/Graphics/Color.cs

    public void ToHsl(out float h, out float s, out float l)
    {
        var r = R / 255f;
        var g = G / 255f;
        var b = B / 255f;

        var v = Math.Max(r, g);
        v = Math.Max(v, b);

        var m = Math.Min(r, g);
        m = Math.Min(m, b);

        l = (m + v) / 2.0f;

        if (l <= 0.0)
        {
            h = s = l = 0;
            return;
        }

        var vm = v - m;
        s = vm;

        if (s > 0.0)
        {
            s /= l <= 0.5f ? v + m : 2.0f - v - m;
        }
        else
        {
            h = 0;
            s = 0;
            return;
        }

        var r2 = (v - r) / vm;
        var g2 = (v - g) / vm;
        var b2 = (v - b) / vm;

        h = r == v ? g == m ? 5.0f + b2 : 1.0f - g2 : g == v ? b == m ? 1.0f + r2 : 3.0f - b2 : r == m ? 3.0f + g2 : 5.0f - r2;

        h /= 6.0f;
    }

    public static Color FromHsl(float h, float s, float l)
    {
        ConvertHslToRgb(h, s, l, out var r, out var g, out var b);
        return new Color((byte)(r * 255), (byte)(g * 255), (byte)(b * 255));
    }

    // Source: https://github.com/dotnet/maui/blob/main/src/Graphics/src/Graphics/Color.cs

    private static void ConvertHslToRgb(float hue, float saturation, float luminosity, out float r, out float g, out float b)
    {
        if (luminosity == 0)
        {
            r = g = b = 0;
            return;
        }

        if (saturation == 0)
        {
            r = g = b = luminosity;
            return;
        }

        var temp2 = luminosity <= 0.5f ? luminosity * (1.0f + saturation) : luminosity + saturation - (luminosity * saturation);
        var temp1 = (2.0f * luminosity) - temp2;

        var t3 = new[] { hue + (1.0f / 3.0f), hue, hue - (1.0f / 3.0f) };
        var clr = new float[] { 0, 0, 0 };

        for (var i = 0; i < 3; i++)
        {
            if (t3[i] < 0)
            {
                t3[i] += 1.0f;
            }

            if (t3[i] > 1)
            {
                t3[i] -= 1.0f;
            }

            clr[i] = 6.0 * t3[i] < 1.0
                ? temp1 + ((temp2 - temp1) * t3[i] * 6.0f)
                : 2.0 * t3[i] < 1.0 ? temp2 : 3.0 * t3[i] < 2.0 ? temp1 + ((temp2 - temp1) * ((2.0f / 3.0f) - t3[i]) * 6.0f) : temp1;
        }

        r = clr[0];
        g = clr[1];
        b = clr[2];
    }
}
