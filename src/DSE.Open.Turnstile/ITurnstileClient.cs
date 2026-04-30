// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace DSE.Open.Turnstile;

/// <summary>
/// Defines a client for performing server-side validation of Cloudflare Turnstile responses.
/// </summary>
public interface ITurnstileClient
{
    /// <summary>
    /// Validates a Turnstile token returned by the client widget against the Cloudflare siteverify endpoint.
    /// </summary>
    /// <param name="clientResponse">The Turnstile response token produced by the client-side widget.</param>
    /// <param name="clientIpAddress">The optional IP address of the visitor that produced the response.</param>
    /// <param name="cancellationToken">A token that can be used to cancel the asynchronous operation.</param>
    /// <returns>The <see cref="ValidationResponse"/> returned by the Turnstile service.</returns>
    [RequiresDynamicCode(WarningMessages.RequiresDynamicCode)]
    [RequiresUnreferencedCode(WarningMessages.RequiresUnreferencedCode)]
    Task<ValidationResponse> ValidateAsync(
        string clientResponse,
        IPAddress? clientIpAddress = null,
        CancellationToken cancellationToken = default);
}
