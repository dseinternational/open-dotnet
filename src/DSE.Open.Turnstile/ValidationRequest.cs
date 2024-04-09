// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Net;
using System.Text.Json.Serialization;

namespace DSE.Open.Turnstile;

public sealed record ValidationRequest
{
    [JsonPropertyName("secret")]
    public required string SecretKey { get; init; }

    [JsonPropertyName("response")]
    public required string Response { get; init; }

    [JsonPropertyName("remoteip")]
    public string? ClientIpAddress { get; init; }

    [JsonPropertyName("idempotency_key")]
    public string? IdempotencyKey { get; init; }
}

