// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Storage.ValueConversion;

/// <summary>
/// Converts a <see cref="TimeSpan"/> to a <see cref="long"/> number of hours for storage,
/// rounding to the nearest whole hour.
/// </summary>
public sealed class TimeSpanToInt64HoursConverter : ValueConverter<TimeSpan, long>
{
    /// <summary>
    /// A shared default instance.
    /// </summary>
    public static readonly TimeSpanToInt64HoursConverter Default = new();

    /// <summary>
    /// Initializes a new instance of <see cref="TimeSpanToInt64HoursConverter"/>.
    /// </summary>
    public TimeSpanToInt64HoursConverter() : base(
        t => (long)Math.Round(t.TotalHours, 0),
        m => TimeSpan.FromHours(m))
    {
    }
}
