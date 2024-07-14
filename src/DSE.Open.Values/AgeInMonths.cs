// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using DSE.Open.Values.Text.Json.Serialization;

namespace DSE.Open.Values;

[JsonConverter(typeof(JsonSpanSerializableValueConverter<AgeInMonths, Months>))]
[ComparableValue]
[StructLayout(LayoutKind.Auto)]
public readonly partial struct AgeInMonths : IComparableValue<AgeInMonths, Months>, IUtf8SpanSerializable<AgeInMonths>
{
    private const int MaxYears = 150;
    private const int MaxMonths = MaxYears * 12;

    private const int MaxAsciiLength = 3 + 1 + 2; // "149:11"

    public static readonly AgeInMonths Zero;

    public static int MaxSerializedCharLength => MaxAsciiLength;

    public static int MaxSerializedByteLength => MaxAsciiLength;

    public AgeInMonths(int years, int months)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(years);
        ArgumentOutOfRangeException.ThrowIfNegative(months);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(years, MaxYears);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(months, MaxMonths);

        checked
        {
            _value = Months.FromValue((years * 12) + months);
        }
    }

    public AgeInMonths(int totalMonths)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(totalMonths);
        _value = Months.FromValue(totalMonths);
    }

    /// <summary>
    /// The total number of months represented by the value.
    /// </summary>
    public int TotalMonths => _value;

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
        return _value.CompareTo(other._value);
    }

    public static bool IsValidValue(Months value)
    {
        return value >= 0 && value <= MaxMonths;
    }

    public static int ConvertTo(AgeInMonths value)
    {
        return value._value;
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

    public static bool TryParse(
        ReadOnlySpan<byte> utf8Text,
        IFormatProvider? provider,
        out AgeInMonths result)
    {
        if (utf8Text.IsEmpty)
        {
            result = default;
            return false;
        }

        var splitIndex = utf8Text.IndexOf((byte)':');

        if (splitIndex == 0)
        {
            result = default;
            return false;
        }

        if (utf8Text[splitIndex..].Contains((byte)':'))
        {
            result = default;
            return false;
        }

        var yearsPart = utf8Text[..splitIndex];
        var monthsPart = utf8Text[splitIndex..];

        if (!int.TryParse(yearsPart, NumberStyles.None, NumberFormatInfo.InvariantInfo, out var years) ||
            !int.TryParse(monthsPart, NumberStyles.None, NumberFormatInfo.InvariantInfo, out var months))
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

    public bool TryFormat(
        Span<byte> utf8Destination,
        out int bytesWritten,
        ReadOnlySpan<char> format,
        IFormatProvider? provider = null)
    {
        if (utf8Destination.Length < 3)
        {
            bytesWritten = 0;
            return false;
        }

        var (years, months) = YearsAndMonths();

        if (!years.TryFormat(utf8Destination, out var written, null, CultureInfo.InvariantCulture))
        {
            bytesWritten = 0;
            return false;
        }

        utf8Destination[written] = (byte)':';
        written++;

        if (!months.TryFormat(utf8Destination[written..], out var written2, null, CultureInfo.InvariantCulture))
        {
            bytesWritten = 0;
            return false;
        }

        bytesWritten = written + written2;
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
