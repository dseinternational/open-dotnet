// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json.Serialization;
using DSE.Open.Serialization.DataTransfer;
using DSE.Open.Values;

namespace DSE.Open.Requests;

/// <summary>
/// Carries the specification for a command or query from a client
/// to a remote system.
/// </summary>
public record Request : ImmutableDataTransferObject
{
    private string? _requestId;

    /// <summary>
    /// Identifies the request. This should be unique for each request and remain the same if a request is
    /// retried.
    /// </summary>
    [JsonPropertyName("request_id")]
    [JsonPropertyOrder(-900000)]
    public string RequestId
    {
        get => _requestId ??= Identifier.New(20, "req"u8).ToStringInvariant();
        init => _requestId = value;
    }

    /// <summary>
    /// Identifies the source of the request.
    /// </summary>
    [JsonPropertyName("source")]
    [JsonPropertyOrder(-899900)]
    public Uri? Source { get; init; }
}
