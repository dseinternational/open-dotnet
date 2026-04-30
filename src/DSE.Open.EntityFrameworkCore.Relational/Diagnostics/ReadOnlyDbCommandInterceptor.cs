// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Data.Common;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace DSE.Open.EntityFrameworkCore.Diagnostics;

/// <summary>
/// A <see cref="DbCommandInterceptor"/> that prevents commands which would modify data
/// from executing, throwing <see cref="UpdateInReadOnlyContextException"/> for write
/// operations such as <c>SaveChanges</c>, <c>ExecuteUpdate</c>, or <c>ExecuteDelete</c>,
/// or for raw SQL containing <c>INSERT</c>, <c>UPDATE</c>, or <c>DELETE</c> statements.
/// </summary>
public partial class ReadOnlyDbCommandInterceptor : DbCommandInterceptor
{
    /// <inheritdoc/>
    public override DbCommand CommandInitialized(CommandEndEventData eventData, DbCommand result)
    {
        ArgumentNullException.ThrowIfNull(eventData);
        ArgumentNullException.ThrowIfNull(result);

        if (eventData.CommandSource is CommandSource.LinqQuery
            or CommandSource.FromSqlQuery
            or CommandSource.Unknown
            or CommandSource.Migrations
            or CommandSource.Scaffolding)
        {
            return base.CommandInitialized(eventData, result);
        }

        if (eventData.CommandSource is CommandSource.SaveChanges
            or CommandSource.ExecuteUpdate
            or CommandSource.ExecuteDelete)
        {
            UpdateInReadOnlyContextException.Throw();
        }

        // CommandSource.ExecuteSqlRaw, CommandSource.BulkUpdate, or
        // CommandSource.ValueGenerator

        if (ContainsWriteStatement(result.CommandText))
        {
            UpdateInReadOnlyContextException.Throw();
        }

        return base.CommandInitialized(eventData, result);
    }

    // internal for testing
    internal static bool ContainsWriteStatement(string commandText)
    {
        return UpdateSqlGeneratedRegex().IsMatch(commandText);
    }

    [GeneratedRegex(
        "INSERT[ \n]+(.*[ \n])*INTO|UPDATE[ \n]+(.*[ \n])*SET|DELETE[ \n]+(.*[ \n])*FROM",
        RegexOptions.IgnoreCase,
        "" // invariant culture
        )]
    private static partial Regex UpdateSqlGeneratedRegex();
}
