// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using DSE.Open.Values.Text.Json.Serialization;

namespace DSE.Open.Values;

[JsonConverter(typeof(JsonStringAgeInMonthsConverter))]
[StructLayout(LayoutKind.Auto)]
public readonly record struct AgeInMonths : ISpanFormattable, ISpanParsable<AgeInMonths>, IComparable<AgeInMonths>
{
    private const string DefaultFormat = "0";

    public static readonly AgeInMonths Zero;

    public AgeInMonths(int years, int months) : this((years * 12) + months)
    {
    }

    public AgeInMonths(int totalMonths)
    {
        TotalMonths = totalMonths;
    }

    /// <summary>
    /// The number of complete years represented by the value.
    /// </summary>
    public int Years => TotalMonths / 12;

    /// <summary>
    /// The number of months represented by the value in addition to the <see cref="Years"/>.
    /// </summary>
    public int Months => TotalMonths % 12;

    /// <summary>
    /// The total number of months represented by the value. This is the combined value of
    /// <see cref="Years"/> and <see cref="Months"/>.
    /// </summary>
    public int TotalMonths { get; }

    public AgeInMonths AddMonths(int months)
    {
        return new(TotalMonths + months);
    }

    public AgeInMonths AddYears(int years)
    {
        return new(TotalMonths + (years * 12));
    }

    public int CompareTo(AgeInMonths other)
    {
        return TotalMonths.CompareTo(other.TotalMonths);
    }

    public bool TryFormat(Span<char> destination, out int charsWritten)
    {
        return TryFormat(destination, out charsWritten, default, default);
    }

    public bool TryFormat(
        Span<char> destination,
        out int charsWritten,
        IFormatProvider? provider)
    {
        return TryFormat(destination, out charsWritten, default, provider);
    }

    public bool TryFormat(
        Span<char> destination,
        out int charsWritten,
        ReadOnlySpan<char> format,
        IFormatProvider? provider)
    {
        if (destination.Length >= 3 && Years.TryFormat(destination, out var charsWrittenYears, DefaultFormat, provider))
        {
            charsWritten = charsWrittenYears;

            if (destination.Length > charsWrittenYears)
            {
                destination[charsWrittenYears] = ':';
                charsWritten++;
                if (Months.TryFormat(destination[charsWritten..], out var charsWrittenMonths, DefaultFormat, provider))
                {
                    charsWritten += charsWrittenMonths;
                    return true;
                }
            }
        }

        charsWritten = 0;
        return false;
    }

    public static AgeInMonths Parse(ReadOnlySpan<char> s)
    {
        return Parse(s, default);
    }

    public static AgeInMonths Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
    {
        if (TryParse(s, provider, out var result))
        {
            return result;
        }

        ThrowHelper.ThrowFormatException($"Could not parse {nameof(AgeInMonths)} value: {s}");
        return default;
    }

    public static AgeInMonths Parse(string s)
    {
        return Parse(s, default);
    }

    public static AgeInMonths Parse(string s, IFormatProvider? provider)
    {
        ArgumentNullException.ThrowIfNull(s);
        return Parse(s.AsSpan(), provider);
    }

    public static bool TryParse(
        ReadOnlySpan<char> s,
        out AgeInMonths result)
    {
        return TryParse(s, default, out result);
    }

    public static bool TryParse(
        ReadOnlySpan<char> s,
        IFormatProvider? provider,
        out AgeInMonths result)
    {
        if (s.Length < 3)
        {
            result = default;
            return false;
        }

        var colonIndex = s.IndexOf(':');

        if (colonIndex < 0)
        {
            result = default;
            return false;
        }

        if (int.TryParse(s[..colonIndex], NumberStyles.Integer, provider, out var years) &&
            int.TryParse(s[(colonIndex + 1)..], NumberStyles.Integer, provider, out var months))
        {
            result = new(years, months);
            return true;
        }

        result = default;
        return false;
    }

    public static bool TryParse(
        [NotNullWhen(true)] string? s,
        IFormatProvider? provider,
        out AgeInMonths result)
    {
        return TryParse(s.AsSpan(), provider, out result);
    }

    public override string ToString()
    {
        return ToString(default, default);
    }

    [SkipLocalsInit]
    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        Span<char> destination = stackalloc char[12];
        _ = TryFormat(destination, out var charsWritten, format, formatProvider);
        return new(destination[..charsWritten]);
    }

    public static bool operator <(AgeInMonths left, AgeInMonths right)
    {
        return left.CompareTo(right) < 0;
    }

    public static bool operator <=(AgeInMonths left, AgeInMonths right)
    {
        return left.CompareTo(right) <= 0;
    }

    public static bool operator >(AgeInMonths left, AgeInMonths right)
    {
        return left.CompareTo(right) > 0;
    }

    public static bool operator >=(AgeInMonths left, AgeInMonths right)
    {
        return left.CompareTo(right) >= 0;
    }
}
