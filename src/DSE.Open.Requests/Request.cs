// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json.Serialization;
using DSE.Open.Serialization.DataTransfer;

namespace DSE.Open.Requests;

/// <summary>
/// Carries the specification for a command or query from a client
/// to a remote system.
/// </summary>
public record Request : ImmutableDataTransferObject
{
    private Guid? _requestId;

    [JsonPropertyName("request_id")]
    public Guid RequestId
    {
        get => _requestId ??= Guid.NewGuid();
        init => _requestId = value;
    }
}
