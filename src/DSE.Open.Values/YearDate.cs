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

    /// <summary>
    /// The default empty <see cref="YearDate"/>.
    /// </summary>
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

    /// <summary>
    /// Gets a value indicating whether the value carries a full date (day and month) in addition to the year.
    /// </summary>
    public bool HasDayAndMonth => _hasDayAndMonth;

    /// <summary>
    /// Gets a value indicating whether the value carries only a year (no day or month).
    /// </summary>
    public bool HasYearOnly => !HasDayAndMonth;

    /// <summary>
    /// Gets the year component.
    /// </summary>
    public int Year => _date.Year;

    /// <summary>
    /// Gets the month component, or <see langword="null"/> if the value carries only a year.
    /// </summary>
    public int? Month => HasDayAndMonth ? _date.Month : null;

    /// <summary>
    /// Gets the day component, or <see langword="null"/> if the value carries only a year.
    /// </summary>
    public int? Day => HasDayAndMonth ? _date.Day : null;

    /// <summary>
    /// Gets the day number (days since 0001-01-01), or <see langword="null"/> if the value carries only a year.
    /// </summary>
    public int? DayNumber => HasDayAndMonth ? _date.DayNumber : null;

    /// <summary>
    /// Gets the day of the week, or <see langword="null"/> if the value carries only a year.
    /// </summary>
    public DayOfWeek? DayOfWeek => HasDayAndMonth ? _date.DayOfWeek : null;

    /// <summary>
    /// Gets the day of the year, or <see langword="null"/> if the value carries only a year.
    /// </summary>
    public int? DayOfYear => HasDayAndMonth ? _date.DayOfYear : null;

    /// <inheritdoc/>
    public int CompareTo(YearDate other)
    {
        return _date.CompareTo(other._date);
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return ToString(null, null);
    }

    /// <summary>
    /// Returns the underlying date as a <see cref="DateOnly"/>. When the value has only a year, the day and month are <c>1</c>.
    /// </summary>
    public DateOnly ToDateOnly()
    {
        return _date;
    }

    /// <summary>
    /// Returns the underlying date as a <see cref="DateTime"/> at midnight. When the value has only a year, the day and month are <c>1</c>.
    /// </summary>
    public DateTime ToDateTime()
    {
        return _date.ToDateTime(TimeOnly.MinValue);
    }

    /// <inheritdoc/>
    [SkipLocalsInit]
    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        Span<char> buffer = stackalloc char[25]; // 25 is the length of DateOnly.MaxValue.ToLongDateString()
        _ = TryFormat(buffer, out var charsWritten, format, formatProvider);
        return buffer[..charsWritten].ToString();
    }

    /// <inheritdoc/>
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

    /// <inheritdoc cref="Parse(string, IFormatProvider?)"/>
    public static YearDate Parse(string s)
    {
        return Parse(s, null);
    }

    /// <inheritdoc/>
    public static YearDate Parse(string s, IFormatProvider? provider)
    {
        ArgumentNullException.ThrowIfNull(s);
        return Parse(s.AsSpan(), provider);
    }

    /// <inheritdoc/>
    public static YearDate Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
    {
        if (TryParse(s, provider, out var result))
        {
            return result;
        }

        ThrowHelper.ThrowFormatException($"Could not parse {nameof(YearDate)} value: {s}");
        return default; // unreachable
    }

    /// <inheritdoc cref="TryParse(string?, IFormatProvider?, out YearDate)"/>
    public static bool TryParse(string? s, out YearDate result)
    {
        return TryParse(s, null, out result);
    }

    /// <inheritdoc/>
    public static bool TryParse(string? s, IFormatProvider? provider, out YearDate result)
    {
        if (s is null)
        {
            result = Empty;
            return false;
        }

        return TryParse(s.AsSpan(), provider, out result);
    }

    /// <inheritdoc/>
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
            default:
                break;
        }

        if (DateTime.TryParse(s, CultureInfo.InvariantCulture, DateTimeStyles.None, out var dateTime))
        {
            result = new(dateTime);
            return true;
        }

        result = default;
        return false;
    }

    /// <inheritdoc/>
    public static bool operator <(YearDate left, YearDate right)
    {
        return left.CompareTo(right) < 0;
    }

    /// <inheritdoc/>
    public static bool operator <=(YearDate left, YearDate right)
    {
        return left.CompareTo(right) <= 0;
    }

    /// <inheritdoc/>
    public static bool operator >(YearDate left, YearDate right)
    {
        return left.CompareTo(right) > 0;
    }

    /// <inheritdoc/>
    public static bool operator >=(YearDate left, YearDate right)
    {
        return left.CompareTo(right) >= 0;
    }
}
