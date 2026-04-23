// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections.Concurrent;
using System.Text.Json.Serialization;

namespace DSE.Open.Results;

/// <summary>
/// Metadata carried alongside a <see cref="Result"/>: a per-instance identifier and
/// an open-ended property bag for application-defined context (e.g. tenant, trace,
/// correlation ids).
/// </summary>
public record ResultMetadata
{
    private Guid? _id;

    /// <summary>
    /// A stable identifier for this metadata instance. Lazily assigned to a
    /// fresh <see cref="Guid"/> on first read if not explicitly set.
    /// </summary>
    [JsonPropertyName("id")]
    public Guid Id
    {
        get => _id ??= Guid.NewGuid();
        init => _id = value;
    }

    /// <summary>
    /// An open-ended, thread-safe property bag. Keys are compared case-insensitively
    /// using <see cref="StringComparer.OrdinalIgnoreCase"/>.
    /// </summary>
    [JsonPropertyName("properties")]
    public ConcurrentDictionary<string, object> Properties { get; }
        = new(StringComparer.OrdinalIgnoreCase);
}
