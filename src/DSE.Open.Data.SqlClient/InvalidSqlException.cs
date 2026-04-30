// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Data.SqlClient;

/// <summary>
/// The exception that is thrown when a Transact-SQL statement or fragment is invalid.
/// </summary>
public class InvalidSqlException : Exception
{
    /// <summary>
    /// Initializes a new instance with a default message.
    /// </summary>
    public InvalidSqlException() : this("Invalid SQL")
    {
    }

    /// <summary>
    /// Initializes a new instance with the specified message.
    /// </summary>
    public InvalidSqlException(string message) : this(message, null)
    {
    }

    /// <summary>
    /// Initializes a new instance with the specified message and inner exception.
    /// </summary>
    public InvalidSqlException(string message, Exception? innerException)
        : this(message, null, innerException)
    {
    }

    /// <summary>
    /// Initializes a new instance with the specified message, offending SQL, and inner exception.
    /// </summary>
    public InvalidSqlException(string message, string? sql, Exception? innerException)
        : base(message, innerException)
    {
        Sql = sql ?? string.Empty;
    }

    /// <summary>
    /// Gets the SQL text associated with the exception, or an empty string if none was provided.
    /// </summary>
    public string Sql { get; }
}
