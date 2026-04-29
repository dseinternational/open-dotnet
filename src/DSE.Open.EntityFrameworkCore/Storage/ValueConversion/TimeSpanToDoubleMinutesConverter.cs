// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Storage.ValueConversion;

/// <summary>
/// Converts a <see cref="TimeSpan"/> to a <see cref="double"/> number of minutes for storage.
/// </summary>
public sealed class TimeSpanToDoubleMinutesConverter : ValueConverter<TimeSpan, double>
{
    /// <summary>
    /// A shared default instance.
    /// </summary>
    public static readonly TimeSpanToDoubleMinutesConverter Default = new();

    /// <summary>
    /// Initializes a new instance of <see cref="TimeSpanToDoubleMinutesConverter"/>.
    /// </summary>
    public TimeSpanToDoubleMinutesConverter() : base(
        t => t.TotalMinutes,
        m => TimeSpan.FromMinutes(m))
    {
    }
}
