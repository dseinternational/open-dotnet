// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json.Serialization;

namespace DSE.Open.Turnstile;

/// <summary>
/// The request payload posted to the Cloudflare Turnstile siteverify endpoint.
/// </summary>
public sealed record ValidationRequest
{
    /// <summary>
    /// The site's Turnstile secret key.
    /// </summary>
    [JsonPropertyName("secret")]
    public required string SecretKey { get; init; }

    /// <summary>
    /// The Turnstile response token produced by the client-side widget.
    /// </summary>
    [JsonPropertyName("response")]
    public required string Response { get; init; }

    /// <summary>
    /// The optional IP address of the visitor that produced the response.
    /// </summary>
    [JsonPropertyName("remoteip")]
    public string? ClientIpAddress { get; init; }

    /// <summary>
    /// An optional idempotency key used to safely retry a validation request.
    /// </summary>
    [JsonPropertyName("idempotency_key")]
    public string? IdempotencyKey { get; init; }
}

