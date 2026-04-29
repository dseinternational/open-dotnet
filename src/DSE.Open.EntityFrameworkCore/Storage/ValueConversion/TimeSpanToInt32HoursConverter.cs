// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Storage.ValueConversion;

/// <summary>
/// Converts a <see cref="TimeSpan"/> to an <see cref="int"/> number of hours for storage,
/// rounding to the nearest whole hour.
/// </summary>
public sealed class TimeSpanToInt32HoursConverter : ValueConverter<TimeSpan, int>
{
    /// <summary>
    /// A shared default instance.
    /// </summary>
    public static readonly TimeSpanToInt32HoursConverter Default = new();

    /// <summary>
    /// Initializes a new instance of <see cref="TimeSpanToInt32HoursConverter"/>.
    /// </summary>
    public TimeSpanToInt32HoursConverter() : base(
        t => (int)Math.Round(t.TotalHours, 0),
        m => TimeSpan.FromHours(m))
    {
    }
}
