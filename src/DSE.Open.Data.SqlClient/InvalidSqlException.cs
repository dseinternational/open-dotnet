// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Data.SqlClient;

public class InvalidSqlException : Exception
{
    public InvalidSqlException() : this("Invalid SQL")
    {
    }

    public InvalidSqlException(string message) : this(message, null)
    {
    }

    public InvalidSqlException(string message, Exception? innerException)
        : this(message, null, innerException)
    {
    }

    public InvalidSqlException(string message, string? sql, Exception? innerException)
        : base(message, innerException)
    {
        Sql = sql ?? string.Empty;
    }

    public string? Sql { get; }
}
