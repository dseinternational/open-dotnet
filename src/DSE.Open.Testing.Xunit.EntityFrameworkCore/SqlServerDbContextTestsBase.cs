// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using DSE.Open.EntityFrameworkCore;
using DSE.Open.EntityFrameworkCore.SqlServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Xunit;

namespace DSE.Open.Testing.Xunit.EntityFrameworkCore;

/// <summary>
/// A <see cref="DbContextTestsBase{TContext}"/> that configures <typeparamref name="TContext"/>
/// to use SQL Server.
/// </summary>
/// <typeparam name="TContext">The type of the <see cref="DbContext"/>.</typeparam>
public abstract class SqlServerDbContextTestsBase<[DynamicallyAccessedMembers(TrimmingHelper.DbContextDynamicallyAccessedMemberTypes)] TContext>
    : DbContextTestsBase<TContext>
    where TContext : DbContext
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SqlServerDbContextTestsBase{TContext}"/> class.
    /// </summary>
    /// <param name="output">The xUnit test output helper.</param>
    protected SqlServerDbContextTestsBase(ITestOutputHelper output) : this(null, output)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SqlServerDbContextTestsBase{TContext}"/> class.
    /// </summary>
    /// <param name="connection">An optional connection string.</param>
    /// <param name="output">The xUnit test output helper.</param>
    protected SqlServerDbContextTestsBase(string? connection, ITestOutputHelper output) : base(output)
    {
    }

    /// <summary>
    /// Gets the SQL Server connection string used to configure the context.
    /// </summary>
    public abstract string ConnectionString { get; }


    /// <inheritdoc/>
    protected override void ConfigureDbContext(IServiceProvider services, DbContextOptionsBuilder options)
    {
        _ = options.UseSqlServer(ConnectionString, ConfigureSqlOptions);
    }

    /// <summary>
    /// Configures SQL Server-specific options on the supplied
    /// <see cref="SqlServerDbContextOptionsBuilder"/>.
    /// </summary>
    /// <param name="options">The SQL Server options builder.</param>
    protected virtual void ConfigureSqlOptions(SqlServerDbContextOptionsBuilder options)
    {
        _ = options.UseDefaultSqlServerOptions();
    }

    /// <inheritdoc/>
    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);

        if (disposing)
        {
        }
    }
}
