// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open;

/// <summary>
/// Specifies the granularity to which a <see cref="DateTime"/> or
/// <see cref="DateTimeOffset"/> value should be truncated.
/// </summary>
public enum DateTimeTruncation
{
    /// <summary>Truncate any sub-millisecond component.</summary>
    Millisecond,

    /// <summary>Truncate any sub-second component.</summary>
    Second,

    /// <summary>Truncate any sub-minute component.</summary>
    Minute,

    /// <summary>Truncate any sub-hour component.</summary>
    Hour,

    /// <summary>Truncate to the start of the day.</summary>
    Day,

    /// <summary>Truncate to the first day of the month.</summary>
    Month,

    /// <summary>Truncate to the first day of the year.</summary>
    Year
}
