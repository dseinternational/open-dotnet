// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Data.SqlTypes;

namespace DSE.Open.Data.SqlClient;

public static class SqlExceptionHelper
{
    public static void ThrowSqlNullValueException(string? message = null, Exception? exception = null)
        => throw new SqlNullValueException(message, exception);
}
