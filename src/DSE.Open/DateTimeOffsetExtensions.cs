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
}
