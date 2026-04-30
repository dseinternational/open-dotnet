// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Data.SqlTypes;

namespace DSE.Open.Data.SqlClient;

/// <summary>
/// Provides helper methods for throwing SQL-related exceptions.
/// </summary>
public static class SqlExceptionHelper
{
    /// <summary>
    /// Throws a <see cref="SqlNullValueException"/> with the supplied message and inner exception.
    /// </summary>
    public static void ThrowSqlNullValueException(string? message = null, Exception? exception = null)
    {
        throw new SqlNullValueException(message, exception);
    }
}
