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
[StructLayout(LayoutKind.Sequential)]
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

    /// <summary>
    /// Initialises a new <see cref="DateMonthOnly"/> from a <see cref="DateOnly"/>, retaining only the year and month.
    /// </summary>
    public DateMonthOnly(DateOnly date)
    {
        StartOfMonth = new(date.Year, date.Month, 1);
    }

    /// <summary>
    /// Gets the first day of the month represented by this value.
    /// </summary>
    public DateOnly StartOfMonth { get; }

    /// <summary>
    /// Gets the year component of the value.
    /// </summary>
    public int Year => StartOfMonth.Year;

    /// <summary>
    /// Gets the month component of the value.
    /// </summary>
    public int Month => StartOfMonth.Month;

    /// <summary>
    /// Gets the total number of months represented as <c>(Year * 12) + Month</c>.
    /// </summary>
    public int TotalMonths => (Year * 12) + StartOfMonth.Month;

    /// <summary>
    /// Gets the day of the week for the first day of the month.
    /// </summary>
    public DayOfWeek DayOfWeek => StartOfMonth.DayOfWeek;

    /// <summary>
    /// Returns a new value with the specified number of months added.
    /// </summary>
    public DateMonthOnly AddMonths(int months)
    {
        return new(StartOfMonth.AddMonths(months));
    }

    /// <summary>
    /// Returns a new value with the specified number of years added.
    /// </summary>
    public DateMonthOnly AddYears(int years)
    {
        return new(StartOfMonth.AddYears(years));
    }

    /// <inheritdoc/>
    public int CompareTo(DateMonthOnly other)
    {
        return StartOfMonth.CompareTo(other.StartOfMonth);
    }

    /// <summary>
    /// Deconstructs the value into its <paramref name="year"/> and <paramref name="month"/> components.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public void Deconstruct(out int year, out int month)
    {
        year = Year;
        month = Month;
    }

    /// <summary>
    /// Returns the first day of the month as a <see cref="DateOnly"/>.
    /// </summary>
    public DateOnly ToDateOnly()
    {
        return StartOfMonth;
    }

    /// <summary>
    /// Returns the first day of the month at midnight as a <see cref="DateTime"/>.
    /// </summary>
    public DateTime ToDateTime()
    {
        return ToDateOnly().ToDateTime(TimeOnly.MinValue);
    }

    /// <summary>
    /// Creates a <see cref="DateMonthOnly"/> from a <see cref="DateOnly"/> by retaining only the year and month.
    /// </summary>
    public static DateMonthOnly FromDateOnly(DateOnly date)
    {
        return new(date);
    }

    /// <summary>
    /// Creates a <see cref="DateMonthOnly"/> from a <see cref="DateTime"/> by retaining only the year and month.
    /// </summary>
    public static DateMonthOnly FromDateTime(DateTime date)
    {
        return new(DateOnly.FromDateTime(date.Date));
    }

    /// <summary>
    /// Creates a <see cref="DateMonthOnly"/> from a <see cref="DateTimeOffset"/> by retaining only the year and month.
    /// </summary>
    public static DateMonthOnly FromDateTimeOffset(DateTimeOffset date)
    {
        return new(new(date.Year, date.Month, 1));
    }

    /// <inheritdoc cref="Parse(ReadOnlySpan{char}, IFormatProvider?)"/>
    public static DateMonthOnly Parse(ReadOnlySpan<char> s)
    {
        return Parse(s, null);
    }

    /// <inheritdoc/>
    public static DateMonthOnly Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
    {
        return new(DateOnly.Parse(s, provider));
    }

    /// <inheritdoc cref="Parse(string, IFormatProvider?)"/>
    public static DateMonthOnly Parse(string s)
    {
        return Parse(s, null);
    }

    /// <inheritdoc/>
    public static DateMonthOnly Parse(string s, IFormatProvider? provider)
    {
        ArgumentNullException.ThrowIfNull(s);
        return new(DateOnly.Parse(s, provider));
    }

    /// <inheritdoc/>
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

    /// <inheritdoc/>
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

    /// <inheritdoc/>
    public override string ToString()
    {
        return ToString(Format, CultureInfo.InvariantCulture);
    }

    /// <inheritdoc/>
    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        return StartOfMonth.ToString(format, formatProvider);
    }

    /// <summary>
    /// Tries to format the value into the provided buffer using the default <c>yyyy-MM</c> format.
    /// </summary>
    public bool TryFormat(Span<char> destination, out int charsWritten)
    {
        return TryFormat(destination, out charsWritten, Format, CultureInfo.InvariantCulture);
    }

    /// <inheritdoc/>
    public bool TryFormat(
        Span<char> destination,
        out int charsWritten,
        ReadOnlySpan<char> format,
        IFormatProvider? provider)
    {
        return StartOfMonth.TryFormat(destination, out charsWritten, format, provider);
    }

    /// <summary>
    /// Converts a <see cref="DateMonthOnly"/> to a <see cref="DateOnly"/> representing the first day of the month.
    /// </summary>
    public static explicit operator DateOnly(DateMonthOnly dateMonthOnly)
    {
        return dateMonthOnly.StartOfMonth;
    }

    /// <summary>
    /// Converts a <see cref="DateOnly"/> to a <see cref="DateMonthOnly"/>, discarding the day component.
    /// </summary>
    public static explicit operator DateMonthOnly(DateOnly date)
    {
        return new(date);
    }

    /// <summary>
    /// Returns the difference between two <see cref="DateMonthOnly"/> values as an <see cref="AgeInMonths"/>.
    /// </summary>
    public static AgeInMonths operator -(DateMonthOnly a, DateMonthOnly b)
    {
        return new(a.Year - b.Year, a.Month - b.Month);
    }

    /// <inheritdoc/>
    public static bool operator <(DateMonthOnly left, DateMonthOnly right)
    {
        return left.CompareTo(right) < 0;
    }

    /// <inheritdoc/>
    public static bool operator <=(DateMonthOnly left, DateMonthOnly right)
    {
        return left.CompareTo(right) <= 0;
    }

    /// <inheritdoc/>
    public static bool operator >(DateMonthOnly left, DateMonthOnly right)
    {
        return left.CompareTo(right) > 0;
    }

    /// <inheritdoc/>
    public static bool operator >=(DateMonthOnly left, DateMonthOnly right)
    {
        return left.CompareTo(right) >= 0;
    }
}
