// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using Microsoft.AspNetCore.Http;
using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace DSE.Open.Turnstile.Web;

/// <summary>
/// Provides extension methods for <see cref="ITurnstileClient"/> that integrate with ASP.NET Core.
/// </summary>
public static class TurnstileClientExtensions
{
    /// <summary>
    /// Validates the Turnstile response token submitted with the current
    /// <see cref="HttpContext"/>'s form, using the request's remote IP address.
    /// </summary>
    /// <param name="client">The Turnstile client used to perform validation.</param>
    /// <param name="httpContext">The HTTP context containing the submitted form and connection details.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>The <see cref="ValidationResponse"/> returned by the Turnstile service.</returns>
    [RequiresDynamicCode(WarningMessages.RequiresDynamicCode)]
    [RequiresUnreferencedCode(WarningMessages.RequiresUnreferencedCode)]
    public static Task<ValidationResponse> ValidateAsync(
        this ITurnstileClient client,
        HttpContext httpContext,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(client);
        ArgumentNullException.ThrowIfNull(httpContext);

        _ = httpContext.Request.Form.TryGetValue(WidgetConstants.DefaultResponseFieldName, out var clientResponse);

        var ip = httpContext.Connection.RemoteIpAddress ?? IPAddress.Loopback;

        return client.ValidateAsync(clientResponse.ToString(), ip, cancellationToken);
    }
}
