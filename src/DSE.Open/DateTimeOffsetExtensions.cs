// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Globalization;
using NodaTime;

namespace DSE.Open;

public static class DateTimeOffsetExtensions
{
    public static DateTimeOffset Truncate(this DateTimeOffset dateTime, DateTimeTruncation dateTimeTruncation) => dateTimeTruncation switch
    {
        DateTimeTruncation.Millisecond => dateTime.AddTicks(-dateTime.Ticks % TimeSpan.TicksPerMillisecond),
        DateTimeTruncation.Second => dateTime.AddTicks(-dateTime.Ticks % TimeSpan.TicksPerSecond),
        DateTimeTruncation.Minute => dateTime.AddTicks(-dateTime.Ticks % TimeSpan.TicksPerMinute),
        DateTimeTruncation.Hour => dateTime.AddTicks(-dateTime.Ticks % TimeSpan.TicksPerHour),
        DateTimeTruncation.Day => dateTime.AddTicks(-dateTime.Ticks % TimeSpan.TicksPerDay),
        DateTimeTruncation.Month => new DateTimeOffset(dateTime.Year, dateTime.Month, 1, 0, 0, 0, dateTime.Offset),
        DateTimeTruncation.Year => new DateTimeOffset(dateTime.Year, 1, 1, 0, 0, 0, dateTime.Offset),
        _ => ThrowHelper.ThrowArgumentOutOfRangeException<DateTimeOffset>(nameof(dateTimeTruncation), $"Invalid {nameof(DateTimeTruncation)}.")
    };

    public static string ToIso8601String(this DateTimeOffset value) =>
        value.ToString("o", CultureInfo.InvariantCulture);

    public static Instant ToInstant(this DateTimeOffset value) => Instant.FromDateTimeOffset(value);

    public static ZonedDateTime ToZonedDateTime(this DateTimeOffset value, DateTimeZone dateTimeZone)
    {
        Guard.IsNotNull(dateTimeZone);
        return value.ToInstant().InZone(dateTimeZone);
    }

    /// <summary>
    /// Determines if the date is within a specified number of months in the past from now.
    /// </summary>
    /// <param name="date"></param>
    /// <param name="months"></param>
    /// <returns><see langword="true"/> if the date and time is greater than or equal to the date and time
    /// the specified number of <paramref name="months"/> ago, otherwise <see langword="false"/>.</returns>
    public static bool IsWithinPastMonths(this DateTimeOffset date, int months)
    {
        var now = DateTimeOffset.UtcNow;
        var past = now.AddMonths(-months);
        return date >= past && date <= now;
    }

    /// <summary>
    /// Determines if the date is within a specified number of months in the past from now,
    /// or is <see langword="null"/>.
    /// </summary>
    /// <param name="date"></param>
    /// <param name="months"></param>
    /// <returns><see langword="true"/> if <paramref name="date"/> is <see langword="null"/> or
    /// if the date and time is greater than or equal to the date and time the specified number of
    /// <paramref name="months"/> ago, otherwise <see langword="false"/>.</returns>
    public static bool IsWithinPastMonthsOrNull(this DateTimeOffset? date, int months)
    {
        if (date is null)
        {
            return true;
        }

        return IsWithinPastMonths(date.Value, months);
    }
}
