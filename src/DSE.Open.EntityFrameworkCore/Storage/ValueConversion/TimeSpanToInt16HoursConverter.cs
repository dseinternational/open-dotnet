// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Storage.ValueConversion;

/// <summary>
/// Converts a <see cref="TimeSpan"/> to a <see cref="short"/> number of hours for storage,
/// rounding to the nearest whole hour.
/// </summary>
public sealed class TimeSpanToInt16HoursConverter : ValueConverter<TimeSpan, short>
{
    /// <summary>
    /// A shared default instance.
    /// </summary>
    public static readonly TimeSpanToInt16HoursConverter Default = new();

    /// <summary>
    /// Initializes a new instance of <see cref="TimeSpanToInt16HoursConverter"/>.
    /// </summary>
    public TimeSpanToInt16HoursConverter() : base(
        t => (short)Math.Round(t.TotalHours, 0),
        m => TimeSpan.FromHours(m))
    {
    }
}
