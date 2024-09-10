// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using DSE.Open.Values.Text.Json.Serialization;

namespace DSE.Open.Values;

/// <summary>
/// A value that identifies a date (day) or just a year. Used for birth date in Open ID connect spec.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
[JsonConverter(typeof(JsonStringYearDateConverter))]
public readonly record struct YearDate : IComparable<YearDate>, ISpanParsable<YearDate>, ISpanFormattable
{
    private readonly DateOnly _date;
    private readonly bool _hasDayAndMonth;

    public static readonly YearDate Empty;

    /// <summary>
    /// Initialises a new value with a year value only.
    /// </summary>
    /// <param name="year"></param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="year"/> is less than that
    /// of <see cref="DateOnly.MinValue"/>.</exception>
    public YearDate(int year)
    {
        _date = new(year, 1, 1);
        _hasDayAndMonth = false;
    }

    /// <summary>
    /// Initialises a new value with a year, month and day.
    /// </summary>
    /// <param name="year"></param>
    /// <param name="month"></param>
    /// <param name="day"></param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="year"/> is less than that
    /// of <see cref="DateOnly.MinValue"/>, or <paramref name="month"/> or <paramref name="day"/>
    /// are outside the range of values permitted by <see cref="DateTime"/>.</exception>
    public YearDate(int year, int month, int day)
    {
        _date = new(year, month, day);
        _hasDayAndMonth = true;
    }

    /// <summary>
    /// Initialises a new value with the year, month and day configured from the <see cref="DateOnly"/> value.
    /// </summary>
    /// <param name="date"></param>
    public YearDate(DateOnly date)
    {
        _date = date;
        _hasDayAndMonth = true;
    }

    /// <summary>
    /// Initialises a new value with the year, month and day configured from the <see cref="DateTime"/> value.
    /// </summary>
    /// <param name="date"></param>
    public YearDate(DateTime date)
    {
        _date = DateOnly.FromDateTime(date);
        _hasDayAndMonth = true;
    }

    public bool HasDayAndMonth => _hasDayAndMonth;

    public bool HasYearOnly => !HasDayAndMonth;

    public int Year => _date.Year;

    public int? Month => HasDayAndMonth ? _date.Month : null;

    public int? Day => HasDayAndMonth ? _date.Day : null;

    public int? DayNumber => HasDayAndMonth ? _date.DayNumber : null;

    public DayOfWeek? DayOfWeek => HasDayAndMonth ? _date.DayOfWeek : null;

    public int? DayOfYear => HasDayAndMonth ? _date.DayOfYear : null;

    public int CompareTo(YearDate other)
    {
        return _date.CompareTo(other._date);
    }

    public override string ToString()
    {
        return ToString(null, null);
    }

    [SkipLocalsInit]
    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        Span<char> buffer = stackalloc char[25]; // 25 is the length of DateOnly.MaxValue.ToLongDateString()
        _ = TryFormat(buffer, out var charsWritten, format, formatProvider);
        return buffer[..charsWritten].ToString();
    }

    public bool TryFormat(
        Span<char> destination,
        out int charsWritten,
        ReadOnlySpan<char> format,
        IFormatProvider? provider)
    {
        if (format.IsEmpty)
        {
            format = "yyyy-MM-dd";
        }

        return _hasDayAndMonth
            ? _date.TryFormat(destination, out charsWritten, format, provider)
            : _date.Year.TryFormat(destination, out charsWritten, "0000", provider);
    }

    public static YearDate Parse(string s)
    {
        return Parse(s, null);
    }

    public static YearDate Parse(string s, IFormatProvider? provider)
    {
        ArgumentNullException.ThrowIfNull(s);
        return Parse(s.AsSpan(), provider);
    }

    public static YearDate Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
    {
        if (TryParse(s, provider, out var result))
        {
            return result;
        }

        ThrowHelper.ThrowFormatException($"Could not parse {nameof(YearDate)} value: {s}");
        return default; // unreachable
    }

    public static bool TryParse(string? s, out YearDate result)
    {
        return TryParse(s, null, out result);
    }

    public static bool TryParse(string? s, IFormatProvider? provider, out YearDate result)
    {
        if (s is null)
        {
            result = Empty;
            return false;
        }

        return TryParse(s.AsSpan(), provider, out result);
    }

    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out YearDate result)
    {
        switch (s.Length)
        {
            case 0:
                result = Empty;
                return true;
            case 4 when int.TryParse(s, NumberStyles.Integer, CultureInfo.InvariantCulture, out var year):
                result = new(year);
                return true;
            case 4:
                result = default;
                return false;
        }

        if (DateTime.TryParse(s, CultureInfo.InvariantCulture, DateTimeStyles.None, out var dateTime))
        {
            result = new(dateTime);
            return true;
        }

        result = default;
        return false;
    }

    public static bool operator <(YearDate left, YearDate right)
    {
        return left.CompareTo(right) < 0;
    }

    public static bool operator <=(YearDate left, YearDate right)
    {
        return left.CompareTo(right) <= 0;
    }

    public static bool operator >(YearDate left, YearDate right)
    {
        return left.CompareTo(right) > 0;
    }

    public static bool operator >=(YearDate left, YearDate right)
    {
        return left.CompareTo(right) >= 0;
    }
}
