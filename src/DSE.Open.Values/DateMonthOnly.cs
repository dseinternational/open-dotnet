// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using DSE.Open.Values.Text.Json.Serialization;

namespace DSE.Open.Values;

/// <summary>
/// Stores the year and month of a date. Helpful for storing birth dates where otherwise they might be personally identifiable.
/// </summary>
[SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "Not needed")]
[JsonConverter(typeof(JsonStringDateMonthOnlyConverter))]
[StructLayout(LayoutKind.Auto)]
public readonly record struct DateMonthOnly : ISpanFormattable, ISpanParsable<DateMonthOnly>, IComparable<DateMonthOnly>
{
    private const string Format = "yyyy-MM";

    /// <summary>
    /// Gets the earliest possible date that can be created.
    /// </summary>
    public static readonly DateMonthOnly MinValue = new(DateOnly.MinValue);

    /// <summary>
    /// Gets the latest possible date that can be created.
    /// </summary>
    public static readonly DateMonthOnly MaxValue = new(DateOnly.MaxValue);

    public DateMonthOnly(DateOnly date)
    {
        StartOfMonth = new(date.Year, date.Month, 1);
    }

    public DateOnly StartOfMonth { get; }

    public int Year => StartOfMonth.Year;

    public int Month => StartOfMonth.Month;

    public int TotalMonths => (Year * 12) + StartOfMonth.Month;

    public DayOfWeek DayOfWeek => StartOfMonth.DayOfWeek;

    public DateMonthOnly AddMonths(int months)
    {
        return new(StartOfMonth.AddMonths(months));
    }

    public DateMonthOnly AddYears(int years)
    {
        return new(StartOfMonth.AddYears(years));
    }

    public int CompareTo(DateMonthOnly other)
    {
        return StartOfMonth.CompareTo(other.StartOfMonth);
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public void Deconstruct(out int year, out int month)
    {
        year = Year;
        month = Month;
    }

    public DateOnly ToDateOnly()
    {
        return StartOfMonth;
    }

    public DateTime ToDateTime()
    {
        return ToDateOnly().ToDateTime(TimeOnly.MinValue);
    }

    public static DateMonthOnly FromDateOnly(DateOnly date)
    {
        return new(date);
    }

    public static DateMonthOnly FromDateTime(DateTime date)
    {
        return new(DateOnly.FromDateTime(date.Date));
    }

    public static DateMonthOnly FromDateTimeOffset(DateTimeOffset date)
    {
        return new(new(date.Year, date.Month, 1));
    }

    public static DateMonthOnly Parse(ReadOnlySpan<char> s)
    {
        return Parse(s, null);
    }

    public static DateMonthOnly Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
    {
        return new(DateOnly.Parse(s, provider));
    }

    public static DateMonthOnly Parse(string s)
    {
        return Parse(s, null);
    }

    public static DateMonthOnly Parse(string s, IFormatProvider? provider)
    {
        Guard.IsNotNull(s);
        return new(DateOnly.Parse(s, provider));
    }

    public static bool TryParse(
        ReadOnlySpan<char> s,
        IFormatProvider? provider,
        out DateMonthOnly result)
    {
        if (!s.IsEmpty && DateOnly.TryParse(s, provider, out var date))
        {
            result = new(date);
            return true;
        }

        result = default;
        return false;
    }

    public static bool TryParse(
        [NotNullWhen(true)] string? s,
        IFormatProvider? provider,
        out DateMonthOnly result)
    {
        if (!string.IsNullOrEmpty(s) && DateOnly.TryParse(s, provider, out var date))
        {
            result = new(date);
            return true;
        }

        result = default;
        return false;
    }

    public override string ToString()
    {
        return ToString(Format, CultureInfo.InvariantCulture);
    }

    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        return StartOfMonth.ToString(format, formatProvider);
    }

    public bool TryFormat(Span<char> destination, out int charsWritten)
    {
        return TryFormat(destination, out charsWritten, Format, CultureInfo.InvariantCulture);
    }

    public bool TryFormat(
        Span<char> destination,
        out int charsWritten,
        ReadOnlySpan<char> format,
        IFormatProvider? provider)
    {
        return StartOfMonth.TryFormat(destination, out charsWritten, format, provider);
    }

    public static explicit operator DateOnly(DateMonthOnly dateMonthOnly)
    {
        return dateMonthOnly.StartOfMonth;
    }

    public static explicit operator DateMonthOnly(DateOnly date)
    {
        return new(date);
    }

    public static int operator -(DateMonthOnly a, DateMonthOnly b)
    {
        return a.TotalMonths - b.TotalMonths;
    }

    public static bool operator <(DateMonthOnly left, DateMonthOnly right)
    {
        return left.CompareTo(right) < 0;
    }

    public static bool operator <=(DateMonthOnly left, DateMonthOnly right)
    {
        return left.CompareTo(right) <= 0;
    }

    public static bool operator >(DateMonthOnly left, DateMonthOnly right)
    {
        return left.CompareTo(right) > 0;
    }

    public static bool operator >=(DateMonthOnly left, DateMonthOnly right)
    {
        return left.CompareTo(right) >= 0;
    }
}
