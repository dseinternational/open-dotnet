// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;

namespace DSE.Open.Time;

[StructLayout(LayoutKind.Auto)]
public readonly record struct Duration
{
    public static readonly Duration None;

    public static readonly Duration OneMinute = Minutes(1);

    public static readonly Duration OneHour = Hours(1);

    public static readonly Duration OneDay = Days(1);

    public static readonly Duration OneMonth = Months(1);

    public static readonly Duration OneYear = Years(1);

    [JsonConstructor]
    public Duration(TimePeriod period, int count)
    {
        Period = period;
        Count = count;
    }

    [JsonPropertyName("period")]
    public TimePeriod Period { get; }

    [JsonPropertyName("count")]
    public int Count { get; }

    public static Duration Minutes(int count)
    {
        return new Duration(TimePeriod.Minute, count);
    }

    public static Duration Hours(int count)
    {
        return new Duration(TimePeriod.Hour, count);
    }

    public static Duration Days(int count)
    {
        return new Duration(TimePeriod.Day, count);
    }

    public static Duration Months(int count)
    {
        return new Duration(TimePeriod.Month, count);
    }

    public static Duration Years(int count)
    {
        return new Duration(TimePeriod.Year, count);
    }

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

    public static DateTimeOffset operator +(DateTimeOffset dateTime, Duration duration)
    {
        return duration.AddTo(dateTime);
    }

    public static DateTime operator +(DateTime dateTime, Duration duration)
    {
        return duration.AddTo(dateTime);
    }

    public override string ToString()
    {
        return Period == TimePeriod.None
            ? Period.ToString()
            : Count is 1 or (-1)
            ? Count.ToStringInvariant() + ' ' + Period.ToString()
            : Count.ToStringInvariant() + ' ' + Period.ToString() + 's';
    }

    public static DateTimeOffset Add(Duration left, Duration right)
    {
        throw new NotImplementedException();
    }
}
