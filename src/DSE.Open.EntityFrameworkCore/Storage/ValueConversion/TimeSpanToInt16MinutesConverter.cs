// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Storage.ValueConversion;

/// <summary>
/// Converts a <see cref="TimeSpan"/> to a <see cref="short"/> number of minutes for storage,
/// rounding to the nearest whole minute.
/// </summary>
public sealed class TimeSpanToInt16MinutesConverter : ValueConverter<TimeSpan, short>
{
    /// <summary>
    /// A shared default instance.
    /// </summary>
    public static readonly TimeSpanToInt16MinutesConverter Default = new();

    /// <summary>
    /// Initializes a new instance of <see cref="TimeSpanToInt16MinutesConverter"/>.
    /// </summary>
    public TimeSpanToInt16MinutesConverter() : base(
        t => (short)Math.Round(t.TotalMinutes, 0),
        m => TimeSpan.FromMinutes(m))
    {
    }
}
