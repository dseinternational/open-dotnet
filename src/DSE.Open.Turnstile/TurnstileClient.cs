// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Http.Json;
using DSE.Open.Diagnostics;
using DSE.Open.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;

namespace DSE.Open.Turnstile;

public sealed partial class TurnstileClient : ITurnstileClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<TurnstileClient> _logger;
    private readonly TurnstileClientOptions _options;

    public TurnstileClient(
        HttpClient httpClient,
        IOptions<TurnstileClientOptions> options,
        ILogger<TurnstileClient> logger)
    {
        Guard.IsNotNull(httpClient);
        Guard.IsNotNull(options);
        Guard.IsNotNull(options.Value);
        Guard.IsNotNull(logger);

        _options = options.Value;
        _httpClient = httpClient;
        _logger = logger;
    }

    public TurnstileClient(
        HttpClient httpClient,
        TurnstileClientOptions options,
        ILogger<TurnstileClient> logger)
    {
        Guard.IsNotNull(httpClient);
        Guard.IsNotNull(options);
        Guard.IsNotNull(logger);

        _options = options;
        _httpClient = httpClient;
        _logger = logger;
    }

    public TurnstileClient(
        HttpClient httpClient,
        TurnstileClientOptions options)
    {
        Guard.IsNotNull(httpClient);
        Guard.IsNotNull(options);

        _options = options;
        _httpClient = httpClient;
        _logger = NullLogger<TurnstileClient>.Instance;
    }

    [RequiresDynamicCode(WarningMessages.RequiresDynamicCode)]
    [RequiresUnreferencedCode(WarningMessages.RequiresUnreferencedCode)]
    public async Task<ValidationResponse> ValidateAsync(
        string clientResponse,
        IPAddress? clientIpAddress = null,
        CancellationToken cancellationToken = default)
    {
        Guard.IsNotNull(clientResponse);

        var idempotencyKey = Guid.NewGuid().ToString();

        var request = new ValidationRequest
        {
            SecretKey = _options.SecretKey,
            Response = clientResponse,
            ClientIpAddress = clientIpAddress?.ToString(),
            IdempotencyKey = idempotencyKey
        };

        ValidationResponse? validation = null;

        for (var attempts = 0; attempts < 5; attempts++)
        {
            if (attempts > 0)
            {
                await Task.Delay(attempts * 1000, cancellationToken).ConfigureAwait(false);
            }

            Log.RequestingValidation(_logger, idempotencyKey, clientIpAddress);

            var response = await _httpClient
                .PostAsJsonAsync(_options.Endpoint, request, JsonSharedOptions.RelaxedJsonEscaping, cancellationToken)
                .ConfigureAwait(false);

            Expect.NotNull(response);

            validation = await response.Content
                .ReadFromJsonAsync<ValidationResponse>(JsonSharedOptions.RelaxedJsonEscaping, cancellationToken)
                .ConfigureAwait(false);

            Expect.NotNull(validation);

            if (!validation.Success)
            {
                if (validation.ErrorCodes.Contains(TurnstileErrorCodes.InternalError))
                {
                    // TODO: log
                    continue;
                }

                // TODO: log
            }

            break;
        }

        if (validation is null)
        {
            throw new ValidationException("Failed to obtain validation, despite retries.");
        }

        return validation;
    }

    private static partial class Log
    {
        [LoggerMessage(
            EventId = 5050103,
            Level = LogLevel.Debug,
            Message = "Requesting validation {idempotencyKey} for client response from {clientIpAddress}")]
        public static partial void RequestingValidation(ILogger logger, string idempotencyKey, IPAddress? clientIpAddress);
    }
}
