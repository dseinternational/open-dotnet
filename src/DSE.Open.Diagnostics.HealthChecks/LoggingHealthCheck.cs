// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;

namespace DSE.Open.Diagnostics.HealthChecks;

public abstract partial class LoggingHealthCheck : IHealthCheck
{
    protected LoggingHealthCheck(ILogger logger)
    {
        Guard.IsNotNull(logger);
        Logger = logger;
    }

    protected ILogger Logger { get; }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        Guard.IsNotNull(context);
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
