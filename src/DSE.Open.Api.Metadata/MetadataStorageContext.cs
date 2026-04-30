// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections.Concurrent;

namespace DSE.Open.Api.Metadata;

/// <summary>
/// Carries metadata key-value pairs together with the <see cref="MetadataStorage"/>
/// medium they originate from or are destined for.
/// </summary>
public class MetadataStorageContext
{
    /// <summary>
    /// Gets the storage medium associated with this context.
    /// </summary>
    public MetadataStorage StorageType { get; init; }

    /// <summary>
    /// Gets the case-insensitive key-value pairs that hold the metadata.
    /// </summary>
    public ConcurrentDictionary<string, string> Data { get; } = new(StringComparer.InvariantCultureIgnoreCase);
}
