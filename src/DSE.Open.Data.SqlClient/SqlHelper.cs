// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Data.SqlClient;

public static class SqlHelper
{
    /// <summary>
    /// Determines if the supplied string is a valid Transact-SQL regular identifier.
    /// </summary>
    /// <param name="identifier"></param>
    /// <returns><see langword="true" /> if <paramref name="identifier"/> is a valid Transact-SQL regular
    /// identifier, otherwise <see langword="false"/>.</returns>
    /// <remarks>See: <see href="https://docs.microsoft.com/en-us/sql/relational-databases/databases/database-identifiers"/></remarks>
    public static bool IsValidRegularIdentifier(string? identifier)
    {
        if (string.IsNullOrEmpty(identifier))
        {
            return false;
        }

        if (!(char.IsLetter(identifier[0]) || identifier[0] == '@' || identifier[0] == '_' || identifier[0] == '#'))
        {
            return false;
        }

        if (identifier.Length == 1)
        {
            return true;
        }

        for (var i = 1; i < identifier.Length; i++)
        {
            if (!(char.IsLetterOrDigit(identifier[i]) || identifier[i] == '_' || identifier[i] == '@' || identifier[i] == '#' || identifier[i] == '$'))
            {
                return false;
            }
        }

        return Array.BinarySearch(s_sqlServerReservedKeywords, identifier, StringComparer.OrdinalIgnoreCase) <= -1;
    }

    /// <summary>
    /// Throws an <see cref="InvalidSqlException"/> if the supplied string is not a
    /// valid Transact-SQL regular identifier.
    /// </summary>
    /// <param name="identifier"></param>
    /// <exception cref="ArgumentNullException"><paramref name="identifier"/> is null.</exception>
    /// <exception cref="InvalidSqlException"><paramref name="identifier"/> is not a valid Transact-SQL regular identifier.</exception>
    public static void EnsureValidRegularIdentifier(string identifier)
    {
        Guard.IsNotNullOrEmpty(identifier);

        if (!IsValidRegularIdentifier(identifier))
        {
            throw new InvalidSqlException("'" + identifier + "' is not a valid Transact-SQL regular identifier.");
        }
    }

    // https://learn.microsoft.com/en-us/sql/t-sql/language-elements/reserved-keywords-transact-sql?view=sql-server-ver16

    private static readonly string[] s_sqlServerReservedKeywords = new[]
    {
        "ADD",
        "ALL",
        "ALTER",
        "AND",
        "ANY",
        "AS",
        "ASC",
        "AUTHORIZATION",
        "BACKUP",
        "BEGIN",
        "BETWEEN",
        "BREAK",
        "BROWSE",
        "BULK",
        "BY",
        "CASCADE",
        "CASE",
        "CHECK",
        "CHECKPOINT",
        "CLOSE",
        "CLUSTERED",
        "COALESCE",
        "COLLATE",
        "COLUMN",
        "COMMIT",
        "COMPUTE",
        "CONSTRAINT",
        "CONTAINS",
        "CONTAINSTABLE",
        "CONTINUE",
        "CONVERT",
        "CREATE",
        "CROSS",
        "CURRENT",
        "CURRENT_DATE",
        "CURRENT_TIME",
        "CURRENT_TIMESTAMP",
        "CURRENT_USER",
        "CURSOR",
        "DATABASE",
        "DBCC",
        "DEALLOCATE",
        "DECLARE",
        "DEFAULT",
        "DELETE",
        "DENY",
        "DESC",
        "DISK",
        "DISTINCT",
        "DISTRIBUTED",
        "DOUBLE",
        "DROP",
        "DUMP",
        "ELSE",
        "END",
        "ERRLVL",
        "ESCAPE",
        "EXCEPT",
        "EXEC",
        "EXECUTE",
        "EXISTS",
        "EXIT",
        "EXTERNAL",
        "FETCH",
        "FILE",
        "FILLFACTOR",
        "FOR",
        "FOREIGN",
        "FREETEXT",
        "FREETEXTTABLE",
        "FROM",
        "FULL",
        "FUNCTION",
        "GOTO",
        "GRANT",
        "GROUP",
        "HAVING",
        "HOLDLOCK",
        "IDENTITY",
        "IDENTITYCOL",
        "IDENTITY_INSERT",
        "IF",
        "IN",
        "INDEX",
        "INNER",
        "INSERT",
        "INTERSECT",
        "INTO",
        "IS",
        "JOIN",
        "KEY",
        "KILL",
        "LEFT",
        "LIKE",
        "LINENO",
        "LOAD",
        "MERGE",
        "NATIONAL",
        "NOCHECK",
        "NONCLUSTERED",
        "NOT",
        "NULL",
        "NULLIF",
        "OF",
        "OFF",
        "OFFSETS",
        "ON",
        "OPEN",
        "OPENDATASOURCE",
        "OPENQUERY",
        "OPENROWSET",
        "OPENXML",
        "OPTION",
        "OR",
        "ORDER",
        "OUTER",
        "OVER",
        "PERCENT",
        "PIVOT",
        "PLAN",
        "PRECISION",
        "PRIMARY",
        "PRINT",
        "PROC",
        "PROCEDURE",
        "PUBLIC",
        "RAISERROR",
        "READ",
        "READTEXT",
        "RECONFIGURE",
        "REFERENCES",
        "REPLICATION",
        "RESTORE",
        "RESTRICT",
        "RETURN",
        "REVERT",
        "REVOKE",
        "RIGHT",
        "ROLLBACK",
        "ROWCOUNT",
        "ROWGUIDCOL",
        "RULE",
        "SAVE",
        "SCHEMA",
        "SECURITYAUDIT",
        "SELECT",
        "SEMANTICKEYPHRASETABLE",
        "SEMANTICSIMILARITYDETAILSTABLE",
        "SEMANTICSIMILARITYTABLE",
        "SESSION_USER",
        "SET",
        "SETUSER",
        "SHUTDOWN",
        "SOME",
        "STATISTICS",
        "SYSTEM_USER",
        "TABLE",
        "TABLESAMPLE",
        "TEXTSIZE",
        "THEN",
        "TO",
        "TOP",
        "TRAN",
        "TRANSACTION",
        "TRIGGER",
        "TRUNCATE",
        "TRY_CONVERT",
        "TSEQUAL",
        "UNION",
        "UNIQUE",
        "UNPIVOT",
        "UPDATE",
        "UPDATETEXT",
        "USE",
        "USER",
        "VALUES",
        "VARYING",
        "VIEW",
        "WAITFOR",
        "WHEN",
        "WHERE",
        "WHILE",
        "WITH",
        "WITHIN GROUP",
        "WRITETEXT",
    };
}
