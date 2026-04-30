// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;

namespace DSE.Open.Time;

/// <summary>
/// Represents a duration expressed as a count of a single <see cref="TimePeriod"/> unit.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public readonly record struct Duration
{
    /// <summary>
    /// A duration of zero with no associated period.
    /// </summary>
    public static readonly Duration None;

    /// <summary>
    /// A duration of one minute.
    /// </summary>
    public static readonly Duration OneMinute = Minutes(1);

    /// <summary>
    /// A duration of one hour.
    /// </summary>
    public static readonly Duration OneHour = Hours(1);

    /// <summary>
    /// A duration of one day.
    /// </summary>
    public static readonly Duration OneDay = Days(1);

    /// <summary>
    /// A duration of one month.
    /// </summary>
    public static readonly Duration OneMonth = Months(1);

    /// <summary>
    /// A duration of one year.
    /// </summary>
    public static readonly Duration OneYear = Years(1);

    /// <summary>
    /// Initialises a new <see cref="Duration"/> with the specified period and count.
    /// </summary>
    [JsonConstructor]
    public Duration(TimePeriod period, int count)
    {
        Period = period;
        Count = count;
    }

    /// <summary>
    /// The unit of time the duration is measured in.
    /// </summary>
    [JsonPropertyName("period")]
    public TimePeriod Period { get; }

    /// <summary>
    /// The number of <see cref="Period"/> units that make up the duration.
    /// </summary>
    [JsonPropertyName("count")]
    public int Count { get; }

    /// <summary>
    /// Creates a duration of <paramref name="count"/> minutes.
    /// </summary>
    public static Duration Minutes(int count)
    {
        return new(TimePeriod.Minute, count);
    }

    /// <summary>
    /// Creates a duration of <paramref name="count"/> hours.
    /// </summary>
    public static Duration Hours(int count)
    {
        return new(TimePeriod.Hour, count);
    }

    /// <summary>
    /// Creates a duration of <paramref name="count"/> days.
    /// </summary>
    public static Duration Days(int count)
    {
        return new(TimePeriod.Day, count);
    }

    /// <summary>
    /// Creates a duration of <paramref name="count"/> months.
    /// </summary>
    public static Duration Months(int count)
    {
        return new(TimePeriod.Month, count);
    }

    /// <summary>
    /// Creates a duration of <paramref name="count"/> years.
    /// </summary>
    public static Duration Years(int count)
    {
        return new(TimePeriod.Year, count);
    }

    /// <summary>
    /// Adds this duration to the specified <see cref="DateTimeOffset"/> using the appropriate
    /// calendar method for <see cref="Period"/>.
    /// </summary>
    public DateTimeOffset AddTo(DateTimeOffset dateTime)
    {
        if (Period == TimePeriod.Minute)
        {
            return dateTime.AddMinutes(Count);
        }

        if (Period == TimePeriod.Hour)
        {
            return dateTime.AddHours(Count);
        }

        if (Period == TimePeriod.Day)
        {
            return dateTime.AddDays(Count);
        }

        if (Period == TimePeriod.Month)
        {
            return dateTime.AddMonths(Count);
        }

        if (Period == TimePeriod.Year)
        {
            return dateTime.AddYears(Count);
        }

        Debug.Assert(Period == TimePeriod.None);
        return dateTime;
    }

    /// <summary>
    /// Adds this duration to the specified <see cref="DateTime"/> using the appropriate
    /// calendar method for <see cref="Period"/>.
    /// </summary>
    public DateTime AddTo(DateTime dateTime)
    {
        if (Period == TimePeriod.Minute)
        {
            return dateTime.AddMinutes(Count);
        }

        if (Period == TimePeriod.Hour)
        {
            return dateTime.AddHours(Count);
        }

        if (Period == TimePeriod.Day)
        {
            return dateTime.AddDays(Count);
        }

        if (Period == TimePeriod.Month)
        {
            return dateTime.AddMonths(Count);
        }

        if (Period == TimePeriod.Year)
        {
            return dateTime.AddYears(Count);
        }

        Debug.Assert(Period == TimePeriod.None);
        return dateTime;
    }

    /// <summary>
    /// Returns the result of adding <paramref name="duration"/> to <paramref name="dateTime"/>.
    /// </summary>
    public static DateTimeOffset operator +(DateTimeOffset dateTime, Duration duration)
    {
        return duration.AddTo(dateTime);
    }

    /// <summary>
    /// Returns the result of adding <paramref name="duration"/> to <paramref name="dateTime"/>.
    /// </summary>
    public static DateTime operator +(DateTime dateTime, Duration duration)
    {
        return duration.AddTo(dateTime);
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        if (Period == TimePeriod.None)
        {
            return Period.ToString();
        }

        return Count is 1 or -1
            ? Count.ToStringInvariant() + ' ' + Period
            : Count.ToStringInvariant() + ' ' + Period + 's';
    }

    /// <summary>
    /// Not implemented.
    /// </summary>
    /// <exception cref="NotImplementedException">Always thrown.</exception>
    public static DateTimeOffset Add(Duration left, Duration right)
    {
        throw new NotImplementedException();
    }
}
