// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Hashing;

namespace DSE.Open;

/// <summary>
/// Extension methods for <see cref="DateTime"/>.
/// </summary>
public static class DateTimeExtensions
{
    /// <summary>
    /// Returns a <see cref="DateTime"/> representing midnight at the start of the same
    /// calendar day as <paramref name="value"/>.
    /// </summary>
    public static DateTime ToStartOfDay(this DateTime value)
    {
        return new(value.Year, value.Month, value.Day, 0, 0, 0, value.Kind);
    }

    /// <summary>
    /// Returns a <see cref="DateTime"/> representing the last millisecond of the same
    /// calendar day as <paramref name="value"/> (23:59:59.999).
    /// </summary>
    public static DateTime ToEndOfDay(this DateTime value)
    {
        return new DateTime(value.Year, value.Month, value.Day, 0, 0, 0, value.Kind)
            .AddTicks(TimeSpan.TicksPerDay - 1);
    }

    /// <summary>
    /// Returns the start of the day for <paramref name="value"/>, or <see langword="null"/>
    /// if <paramref name="value"/> is <see langword="null"/>.
    /// </summary>
    public static DateTime? ToStartOfDay(this DateTime? value)
    {
        return value?.ToStartOfDay();
    }

    /// <summary>
    /// Returns the end of the day for <paramref name="value"/>, or <see langword="null"/>
    /// if <paramref name="value"/> is <see langword="null"/>.
    /// </summary>
    public static DateTime? ToEndOfDay(this DateTime? value)
    {
        return value?.ToEndOfDay();
    }

    /// <summary>
    /// Gives a <see cref="DateTime" /> value of kind <see cref="DateTimeKind.Utc" />,
    /// converting the initial value if it is of kind <seealso cref="DateTimeKind.Local" />.
    /// If the initial value is of kind <seealso cref="DateTimeKind.Unspecified" /> it is
    /// assumed to be a value in UTC and specified as such.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static DateTime ToUtcDateTime(this DateTime value)
    {
        return value.Kind switch
        {
            DateTimeKind.Utc => value,
            DateTimeKind.Local => value.ToUniversalTime(),
            DateTimeKind.Unspecified => DateTime.SpecifyKind(value, DateTimeKind.Utc),
            _ => throw new ArgumentOutOfRangeException(nameof(value), "Unsupported DateTimeKind.")
        };
    }

    /// <summary>
    /// Formats the value using the ISO 8601 "round-trip" (<c>"o"</c>) format specifier with the
    /// invariant culture.
    /// </summary>
    public static string ToIso8601String(this DateTime value)
    {
        return value.ToString("o", CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Truncates a <see cref="DateTime" /> value to the specified <see cref="DateTimeTruncation" />.
    /// </summary>
    public static DateTime Truncate(this DateTime value, DateTimeTruncation truncation)
    {
        return truncation switch
        {
            DateTimeTruncation.Millisecond => value.AddTicks(-(value.Ticks % TimeSpan.TicksPerMillisecond)),
            DateTimeTruncation.Second => value.AddTicks(-(value.Ticks % TimeSpan.TicksPerSecond)),
            DateTimeTruncation.Minute => value.AddTicks(-(value.Ticks % TimeSpan.TicksPerMinute)),
            DateTimeTruncation.Hour => value.AddTicks(-(value.Ticks % TimeSpan.TicksPerHour)),
            DateTimeTruncation.Day => value.AddTicks(-(value.Ticks % TimeSpan.TicksPerDay)),
            DateTimeTruncation.Month => new(value.Year, value.Month, 1, 0, 0, 0, value.Kind),
            DateTimeTruncation.Year => new(value.Year, 1, 1, 0, 0, 0, value.Kind),
            _ => ThrowHelper.ThrowArgumentOutOfRangeException<DateTime>(nameof(truncation), $"Unsupported {nameof(DateTimeTruncation)}")
        };
    }

    /// <summary>
    /// Computes a 64-bit hash code that is stable across application runs and process
    /// architectures.
    /// </summary>
    public static ulong GetRepeatableHashCode(this DateTime value)
    {
        return RepeatableHash64Provider.Default.GetRepeatableHashCode(value);
    }
}
