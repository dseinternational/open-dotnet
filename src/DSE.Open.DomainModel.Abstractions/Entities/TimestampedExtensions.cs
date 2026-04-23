// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.DomainModel.Entities;

/// <summary>
/// Extension methods on <see cref="ITimestamped"/>.
/// </summary>
public static class TimestampedExtensions
{
    /// <summary>
    /// Returns <see langword="true"/> if <paramref name="timestamped"/> has a
    /// non-null <see cref="ITimestamped.Timestamp"/>, indicating it has been
    /// persisted at least once.
    /// </summary>
    /// <param name="timestamped">The object to test.</param>
    /// <exception cref="ArgumentNullException"><paramref name="timestamped"/> is <see langword="null"/>.</exception>
    public static bool IsPersisted(this ITimestamped timestamped)
    {
        ArgumentNullException.ThrowIfNull(timestamped);
        return timestamped.Timestamp.HasValue;
    }
}
