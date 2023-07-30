// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.DomainModel.Abstractions;

public static class TimestampedExtensions
{
    public static bool IsPersisted(this ITimestamped timestamped)
    {
        Guard.IsNotNull(timestamped);
        return timestamped.Timestamp.HasValue;
    }
}
