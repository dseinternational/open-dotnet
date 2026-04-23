// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Hashing;
using NodaTime;

namespace DSE.Open;

/// <summary>
/// Extension methods for <see cref="DateTimeOffset"/>.
/// </summary>
public static class DateTimeOffsetExtensions
{
    /// <summary>
    /// Truncates the value to the specified granularity. See <see cref="DateTimeTruncation"/>
    /// for the set of supported truncation levels.
    /// </summary>
    /// <param name="dateTime">The value to truncate.</param>
    /// <param name="dateTimeTruncation">The granularity.</param>
    public static DateTimeOffset Truncate(this DateTimeOffset dateTime, DateTimeTruncation dateTimeTruncation)
    {
        return dateTimeTruncation switch
        {
            DateTimeTruncation.Millisecond => dateTime.AddTicks(-dateTime.Ticks % TimeSpan.TicksPerMillisecond),
            DateTimeTruncation.Second => dateTime.AddTicks(-dateTime.Ticks % TimeSpan.TicksPerSecond),
            DateTimeTruncation.Minute => dateTime.AddTicks(-dateTime.Ticks % TimeSpan.TicksPerMinute),
            DateTimeTruncation.Hour => dateTime.AddTicks(-dateTime.Ticks % TimeSpan.TicksPerHour),
            DateTimeTruncation.Day => dateTime.AddTicks(-dateTime.Ticks % TimeSpan.TicksPerDay),
            DateTimeTruncation.Month => new(dateTime.Year, dateTime.Month, 1, 0, 0, 0, dateTime.Offset),
            DateTimeTruncation.Year => new(dateTime.Year, 1, 1, 0, 0, 0, dateTime.Offset),
            _ => ThrowHelper.ThrowArgumentOutOfRangeException<DateTimeOffset>(nameof(dateTimeTruncation), $"Invalid {nameof(DateTimeTruncation)}.")
        };
    }

    /// <summary>
    /// Formats the value using the ISO 8601 "round-trip" (<c>"o"</c>) format specifier with the
    /// invariant culture.
    /// </summary>
    public static string ToIso8601String(this DateTimeOffset value)
    {
        return value.ToString("o", CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Converts the value to a NodaTime <see cref="Instant"/>.
    /// </summary>
    public static Instant ToInstant(this DateTimeOffset value)
    {
        return Instant.FromDateTimeOffset(value);
    }

    /// <summary>
    /// Converts the value to a NodaTime <see cref="ZonedDateTime"/> in the specified time zone.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    /// <param name="dateTimeZone">The target time zone.</param>
    /// <exception cref="ArgumentNullException"><paramref name="dateTimeZone"/> is <see langword="null"/>.</exception>
    public static ZonedDateTime ToZonedDateTime(this DateTimeOffset value, DateTimeZone dateTimeZone)
    {
        ArgumentNullException.ThrowIfNull(dateTimeZone);
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

    /// <summary>
    /// Computes a 64-bit hash code that is stable across application runs and process
    /// architectures.
    /// </summary>
    public static ulong GetRepeatableHashCode(this DateTimeOffset value)
    {
        return RepeatableHash64Provider.Default.GetRepeatableHashCode(value);
    }
}
