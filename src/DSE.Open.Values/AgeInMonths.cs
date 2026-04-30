// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using DSE.Open.Values.Text.Json.Serialization;

namespace DSE.Open.Values;

/// <summary>
/// Represents an age expressed as a total number of whole months.
/// </summary>
[JsonConverter(typeof(JsonStringAgeInMonthsConverter))]
[StructLayout(LayoutKind.Sequential)]
public readonly record struct AgeInMonths : ISpanFormattable, ISpanParsable<AgeInMonths>, IComparable<AgeInMonths>
{
    private const string DefaultFormat = "0";

    /// <summary>
    /// An <see cref="AgeInMonths"/> value of zero months.
    /// </summary>
    public static readonly AgeInMonths Zero;

    /// <summary>
    /// Initialises a new <see cref="AgeInMonths"/> from a number of years and additional months.
    /// </summary>
    public AgeInMonths(int years, int months) : this((years * 12) + months)
    {
    }

    /// <summary>
    /// Initialises a new <see cref="AgeInMonths"/> from a total number of months.
    /// </summary>
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

    /// <summary>
    /// Returns a new value with the specified number of months added.
    /// </summary>
    public AgeInMonths AddMonths(int months)
    {
        return new(TotalMonths + months);
    }

    /// <summary>
    /// Returns a new value with the specified number of years (12 months each) added.
    /// </summary>
    public AgeInMonths AddYears(int years)
    {
        return new(TotalMonths + (years * 12));
    }

    /// <inheritdoc/>
    public int CompareTo(AgeInMonths other)
    {
        return TotalMonths.CompareTo(other.TotalMonths);
    }

    /// <summary>
    /// Tries to format the value into the provided buffer using the default format.
    /// </summary>
    public bool TryFormat(Span<char> destination, out int charsWritten)
    {
        return TryFormat(destination, out charsWritten, default, default);
    }

    /// <summary>
    /// Tries to format the value into the provided buffer using the specified format provider.
    /// </summary>
    public bool TryFormat(
        Span<char> destination,
        out int charsWritten,
        IFormatProvider? provider)
    {
        return TryFormat(destination, out charsWritten, default, provider);
    }

    /// <summary>
    /// Tries to format the value into the provided buffer as <c>years:months</c>.
    /// </summary>
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

    /// <inheritdoc cref="Parse(ReadOnlySpan{char}, IFormatProvider?)"/>
    public static AgeInMonths Parse(ReadOnlySpan<char> s)
    {
        return Parse(s, default);
    }

    /// <inheritdoc/>
    public static AgeInMonths Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
    {
        if (TryParse(s, provider, out var result))
        {
            return result;
        }

        ThrowHelper.ThrowFormatException($"Could not parse {nameof(AgeInMonths)} value: {s}");
        return default;
    }

    /// <inheritdoc cref="Parse(string, IFormatProvider?)"/>
    public static AgeInMonths Parse(string s)
    {
        return Parse(s, default);
    }

    /// <inheritdoc/>
    public static AgeInMonths Parse(string s, IFormatProvider? provider)
    {
        ArgumentNullException.ThrowIfNull(s);
        return Parse(s.AsSpan(), provider);
    }

    /// <inheritdoc cref="TryParse(ReadOnlySpan{char}, IFormatProvider?, out AgeInMonths)"/>
    public static bool TryParse(
        ReadOnlySpan<char> s,
        out AgeInMonths result)
    {
        return TryParse(s, default, out result);
    }

    /// <inheritdoc/>
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

    /// <inheritdoc/>
    public static bool TryParse(
        [NotNullWhen(true)] string? s,
        IFormatProvider? provider,
        out AgeInMonths result)
    {
        return TryParse(s.AsSpan(), provider, out result);
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return ToString(default, default);
    }

    /// <inheritdoc/>
    [SkipLocalsInit]
    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        Span<char> destination = stackalloc char[12];
        _ = TryFormat(destination, out var charsWritten, format, formatProvider);
        return new(destination[..charsWritten]);
    }

    /// <inheritdoc/>
    public static bool operator <(AgeInMonths left, AgeInMonths right)
    {
        return left.CompareTo(right) < 0;
    }

    /// <inheritdoc/>
    public static bool operator <=(AgeInMonths left, AgeInMonths right)
    {
        return left.CompareTo(right) <= 0;
    }

    /// <inheritdoc/>
    public static bool operator >(AgeInMonths left, AgeInMonths right)
    {
        return left.CompareTo(right) > 0;
    }

    /// <inheritdoc/>
    public static bool operator >=(AgeInMonths left, AgeInMonths right)
    {
        return left.CompareTo(right) >= 0;
    }
}
