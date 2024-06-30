// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using DSE.Open.Values.Text.Json.Serialization;

namespace DSE.Open.Values;

[JsonConverter(typeof(JsonSpanSerializableValueConverter<AgeInMonths, int>))]
[ComparableValue]
[StructLayout(LayoutKind.Auto)]
public readonly partial struct AgeInMonths : IComparableValue<AgeInMonths, int>
{
    private const int MaxYears = int.MaxValue / 12;
    private const int MaxMonths = int.MaxValue;

    public static readonly AgeInMonths Zero;

    public static int MaxSerializedCharLength => 12;

    public AgeInMonths(int years, int months)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(years);
        ArgumentOutOfRangeException.ThrowIfNegative(months);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(years, MaxYears);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(months, MaxMonths);

        var maxMonths = int.MaxValue - (years * 12);

        if (months > maxMonths)
        {
            throw new ArgumentOutOfRangeException(nameof(years),
                "The resulting value is outside the range of a AgeInMonths");
        }

        checked
        {
            TotalMonths = (years * 12) + months;
        }
    }

    public AgeInMonths(int totalMonths)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(totalMonths);
        TotalMonths = totalMonths;
    }

    /// <summary>
    /// The total number of months represented by the value.
    /// </summary>
    public int TotalMonths { get; }

    /// <summary>
    /// The total number of years and months represented by the value.
    /// </summary>
    /// <example>
    /// 13 months would return 1 year and 1 month.
    /// </example>
    public (int Years, int Months) YearsAndMonths()
    {
        var years = TotalMonths / 12;
        var months = TotalMonths - (years * 12);
        return (years, months);
    }

    /// <summary>
    /// Adds the specified number of months to the current value.
    /// </summary>
    /// <param name="months"></param>
    /// <returns></returns>
    /// <exception cref="OverflowException">The resulting value is outside the range of a <see cref="AgeInMonths"/>.</exception>
    public AgeInMonths AddMonths(int months)
    {
        checked
        {
            return new AgeInMonths(TotalMonths + months);
        }
    }

    /// <summary>
    /// Adds the specified number of years to the current value.
    /// </summary>
    /// <param name="years"></param>
    /// <returns></returns>
    /// <exception cref="OverflowException">The resulting value is outside the range of a <see cref="AgeInMonths"/>.</exception>
    public AgeInMonths AddYears(int years)
    {
        checked
        {
            return new AgeInMonths(TotalMonths + (years * 12));
        }
    }

    public int CompareTo(AgeInMonths other)
    {
        return TotalMonths.CompareTo(other.TotalMonths);
    }

    public static bool IsValidValue(int value)
    {
        return value >= 0;
    }

    public static int ConvertTo(AgeInMonths value)
    {
        return value.TotalMonths;
    }

    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out AgeInMonths result)
    {
        if (s.IsEmpty)
        {
            result = default;
            return false;
        }

        Span<Range> ranges = stackalloc Range[3];

        var parts = s.Split(ranges, ':', StringSplitOptions.RemoveEmptyEntries);

        if (parts != 2)
        {
            result = default;
            return false;
        }

        if (!int.TryParse(s[ranges[0]], NumberStyles.None, NumberFormatInfo.InvariantInfo, out var years) ||
            !int.TryParse(s[ranges[1]], NumberStyles.None, NumberFormatInfo.InvariantInfo, out var months))
        {
            result = default;
            return false;
        }

        result = new AgeInMonths(years, months);
        return true;
    }

    public bool TryFormat(
        Span<char> destination,
        out int charsWritten,
        ReadOnlySpan<char> format,
        IFormatProvider? provider = null)
    {
        if (destination.Length < 3)
        {
            charsWritten = 0;
            return false;
        }

        var (years, months) = YearsAndMonths();

        if (!years.TryFormat(destination, out var written, null, CultureInfo.InvariantCulture))
        {
            charsWritten = 0;
            return false;
        }

        destination[written] = ':';
        written++;

        if (!months.TryFormat(destination[written..], out var written2, null, CultureInfo.InvariantCulture))
        {
            charsWritten = 0;
            return false;
        }

        charsWritten = written + written2;
        return true;
    }

    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        Span<char> buffer = stackalloc char[MaxSerializedCharLength];

        if (!TryFormat(buffer, out var charsWritten, null, null))
        {
            throw new UnreachableException("The buffer was not large enough to hold the formatted value.");
        }

        return new string(buffer[..charsWritten]);
    }
}
