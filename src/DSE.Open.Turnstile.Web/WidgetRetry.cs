// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Turnstile.Web;

/// <summary>
/// Controls whether the Turnstile widget automatically retries to obtain a token on failure.
/// </summary>
public enum WidgetRetry
{
    /// <summary>
    /// The widget retries automatically on failure (default).
    /// </summary>
    Auto,
    /// <summary>
    /// The widget does not retry on failure.
    /// </summary>
    Never
}
