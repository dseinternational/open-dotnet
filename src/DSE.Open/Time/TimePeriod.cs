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

    public static readonly TimePeriod None;
    public static readonly TimePeriod Minute = new(MinuteValue);
    public static readonly TimePeriod Hour = new(HourValue);
    public static readonly TimePeriod Day = new(DayValue);
    public static readonly TimePeriod Week = new(WeekValue);
    public static readonly TimePeriod Month = new(MonthValue);
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

    public int CompareTo(TimePeriod other)
    {
        return _value.CompareTo(other._value);
    }

    public static bool operator <(TimePeriod left, TimePeriod right)
    {
        return left.CompareTo(right) < 0;
    }

    public static bool operator <=(TimePeriod left, TimePeriod right)
    {
        return left.CompareTo(right) <= 0;
    }

    public static bool operator >(TimePeriod left, TimePeriod right)
    {
        return left.CompareTo(right) > 0;
    }

    public static bool operator >=(TimePeriod left, TimePeriod right)
    {
        return left.CompareTo(right) >= 0;
    }

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

    public static TimePeriod Parse(string value)
    {
        ArgumentNullException.ThrowIfNull(value);

        return TryParse(value, out var result) ? result : throw new FormatException("Invalid TimePeriod.");
    }

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
