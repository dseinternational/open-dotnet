// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Data.Common;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace DSE.Open.EntityFrameworkCore.Diagnostics;

public partial class ReadOnlyDbCommandInterceptor : DbCommandInterceptor
{
    public override DbCommand CommandInitialized(CommandEndEventData eventData, DbCommand result)
    {
        Guard.IsNotNull(eventData);
        Guard.IsNotNull(result);

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
    internal static bool ContainsWriteStatement(string commandText) => UpdateSqlGeneratedRegex().IsMatch(commandText);

    [GeneratedRegex(
        "INSERT[ \n]+(.*[ \n])*INTO|UPDATE[ \n]+(.*[ \n])*SET|DELETE[ \n]+(.*[ \n])*FROM",
        RegexOptions.IgnoreCase,
        "" // invariant culture
        )]
    private static partial Regex UpdateSqlGeneratedRegex();
}
