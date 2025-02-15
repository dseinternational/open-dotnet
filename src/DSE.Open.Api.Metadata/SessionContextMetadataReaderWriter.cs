// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using DSE.Open.Requests;
using DSE.Open.Results;
using DSE.Open.Sessions;
using Microsoft.Extensions.Logging;

namespace DSE.Open.Api.Metadata;

/// <summary>
/// <para>
/// This reader/writer handles reading/writing session context data from/to HTTP headers and HTTP cookies.
/// </para>
/// <para>
/// In client web applications, session context data is read from cookies when a user requests a page. The
/// session context data is then passed via HTTP request headers to backend APIs. Session context returned
/// from backend APIs is then read from HTTP response headers, and subsequently written to cookies in the
/// response to the user.
/// </para>
/// <para>
/// When a request is read, the session context object is added to both request and result metadata. This
/// ensures that any services writing to the result metadata write to the same session.
/// </para>
/// <para>
/// When result data is read, the session context object is simply added to result metadata. At this point,
/// the session in result metadata will no longer be the same instance.
/// </para>
/// </summary>
[RequiresDynamicCode(WarningMessages.RequiresDynamicCode)]
[RequiresUnreferencedCode(WarningMessages.RequiresUnreferencedCode)]
public sealed partial class SessionContextMetadataReaderWriter : IMetadataReader, IMetadataWriter
{
    private readonly ILogger<SessionContextMetadataReaderWriter> _logger;

    public SessionContextMetadataReaderWriter(ILogger<SessionContextMetadataReaderWriter> logger)
    {
        ArgumentNullException.ThrowIfNull(logger);
        _logger = logger;
    }

    public ValueTask ReadRequestMetadataAsync(
        RequestMetadata request,
        ResultMetadata result,
        MetadataStorageContext context,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(result);

        SessionContext? sessionContext;

        if (context.Data.TryGetValue(SessionContextMetadataKeys.SessionContext, out var sessionContextValue))
        {
            if (SessionContextSerializer.TryDeserializeFromBase64Utf8Json(sessionContextValue, out sessionContext))
            {
                Log.SessionContextReadFromRequestMetadata(_logger, sessionContext);
            }
            else
            {
                Log.FailedToDeserializeSessionContextFromRequestMetadata(_logger, sessionContextValue);
                ThrowHelper.ThrowInvalidOperationException($"Failed to deserialize {nameof(SessionContext)} from {nameof(RequestMetadata)}.");
                return ValueTask.CompletedTask; // unreachable
            }
        }
        else
        {
            sessionContext = new();
            Log.NoSessionContextInRequestMetadata(_logger, sessionContext);
        }

        _ = request.Properties.AddOrUpdate(
            SessionContextMetadataKeys.SessionContext,
            sessionContext,
            (_, _) => sessionContext);

        _ = result.Properties.AddOrUpdate(
            SessionContextMetadataKeys.SessionContext,
            sessionContext,
            (_, _) => sessionContext);

        return ValueTask.CompletedTask;
    }

    public ValueTask ReadResultMetadataAsync(
        RequestMetadata request,
        ResultMetadata result,
        MetadataStorageContext context,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(result);

        if (!context.Data.TryGetValue(SessionContextMetadataKeys.SessionContext, out var sessionContextValue))
        {
            // Already added, and we don't have anything new so leave the existing value.
            return ValueTask.CompletedTask;
        }

        if (SessionContextSerializer.TryDeserializeFromBase64Utf8Json(sessionContextValue, out var sessionContext))
        {
            Log.SessionContextReadFromResultMetadata(_logger, sessionContext);

            _ = result.Properties.AddOrUpdate(
                SessionContextMetadataKeys.SessionContext,
                sessionContext,
                (_, _) => sessionContext);
        }
        else
        {
            // We're reading from result, but the base64 value is invalid. This
            // is a bug in some using code.
            Log.FailedToDeserializeSessionContextFromResultMetadata(_logger, sessionContextValue);
            ThrowHelper.ThrowInvalidOperationException($"Failed to deserialize {nameof(SessionContext)} from {nameof(ResultMetadata)}.");
            return ValueTask.CompletedTask; // unreachable
        }

        return ValueTask.CompletedTask;
    }

    public ValueTask WriteRequestMetadataAsync(
        RequestMetadata request,
        ResultMetadata result,
        MetadataStorageContext context,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(context);

        if (!request.Properties.TryGetValue(SessionContextMetadataKeys.SessionContext, out var sessionContextObj)
            || sessionContextObj is not SessionContext sessionContext)
        {
            return ValueTask.CompletedTask;
        }

        var sessionContextValue = SessionContextSerializer.SerializeToBase64Utf8Json(sessionContext);

        context.Data[SessionContextMetadataKeys.SessionContext] = sessionContextValue;

        Log.SessionContextWrittenToRequestMetadata(_logger, sessionContext);

        return ValueTask.CompletedTask;
    }

    public ValueTask WriteResultMetadataAsync(
        RequestMetadata request,
        ResultMetadata result,
        MetadataStorageContext context,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(context);

        if (!result.Properties.TryGetValue(SessionContextMetadataKeys.SessionContext, out var sessionContextObj)
            || sessionContextObj is not SessionContext sessionContext)
        {
            return ValueTask.CompletedTask;
        }

        var sessionContextValue = SessionContextSerializer.SerializeToBase64Utf8Json(sessionContext);

        context.Data[SessionContextMetadataKeys.SessionContext] = sessionContextValue;

        Log.SessionContextWrittenToResponseMetadata(_logger, sessionContext);

        return ValueTask.CompletedTask;
    }

    private static partial class Log
    {
        [LoggerMessage(
            EventId = 1,
            Level = LogLevel.Trace,
            Message = "Session context read from request metadata: {SessionContext}")]
        public static partial void SessionContextReadFromRequestMetadata(
            ILogger logger,
            SessionContext sessionContext);

        [LoggerMessage(
            EventId = 2,
            Level = LogLevel.Trace,
            Message = "No session context in request metadata - created new context: {SessionContext}")]
        public static partial void NoSessionContextInRequestMetadata(
            ILogger logger,
            SessionContext sessionContext);

        [LoggerMessage(
            EventId = 3,
            Level = LogLevel.Trace,
            Message = "Session context read from result metadata: {SessionContext}")]
        public static partial void SessionContextReadFromResultMetadata(
            ILogger logger,
            SessionContext sessionContext);

        [LoggerMessage(
            EventId = 5,
            Level = LogLevel.Trace,
            Message = "Session context written to request metadata: {SessionContext}")]
        public static partial void SessionContextWrittenToRequestMetadata(
            ILogger logger,
            SessionContext sessionContext);

        [LoggerMessage(
            EventId = 6,
            Level = LogLevel.Trace,
            Message = "Session context written to response metadata: {SessionContext}")]
        public static partial void SessionContextWrittenToResponseMetadata(
            ILogger logger,
            SessionContext sessionContext);

        [LoggerMessage(
            EventId = 7,
            Level = LogLevel.Warning,
            Message = "Failed to deserialize session context from request metadata: {SessionContextValue}")]
        public static partial void FailedToDeserializeSessionContextFromRequestMetadata(
            ILogger logger,
            string sessionContextValue);

        [LoggerMessage(
            EventId = 8,
            Level = LogLevel.Warning,
            Message = "Failed to deserialize session context from result metadata: {SessionContextValue}")]
        public static partial void FailedToDeserializeSessionContextFromResultMetadata(
            ILogger logger,
            string sessionContextValue);
    }
}
