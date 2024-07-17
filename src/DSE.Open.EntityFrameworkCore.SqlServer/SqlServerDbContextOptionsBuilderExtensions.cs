// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace DSE.Open.EntityFrameworkCore.SqlServer;

public static class SqlServerDbContextOptionsBuilderExtensions
{
    /// <summary>
    ///     Applies our default SQL Server options.
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="commandTimeout"></param>
    /// <param name="querySplittingBehavior"></param>
    /// <param name="useHierarchyId"></param>
    /// <returns></returns>
    /// <remarks>
    ///     <para>
    ///         This applies the following defaults:
    ///     </para>
    ///     <list type="bullet">
    ///         <item>Default command timeout of 60 seconds</item>
    ///         <item>Default execution strategy is <see cref="SqlServerNonBufferingRetryingExecutionStrategy"/></item>
    ///         <item>Query splitting behaviour is <see cref="QuerySplittingBehavior.SingleQuery"/></item>
    ///     </list>
    /// </remarks>
    public static SqlServerDbContextOptionsBuilder UseDefaultSqlServerOptions(
        this SqlServerDbContextOptionsBuilder builder,
        int commandTimeout = 60,
        QuerySplittingBehavior querySplittingBehavior = QuerySplittingBehavior.SingleQuery,
        bool useHierarchyId = true)
    {
        ArgumentNullException.ThrowIfNull(builder);

        _ = builder
            .CommandTimeout(commandTimeout)
            .ExecutionStrategy(deps => new SqlServerNonBufferingRetryingExecutionStrategy(deps))
            .UseQuerySplittingBehavior(querySplittingBehavior);

        if (useHierarchyId)
        {
            _ = builder.UseHierarchyId();
        }

        return builder;
    }
}
