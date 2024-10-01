// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using DSE.Open.EntityFrameworkCore;
using DSE.Open.EntityFrameworkCore.SqlServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Xunit;

namespace DSE.Open.Testing.Xunit.EntityFrameworkCore;

public abstract class SqlServerDbContextTestsBase<[DynamicallyAccessedMembers(TrimmingHelper.DbContextDynamicallyAccessedMemberTypes)] TContext>
    : DbContextTestsBase<TContext>
    where TContext : DbContext
{
    protected SqlServerDbContextTestsBase(ITestOutputHelper output) : this(null, output)
    {
    }

    protected SqlServerDbContextTestsBase(string? connection, ITestOutputHelper output) : base(output)
    {
    }

    public abstract string ConnectionString { get; }


    protected override void ConfigureDbContext(IServiceProvider services, DbContextOptionsBuilder options)
    {
        _ = options.UseSqlServer(ConnectionString, ConfigureSqlOptions);
    }

    protected virtual void ConfigureSqlOptions(SqlServerDbContextOptionsBuilder options)
    {
        _ = options.UseDefaultSqlServerOptions();
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);

        if (disposing)
        {
        }
    }
}
