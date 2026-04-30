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

/// <summary>
/// An <see cref="ITurnstileClient"/> implementation that calls the Cloudflare Turnstile siteverify endpoint via <see cref="HttpClient"/>.
/// </summary>
public sealed partial class TurnstileClient : ITurnstileClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<TurnstileClient> _logger;
    private readonly TurnstileClientOptions _options;

    /// <summary>
    /// Initializes a new instance of the <see cref="TurnstileClient"/> class using options resolved
    /// from <see cref="IOptions{TOptions}"/>.
    /// </summary>
    public TurnstileClient(
        HttpClient httpClient,
        IOptions<TurnstileClientOptions> options,
        ILogger<TurnstileClient> logger)
    {
        ArgumentNullException.ThrowIfNull(httpClient);
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(options.Value);
        ArgumentNullException.ThrowIfNull(logger);

        _options = options.Value;
        _httpClient = httpClient;
        _logger = logger;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TurnstileClient"/> class with the supplied options and logger.
    /// </summary>
    public TurnstileClient(
        HttpClient httpClient,
        TurnstileClientOptions options,
        ILogger<TurnstileClient> logger)
    {
        ArgumentNullException.ThrowIfNull(httpClient);
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(logger);

        _options = options;
        _httpClient = httpClient;
        _logger = logger;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TurnstileClient"/> class with the supplied options
    /// and a <see cref="NullLogger{T}"/>.
    /// </summary>
    public TurnstileClient(
        HttpClient httpClient,
        TurnstileClientOptions options)
    {
        ArgumentNullException.ThrowIfNull(httpClient);
        ArgumentNullException.ThrowIfNull(options);

        _options = options;
        _httpClient = httpClient;
        _logger = NullLogger<TurnstileClient>.Instance;
    }

    /// <summary>
    /// Posts a validation request to the configured Turnstile endpoint, retrying up to five times when the
    /// service reports an internal error.
    /// </summary>
    /// <param name="clientResponse">The Turnstile response token produced by the client-side widget.</param>
    /// <param name="clientIpAddress">The optional IP address of the visitor that produced the response.</param>
    /// <param name="cancellationToken">A token that can be used to cancel the asynchronous operation.</param>
    /// <returns>The <see cref="ValidationResponse"/> returned by the Turnstile service.</returns>
    /// <exception cref="ValidationException">Thrown when no validation response could be obtained after retries.</exception>
    [RequiresDynamicCode(WarningMessages.RequiresDynamicCode)]
    [RequiresUnreferencedCode(WarningMessages.RequiresUnreferencedCode)]
    public async Task<ValidationResponse> ValidateAsync(
        string clientResponse,
        IPAddress? clientIpAddress = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(clientResponse);

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
