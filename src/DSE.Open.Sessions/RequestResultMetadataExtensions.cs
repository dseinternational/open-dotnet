// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using DSE.Open.Requests;
using DSE.Open.Results;

namespace DSE.Open.Sessions;

public static class RequestResultMetadataExtensions
{
    public static SessionContext GetSessionContext(this RequestMetadata requestMetadata)
    {
        ArgumentNullException.ThrowIfNull(requestMetadata);
        return (SessionContext)requestMetadata.Properties[SessionContextMetadataKeys.SessionContext];
    }

    public static SessionContext GetSessionContext(this ResultMetadata resultMetadata)
    {
        ArgumentNullException.ThrowIfNull(resultMetadata);
        return (SessionContext)resultMetadata.Properties[SessionContextMetadataKeys.SessionContext];
    }

    public static bool TryGetSessionContext(
        this RequestMetadata requestMetadata,
        [NotNullWhen(true)] out SessionContext? sessionContext)
    {
        ArgumentNullException.ThrowIfNull(requestMetadata);

        if (requestMetadata.Properties.TryGetValue(SessionContextMetadataKeys.SessionContext, out var sessionObj)
            && sessionObj is SessionContext session)
        {
            sessionContext = session;
            return true;
        }

        sessionContext = null;
        return false;
    }

    public static bool TryGetSessionContext(
        this ResultMetadata resultMetadata,
        [NotNullWhen(true)] out SessionContext? sessionContext)
    {
        ArgumentNullException.ThrowIfNull(resultMetadata);

        if (resultMetadata.Properties.TryGetValue(SessionContextMetadataKeys.SessionContext, out var sessionObj)
            && sessionObj is SessionContext session)
        {
            sessionContext = session;
            return true;
        }

        sessionContext = null;
        return false;
    }
}
