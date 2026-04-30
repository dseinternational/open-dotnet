// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using DSE.Open.Text.Json.Serialization;

namespace DSE.Open.Time;

/// <summary>
/// Defines a period of time.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
[JsonConverter(typeof(JsonStringTimePeriodConverter))]
public readonly record struct TimePeriod : IComparable<TimePeriod>
{
    private readonly byte _value;

    /// <summary>
    /// The default, unspecified time period.
    /// </summary>
    public static readonly TimePeriod None;

    /// <summary>
    /// One minute.
    /// </summary>
    public static readonly TimePeriod Minute = new(MinuteValue);

    /// <summary>
    /// One hour.
    /// </summary>
    public static readonly TimePeriod Hour = new(HourValue);

    /// <summary>
    /// One day.
    /// </summary>
    public static readonly TimePeriod Day = new(DayValue);

    /// <summary>
    /// One week.
    /// </summary>
    public static readonly TimePeriod Week = new(WeekValue);

    /// <summary>
    /// One month.
    /// </summary>
    public static readonly TimePeriod Month = new(MonthValue);

    /// <summary>
    /// One year.
    /// </summary>
    public static readonly TimePeriod Year = new(YearValue);

    private const byte MinuteValue = 1;
    private const byte HourValue = 2;
    private const byte DayValue = 3;
    private const byte WeekValue = 4;
    private const byte MonthValue = 5;
    private const byte YearValue = 6;

    private const string NoneLabel = "none";
    private const string MinuteLabel = "minute";
    private const string HourLabel = "hour";
    private const string DayLabel = "day";
    private const string WeekLabel = "week";
    private const string MonthLabel = "month";
    private const string YearLabel = "year";

    private TimePeriod(byte value)
    {
        _value = value;
    }

    /// <inheritdoc/>
    public int CompareTo(TimePeriod other)
    {
        return _value.CompareTo(other._value);
    }

    /// <summary>
    /// Returns <see langword="true"/> if <paramref name="left"/> represents a shorter period than <paramref name="right"/>.
    /// </summary>
    public static bool operator <(TimePeriod left, TimePeriod right)
    {
        return left.CompareTo(right) < 0;
    }

    /// <summary>
    /// Returns <see langword="true"/> if <paramref name="left"/> represents a shorter or equal period to <paramref name="right"/>.
    /// </summary>
    public static bool operator <=(TimePeriod left, TimePeriod right)
    {
        return left.CompareTo(right) <= 0;
    }

    /// <summary>
    /// Returns <see langword="true"/> if <paramref name="left"/> represents a longer period than <paramref name="right"/>.
    /// </summary>
    public static bool operator >(TimePeriod left, TimePeriod right)
    {
        return left.CompareTo(right) > 0;
    }

    /// <summary>
    /// Returns <see langword="true"/> if <paramref name="left"/> represents a longer or equal period to <paramref name="right"/>.
    /// </summary>
    public static bool operator >=(TimePeriod left, TimePeriod right)
    {
        return left.CompareTo(right) >= 0;
    }

    /// <summary>
    /// Returns the lower-case label for this period (for example, <c>"minute"</c>, <c>"hour"</c>, <c>"none"</c>).
    /// </summary>
    public override string ToString()
    {
        return _value switch
        {
            MinuteValue => MinuteLabel,
            HourValue => HourLabel,
            DayValue => DayLabel,
            WeekValue => WeekLabel,
            MonthValue => MonthLabel,
            YearValue => YearLabel,
            _ => NoneLabel
        };
    }

    /// <summary>
    /// Parses one of the recognised period labels (case-insensitive).
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="value"/> is <see langword="null"/>.</exception>
    /// <exception cref="FormatException"><paramref name="value"/> is not a recognised label.</exception>
    public static TimePeriod Parse(string value)
    {
        ArgumentNullException.ThrowIfNull(value);

        return TryParse(value, out var result) ? result : throw new FormatException("Invalid TimePeriod.");
    }

    /// <summary>
    /// Attempts to parse one of the recognised period labels (case-insensitive). Returns <see langword="false"/>
    /// when <paramref name="value"/> is <see langword="null"/>, empty, or unrecognised.
    /// </summary>
    public static bool TryParse([NotNullWhen(true)] string? value, out TimePeriod timePeriod)
    {
        if (!string.IsNullOrEmpty(value))
        {
            switch (value.ToLowerInvariant())
            {
                case MinuteLabel:
                    timePeriod = Minute;
                    return true;
                case HourLabel:
                    timePeriod = Hour;
                    return true;
                case DayLabel:
                    timePeriod = Day;
                    return true;
                case WeekLabel:
                    timePeriod = Week;
                    return true;
                case MonthLabel:
                    timePeriod = Month;
                    return true;
                case YearLabel:
                    timePeriod = Year;
                    return true;
                case NoneLabel:
                    timePeriod = None;
                    return true;
                default:
                    break;
            }
        }

        timePeriod = default;
        return false;
    }
}
