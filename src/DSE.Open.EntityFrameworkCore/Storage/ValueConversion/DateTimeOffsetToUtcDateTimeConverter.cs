// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Storage.ValueConversion;

/// <summary>
/// Converts a <see cref="DateTimeOffset"/> to a UTC <see cref="DateTime"/> for storage.
/// </summary>
public sealed class DateTimeOffsetToUtcDateTimeConverter : ValueConverter<DateTimeOffset, DateTime>
{
    /// <summary>
    /// A shared default instance.
    /// </summary>
    public static readonly DateTimeOffsetToUtcDateTimeConverter Default = new();

    /// <summary>
    /// Initializes a new instance of <see cref="DateTimeOffsetToUtcDateTimeConverter"/>.
    /// </summary>
    public DateTimeOffsetToUtcDateTimeConverter()
        : base(v => ToUtcTicks(v), v => FromUtcTicks(v))
    {
    }

    /// <summary>
    /// Converts a <see cref="DateTimeOffset"/> to its UTC <see cref="DateTime"/> storage form.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    /// <returns>The UTC <see cref="DateTime"/>.</returns>
    // keep public for EF Core compiled models
    public static DateTime ToUtcTicks(DateTimeOffset value)
    {
        return value.UtcDateTime;
    }

    /// <summary>
    /// Converts a UTC <see cref="DateTime"/> storage value back to a <see cref="DateTimeOffset"/>
    /// with a zero offset.
    /// </summary>
    /// <param name="value">The stored UTC date and time.</param>
    /// <returns>The reconstructed <see cref="DateTimeOffset"/>.</returns>
    // keep public for EF Core compiled models
    public static DateTimeOffset FromUtcTicks(DateTime value)
    {
        return new(value, TimeSpan.Zero);
    }
}
