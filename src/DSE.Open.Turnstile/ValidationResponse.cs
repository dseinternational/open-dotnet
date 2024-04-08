// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json.Serialization;
using DSE.Open.Collections.Generic;

namespace DSE.Open.Turnstile;

/// <summary>
/// Encapsulates the response from the Cloudflare Turnstile service.
/// </summary>
/// <remarks>
/// <see href="https://developers.cloudflare.com/turnstile/get-started/server-side-validation/"/>
/// </remarks>
public sealed record ValidationResponse
{
    /// <summary>
    /// Indicates whether the validation was successful or not.
    /// </summary>
    [JsonPropertyName("success")]
    public required bool Success { get; init; }

    /// <summary>
    /// ISO timestamp for the time the challenge was solved.
    /// </summary>
    /// <remarks>
    /// <see href="https://developers.cloudflare.com/turnstile/get-started/server-side-validation/"/>
    /// </remarks>
    [JsonPropertyName("challenge_ts")]
    public DateTimeOffset Timestamp { get; init; }

    /// <summary>
    /// The hostname for which the challenge was served
    /// </summary>
    /// <remarks>
    /// <see href="https://developers.cloudflare.com/turnstile/get-started/server-side-validation/"/>
    /// </remarks>
    [JsonPropertyName("hostname")]
    public string? Hostname { get; init; }

    /// <summary>
    /// Widget identifier passed to the widget on the client side. This is used to differentiate
    /// widgets using the same sitekey in analytics. Its integrity is protected by modifications
    /// from an attacker. It is recommended to validate that the action matches an expected value.
    /// </summary>
    /// <remarks>
    /// <see href="https://developers.cloudflare.com/turnstile/get-started/server-side-validation/"/>
    /// </remarks>
    [JsonPropertyName("action")]
    public string? Action { get; init; }

    /// <summary>
    /// Data passed to the widget on the client side. This can be used by the customer to convey
    /// state. Its integrity is protected by modifications from an attacker.
    /// </summary>
    /// <remarks>
    /// <see href="https://developers.cloudflare.com/turnstile/get-started/server-side-validation/"/>
    /// </remarks>
    [JsonPropertyName("cdata")]
    public string? Data { get; init; }

    /// <summary>
    /// A list of errors that occurred.
    /// </summary>
    /// <remarks>
    /// <see href="https://developers.cloudflare.com/turnstile/get-started/server-side-validation/"/>
    /// </remarks>    [JsonPropertyName("error-codes")]
    public ReadOnlyValueCollection<string> ErrorCodes { get; init; } = [];
}
