// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using DSE.Open.Requests;
using DSE.Open.Results;
using DSE.Open.Sessions;
using Microsoft.Extensions.Logging;

namespace DSE.Open.Api.Metadata;

/*
    This reader/writer handles reading/writing session context data from/to HTTP headers and HTTP cookies.

    In client web applications, session context data is read from cookies when a user requests a page. The
    session context data is then passed via HTTP request headers to backend APIs. Session context returned
    from backend APIs is then read from HTTP response headers, and subsequently written to cookies in the
    response to the user.

    When a request is read, the session context object is added to both request and result metadata. This
    ensures that any services writing to the result metadata write to the same session.

    When result data is read, the session context object is simply added to result metadata. At this point,
    the session in result metadata will no longer be the same instance. 

*/

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
        MetadataStorageContext context,
        CancellationToken cancellationToken = default)
    {
        Guard.IsNotNull(context);
        Guard.IsNotNull(request);
        Guard.IsNotNull(result);

        SessionContext? sessionContext = null;

        if (context.Data.TryGetValue(SessionContextMetadataKeys.SessionContext, out var sessionContextValue))
        {
            if (SessionContextSerializer.TryDeserializeFromBase64Utf8Json(sessionContextValue, out sessionContext))
            {
                Log.SessionContextReadFromRequestMetadata(_logger, sessionContext);
            }
            else
            {
                _logger.LogWarning(
                    "Failed to deserialize session context from request " +
                    "metadata: {SessionContextValue}", sessionContextValue);
            }
        }

        // not read - create new session

        sessionContext ??= new SessionContext();

        Log.NoSessionContextInRequestMetadata(_logger, sessionContext);

        sessionContext = (SessionContext)request.Properties.AddOrUpdate(
            SessionContextMetadataKeys.SessionContext,
            sessionContext,
            (k, v) => sessionContext);

        _ = (SessionContext)result.Properties.AddOrUpdate(
            SessionContextMetadataKeys.SessionContext,
            sessionContext,
            (k, v) => sessionContext);

        return ValueTask.CompletedTask;
    }

    public ValueTask ReadResultMetadataAsync(
        RequestMetadata request,
        ResultMetadata result,
        MetadataStorageContext context,
        CancellationToken cancellationToken = default)
    {
        Guard.IsNotNull(context);
        Guard.IsNotNull(request);
        Guard.IsNotNull(result);

        SessionContext? sessionContext = null;

        if (context.Data.TryGetValue(SessionContextMetadataKeys.SessionContext, out var sessionContextValue))
        {
            if (SessionContextSerializer.TryDeserializeFromBase64Utf8Json(sessionContextValue, out sessionContext))
            {
                Log.SessionContextReadFromResultMetadata(_logger, sessionContext);

                sessionContext = (SessionContext)result.Properties.AddOrUpdate(
                    SessionContextMetadataKeys.SessionContext,
                    sessionContext,
                    (k, v) => sessionContext);
            }
            else
            {
                _logger.LogWarning(
                    "Failed to deserialize session context from result " +
                    "metadata: {SessionContextValue}", sessionContextValue);
            }
        }

        return ValueTask.CompletedTask;
    }

    public ValueTask WriteRequestMetadataAsync(
        RequestMetadata request,
        ResultMetadata result,
        MetadataStorageContext context,
        CancellationToken cancellationToken = default)
    {
        Guard.IsNotNull(request);
        Guard.IsNotNull(result);
        Guard.IsNotNull(context);

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
        Guard.IsNotNull(request);
        Guard.IsNotNull(result);
        Guard.IsNotNull(context);

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
            Message = "Session context read from result metadata: {SessionContext}")]
        public static partial void SessionContextReadFromResultMetadata(
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