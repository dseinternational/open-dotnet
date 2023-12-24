// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections.Concurrent;
using System.Text.Json.Serialization;
using DSE.Open.Serialization.DataTransfer;

namespace DSE.Open.Requests;

public record RequestMetadata : ImmutableDataTransferObject
{
    private Guid? _id;

    [JsonPropertyName("id")]
    public Guid Id
    {
        get => _id ??= Guid.NewGuid();
        init => _id = value;
    }

    [JsonPropertyName("properties")]
    public ConcurrentDictionary<string, object> Properties { get; }
        = new(StringComparer.InvariantCultureIgnoreCase);
}
