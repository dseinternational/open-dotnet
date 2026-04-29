// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Storage.ValueConversion;

/// <summary>
/// Converts a <see cref="TimeSpan"/> to a <see cref="long"/> number of minutes for storage,
/// rounding to the nearest whole minute.
/// </summary>
public sealed class TimeSpanToInt64MinutesConverter : ValueConverter<TimeSpan, long>
{
    /// <summary>
    /// A shared default instance.
    /// </summary>
    public static readonly TimeSpanToInt64MinutesConverter Default = new();

    /// <summary>
    /// Initializes a new instance of <see cref="TimeSpanToInt64MinutesConverter"/>.
    /// </summary>
    public TimeSpanToInt64MinutesConverter() : base(
        t => (long)Math.Round(t.TotalMinutes, 0),
        m => TimeSpan.FromMinutes(m))
    {
    }
}
