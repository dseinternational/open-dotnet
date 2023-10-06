// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using DSE.Open.Requests;
using DSE.Open.Results;
using DSE.Open.Sessions;
using Microsoft.Extensions.Logging;

namespace DSE.Open.Api.Metadata;

[RequiresDynamicCode(WarningMessages.RequiresDynamicCode)]
[RequiresUnreferencedCode(WarningMessages.RequiresUnreferencedCode)]
public sealed partial class SessionContextMetadataReaderWriter : IMetadataReader, IMetadataWriter
{
    private readonly ILogger<SessionContextMetadataReaderWriter> _logger;

    public SessionContextMetadataReaderWriter(ILogger<SessionContextMetadataReaderWriter> logger)
    {
        Guard.IsNotNull(logger);
        _logger = logger;
    }

    public ValueTask ReadRequestMetadataAsync(
        RequestMetadata request,
        ResultMetadata result,
        ConcurrentDictionary<string, string> source,
        CancellationToken cancellationToken = default)
    {
        Guard.IsNotNull(source);
        Guard.IsNotNull(request);
        Guard.IsNotNull(result);

        if (!source.TryGetValue(SessionContextMetadataKeys.SessionContext, out var sessionContextValue))
        {
            return ValueTask.CompletedTask;
        }

        if (!SessionContextSerializer.TryDeserializeFromBase64Utf8Json(sessionContextValue, out var sessionContext))
        {
            _logger.LogWarning("Failed to deserialize session context from request metadata: {SessionContextValue}", sessionContextValue);
        }

        if (sessionContext is not null)
        {
            Log.SessionContextReadFromRequestMetadata(_logger, sessionContext);
        }
        else
        {
            sessionContext = new SessionContext();
            Log.NoSessionContextInRequestMetadata(_logger, sessionContext);
        }

        sessionContext = (SessionContext)request.Properties.AddOrUpdate(SessionContextMetadataKeys.SessionContext, sessionContext, (k, v) => sessionContext);

        // add to result now to ensure it is returned to the client
        _ = result.Properties.AddOrUpdate(SessionContextMetadataKeys.SessionContext, sessionContext, (k, v) => sessionContext);

        return ValueTask.CompletedTask;
    }

    public ValueTask ReadResultMetadataAsync(
        RequestMetadata request,
        ResultMetadata result,
        ConcurrentDictionary<string, string> source,
        CancellationToken cancellationToken = default)
    {
        Guard.IsNotNull(source);
        Guard.IsNotNull(request);
        Guard.IsNotNull(result);

        if (!source.TryGetValue(SessionContextMetadataKeys.SessionContext, out var sessionContextValue))
        {
            return ValueTask.CompletedTask;
        }

        var sessionContext = SessionContextSerializer.DeserializeFromBase64Utf8Json(sessionContextValue);

        if (sessionContext is not null)
        {
            Log.SessionContextReadFromResultMetadata(_logger, sessionContext);
        }
        else
        {
            sessionContext = new SessionContext();
            Log.NoSessionContextInRequestResult(_logger, sessionContext);
        }

        _ = result.Properties.AddOrUpdate(SessionContextMetadataKeys.SessionContext, sessionContext, (k, v) => sessionContext);

        return ValueTask.CompletedTask;
    }

    public ValueTask WriteRequestMetadataAsync(
        RequestMetadata request,
        ResultMetadata result,
        ConcurrentDictionary<string, string> target,
        CancellationToken cancellationToken = default)
    {
        Guard.IsNotNull(request);
        Guard.IsNotNull(result);
        Guard.IsNotNull(target);

        if (!request.Properties.TryGetValue(SessionContextMetadataKeys.SessionContext, out var sessionContextObj)
            || sessionContextObj is not SessionContext sessionContext)
        {
            return ValueTask.CompletedTask;
        }

        var sessionContextValue = SessionContextSerializer.SerializeToBase64Utf8Json(sessionContext);

        target[SessionContextMetadataKeys.SessionContext] = sessionContextValue;

        Log.SessionContextWrittenToRequestMetadata(_logger, sessionContext);

        return ValueTask.CompletedTask;
    }

    public ValueTask WriteResultMetadataAsync(
        RequestMetadata request,
        ResultMetadata result,
        ConcurrentDictionary<string, string> target,
        CancellationToken cancellationToken = default)
    {
        Guard.IsNotNull(request);
        Guard.IsNotNull(result);
        Guard.IsNotNull(target);

        if (!result.Properties.TryGetValue(SessionContextMetadataKeys.SessionContext, out var sessionContextObj)
            || sessionContextObj is not SessionContext sessionContext)
        {
            return ValueTask.CompletedTask;
        }

        var sessionContextValue = SessionContextSerializer.SerializeToBase64Utf8Json(sessionContext);

        target[SessionContextMetadataKeys.SessionContext] = sessionContextValue;

        Log.SessionContextWrittenToResponseMetadata(_logger, sessionContext);

        return ValueTask.CompletedTask;
    }

    private static partial class Log
    {
        [LoggerMessage(
            EventId = 1,
            Level = LogLevel.Debug,
            Message = "Session context read from request metadata: {SessionContext}")]
        public static partial void SessionContextReadFromRequestMetadata(
            ILogger logger,
            SessionContext sessionContext);

        [LoggerMessage(
            EventId = 2,
            Level = LogLevel.Debug,
            Message = "No session context in request metadata - created new context: {SessionContext}")]
        public static partial void NoSessionContextInRequestMetadata(
            ILogger logger,
            SessionContext sessionContext);

        [LoggerMessage(
            EventId = 3,
            Level = LogLevel.Debug,
            Message = "Session context read from request result: {SessionContext}")]
        public static partial void SessionContextReadFromResultMetadata(
            ILogger logger,
            SessionContext sessionContext);

        [LoggerMessage(
            EventId = 4,
            Level = LogLevel.Debug,
            Message = "No session context in request result - created new context: {SessionContext}")]
        public static partial void NoSessionContextInRequestResult(
            ILogger logger,
            SessionContext sessionContext);

        [LoggerMessage(
            EventId = 5,
            Level = LogLevel.Debug,
            Message = "Session context written to request metadata: {SessionContext}")]
        public static partial void SessionContextWrittenToRequestMetadata(
            ILogger logger,
            SessionContext sessionContext);

        [LoggerMessage(
            EventId = 6,
            Level = LogLevel.Debug,
            Message = "Session context written to response metadata: {SessionContext}")]
        public static partial void SessionContextWrittenToResponseMetadata(
            ILogger logger,
            SessionContext sessionContext);
    }
}

internal static class WarningMessages
{
    public const string RequiresDynamicCode =
        "JSON serialization and deserialization might require types that cannot be statically " +
        "analyzed and might need runtime code generation. Use System.Text.Json source generation for " +
        "native AOT applications.";

    public const string RequiresUnreferencedCode =
        "JSON serialization and deserialization might require types that cannot be " +
        "statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make " +
        "sure all of the required types are preserved.";
}
