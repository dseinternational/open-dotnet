// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open;

public static class DateTimeExtensions
{
    public static DateTime ToStartOfDay(this DateTime value)
        => new(value.Year, value.Month, value.Day, 0, 0, 0);

    public static DateTime ToEndOfDay(this DateTime value)
        => new(value.Year, value.Month, value.Day, 23, 59, 59, 999);

    public static DateTime? ToStartOfDay(this DateTime? value) => value?.ToStartOfDay();

    public static DateTime? ToEndOfDay(this DateTime? value) => value?.ToEndOfDay();

    /// <summary>
    /// Gives a <see cref="DateTime" /> value of kind <see cref="DateTimeKind.Utc" />,
    /// converting the initial value if it is of kind <seealso cref="DateTimeKind.Local" />.
    /// If the initial value is of kind <seealso cref="DateTimeKind.Unspecified" /> it is
    /// assumed to be a value in UTC and specified as such.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static DateTime ToUtcDateTime(this DateTime value) => value.Kind switch
    {
        DateTimeKind.Utc => value,
        DateTimeKind.Local => value.ToUniversalTime(),
        DateTimeKind.Unspecified => DateTime.SpecifyKind(value, DateTimeKind.Utc),
        _ => throw new ArgumentOutOfRangeException(nameof(value), "Unsupported DateTimeKind.")
    };

    public static string ToIso8601String(this DateTime value) => value.ToString("o", CultureInfo.InvariantCulture);

    /// <summary>
    /// Truncates a <see cref="DateTime" /> value to the specified <see cref="DateTimeTruncation" />.
    /// </summary>
    public static DateTime Truncate(this DateTime value, DateTimeTruncation truncation) => truncation switch
    {
        DateTimeTruncation.Millisecond => value.AddTicks(-(value.Ticks % TimeSpan.TicksPerMillisecond)),
        DateTimeTruncation.Second => value.AddTicks(-(value.Ticks % TimeSpan.TicksPerSecond)),
        DateTimeTruncation.Minute => value.AddTicks(-(value.Ticks % TimeSpan.TicksPerMinute)),
        DateTimeTruncation.Hour => value.AddTicks(-(value.Ticks % TimeSpan.TicksPerHour)),
        DateTimeTruncation.Day => value.AddTicks(-(value.Ticks % TimeSpan.TicksPerDay)),
        DateTimeTruncation.Month => new DateTime(value.Year, value.Month, 1, 0, 0, 0, value.Kind),
        DateTimeTruncation.Year => new DateTime(value.Year, 1, 1, 0, 0, 0, value.Kind),
        _ => ThrowHelper.ThrowArgumentOutOfRangeException<DateTime>(nameof(truncation), $"Unsupported {nameof(DateTimeTruncation)}")
    };
}
