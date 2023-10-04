// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json.Serialization;
using DSE.Open.Serialization.DataTransfer;

namespace DSE.Open.Results;

public record ResultMetadata : ImmutableDataTransferObject
{
    [JsonPropertyName("properties")]
    public IDictionary<string, string> Properties { get; } = new Dictionary<string, string>();
}
