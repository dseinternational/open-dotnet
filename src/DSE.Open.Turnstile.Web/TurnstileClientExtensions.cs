// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using Microsoft.AspNetCore.Http;
using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace DSE.Open.Turnstile.Web;

public static class TurnstileClientExtensions
{
    [RequiresDynamicCode(WarningMessages.RequiresDynamicCode)]
    [RequiresUnreferencedCode(WarningMessages.RequiresUnreferencedCode)]
    public static Task<ValidationResponse> ValidateAsync(
        this ITurnstileClient client,
        HttpContext httpContext,
        CancellationToken cancellationToken = default)
    {
        Guard.IsNotNull(client);
        Guard.IsNotNull(httpContext);

        _ = httpContext.Request.Form.TryGetValue(WidgetConstants.DefaultResponseFieldName, out var clientResponse);

        var ip = httpContext.Connection.RemoteIpAddress ?? IPAddress.Loopback;

        return client.ValidateAsync(clientResponse.ToString(), ip, cancellationToken);
    }
}
