// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace DSE.Open.Turnstile;

public interface ITurnstileClient
{
    [RequiresDynamicCode(WarningMessages.RequiresDynamicCode)]
    [RequiresUnreferencedCode(WarningMessages.RequiresUnreferencedCode)]
    Task<ValidationResponse> ValidateAsync(
        string clientResponse,
        IPAddress? clientIpAddress = null,
        CancellationToken cancellationToken = default);
}
