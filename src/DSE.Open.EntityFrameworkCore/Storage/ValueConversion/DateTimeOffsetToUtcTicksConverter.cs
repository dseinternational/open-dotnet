// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Storage.ValueConversion;

/// <summary>
/// Converts a <see cref="DateTimeOffset"/> to its UTC ticks (<see cref="long"/>) for storage.
/// </summary>
public sealed class DateTimeOffsetToUtcTicksConverter : ValueConverter<DateTimeOffset, long>
{
    /// <summary>
    /// A shared default instance.
    /// </summary>
    public static readonly DateTimeOffsetToUtcTicksConverter Default = new();

    /// <summary>
    /// Initializes a new instance of <see cref="DateTimeOffsetToUtcTicksConverter"/>.
    /// </summary>
    public DateTimeOffsetToUtcTicksConverter()
        : base(v => ToUtcTicks(v), v => FromUtcTicks(v))
    {
    }

    /// <summary>
    /// Converts a <see cref="DateTimeOffset"/> to its UTC ticks (<see cref="long"/>) storage form.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    /// <returns>The UTC ticks.</returns>
    // keep public for EF Core compiled models
    public static long ToUtcTicks(DateTimeOffset value)
    {
        return value.UtcTicks;
    }

    /// <summary>
    /// Converts a UTC ticks (<see cref="long"/>) storage value back to a
    /// <see cref="DateTimeOffset"/> with a zero offset.
    /// </summary>
    /// <param name="value">The stored UTC ticks.</param>
    /// <returns>The reconstructed <see cref="DateTimeOffset"/>.</returns>
    // keep public for EF Core compiled models
    public static DateTimeOffset FromUtcTicks(long value)
    {
        return new(value, TimeSpan.Zero);
    }
}
