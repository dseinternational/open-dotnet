// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using DSE.Open.Requests;
using DSE.Open.Results;

namespace DSE.Open.Sessions;

/// <summary>
/// Extension methods for accessing a <see cref="SessionContext"/> stored in
/// <see cref="RequestMetadata"/> or <see cref="ResultMetadata"/>.
/// </summary>
public static class RequestResultMetadataExtensions
{
    /// <summary>
    /// Gets the <see cref="SessionContext"/> stored in the specified
    /// <paramref name="requestMetadata"/>.
    /// </summary>
    public static SessionContext GetSessionContext(this RequestMetadata requestMetadata)
    {
        ArgumentNullException.ThrowIfNull(requestMetadata);
        return (SessionContext)requestMetadata.Properties[SessionContextMetadataKeys.SessionContext];
    }

    /// <summary>
    /// Gets the <see cref="SessionContext"/> stored in the specified
    /// <paramref name="resultMetadata"/>.
    /// </summary>
    public static SessionContext GetSessionContext(this ResultMetadata resultMetadata)
    {
        ArgumentNullException.ThrowIfNull(resultMetadata);
        return (SessionContext)resultMetadata.Properties[SessionContextMetadataKeys.SessionContext];
    }

    /// <summary>
    /// Attempts to get the <see cref="SessionContext"/> stored in the specified
    /// <paramref name="requestMetadata"/>.
    /// </summary>
    /// <returns><see langword="true"/> if a <see cref="SessionContext"/> was found;
    /// otherwise, <see langword="false"/>.</returns>
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

    /// <summary>
    /// Attempts to get the <see cref="SessionContext"/> stored in the specified
    /// <paramref name="resultMetadata"/>.
    /// </summary>
    /// <returns><see langword="true"/> if a <see cref="SessionContext"/> was found;
    /// otherwise, <see langword="false"/>.</returns>
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
