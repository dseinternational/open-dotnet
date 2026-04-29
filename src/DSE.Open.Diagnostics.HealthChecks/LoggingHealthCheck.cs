// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;

namespace DSE.Open.Diagnostics.HealthChecks;

/// <summary>
/// Base class for <see cref="IHealthCheck"/> implementations that log the outcome
/// of each health check execution and convert unhandled exceptions into the
/// registered failure status.
/// </summary>
public abstract partial class LoggingHealthCheck : IHealthCheck
{
    /// <summary>
    /// Initializes a new instance with the logger used to record health check outcomes.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="logger"/> is <see langword="null"/>.</exception>
    protected LoggingHealthCheck(ILogger logger)
    {
        ArgumentNullException.ThrowIfNull(logger);
        Logger = logger;
    }

    /// <summary>
    /// Gets the logger used to record health check outcomes and errors.
    /// </summary>
    protected ILogger Logger { get; }

    /// <summary>
    /// Runs <see cref="CheckHealthCoreAsync"/>, logs the resulting status, and
    /// converts any thrown exception into a result with the registration's failure status.
    /// </summary>
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(context);
#pragma warning disable CA1031 // Do not catch general exception types
        try
        {
            var result = await CheckHealthCoreAsync(context, cancellationToken).ConfigureAwait(false);

            switch (result.Status)
            {
                case HealthStatus.Unhealthy:
                    Log.Unhealthy(Logger, result.Description);
                    break;
                case HealthStatus.Degraded:
                    Log.Degraded(Logger, result.Description);
                    break;
                case HealthStatus.Healthy:
                    Log.Healthy(Logger, result.Description);
                    break;
                default:
                    ThrowHelper.ThrowInvalidOperationException($"Unsupported HealthStatus: {result.Status}");
                    break;
            }

            return result;
        }
        catch (Exception ex)
        {
            Log.Error(Logger, ex.Message, ex);
            return new(context.Registration.FailureStatus, description: $"Health Check Error: {ex.Message}", exception: ex);
        }
#pragma warning restore CA1031 // Do not catch general exception types
    }

    /// <summary>
    /// When implemented in a derived class, performs the health check logic and
    /// returns its result. Exceptions are caught and logged by the calling
    /// <see cref="CheckHealthAsync"/> method.
    /// </summary>
    protected abstract Task<HealthCheckResult> CheckHealthCoreAsync(HealthCheckContext context, CancellationToken cancellationToken);

    private static partial class Log
    {
        [LoggerMessage(EventId = 99099001, Level = LogLevel.Trace, Message = "Health Check Healthy: {description}")]
        public static partial void Healthy(ILogger logger, string? description);

        [LoggerMessage(EventId = 99099002, Level = LogLevel.Warning, Message = "Health Check Degraded: {description}")]
        public static partial void Degraded(ILogger logger, string? description);

        [LoggerMessage(EventId = 99099003, Level = LogLevel.Error, Message = "Health Check Unhealthy: {description}")]
        public static partial void Unhealthy(ILogger logger, string? description);

        [LoggerMessage(EventId = 99099004, Level = LogLevel.Error, Message = "Health Check Error: {message}")]
        public static partial void Error(ILogger logger, string message, Exception e);
    }
}
