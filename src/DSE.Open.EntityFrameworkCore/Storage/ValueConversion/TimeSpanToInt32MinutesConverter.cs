// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Storage.ValueConversion;

/// <summary>
/// Converts a <see cref="TimeSpan"/> to an <see cref="int"/> number of minutes for storage,
/// rounding to the nearest whole minute.
/// </summary>
public sealed class TimeSpanToInt32MinutesConverter : ValueConverter<TimeSpan, int>
{
    /// <summary>
    /// A shared default instance.
    /// </summary>
    public static readonly TimeSpanToInt32MinutesConverter Default = new();

    /// <summary>
    /// Initializes a new instance of <see cref="TimeSpanToInt32MinutesConverter"/>.
    /// </summary>
    public TimeSpanToInt32MinutesConverter() : base(
        t => (int)Math.Round(t.TotalMinutes, 0),
        m => TimeSpan.FromMinutes(m))
    {
    }
}
