// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Turnstile;

/// <summary>
/// Options used to configure a <see cref="TurnstileClient"/>.
/// </summary>
public sealed class TurnstileClientOptions
{
    private static readonly Uri s_defaultEndpoint = new("https://challenges.cloudflare.com/turnstile/v0/siteverify");

    /// <summary>
    /// The Turnstile siteverify endpoint that validation requests are posted to.
    /// </summary>
    public Uri Endpoint { get; set; } = s_defaultEndpoint;

    /// <summary>
    /// Public key used to invoke the Turnstile widget on your site.
    /// </summary>
    public string SiteKey { get; set; } = string.Empty;

    /// <summary>
    /// Secret key used, together with the <see cref="SiteKey"/> to validate
    /// the response from the Turnstile widget.
    /// </summary>
    public string SecretKey { get; set; } = string.Empty;
}
