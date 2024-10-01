// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Data.Common;
using System.Diagnostics.CodeAnalysis;
using DSE.Open.EntityFrameworkCore;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DSE.Open.Testing.Xunit.EntityFrameworkCore;

public abstract class SqliteDbContextTestsBase<[DynamicallyAccessedMembers(TrimmingHelper.DbContextDynamicallyAccessedMemberTypes)] TContext>
    : DbContextTestsBase<TContext>
    where TContext : DbContext
{
    private readonly DbConnection _connection;

    protected SqliteDbContextTestsBase(ITestOutputHelper output) : this(null, output)
    {
    }

    protected SqliteDbContextTestsBase(string? connection, ITestOutputHelper output) : base(output)
    {
        EnsureDatabaseCreated = true;
        EnsureDatabaseInitialized = true;

        connection ??= "Filename=:memory:";

        _connection = new SqliteConnection(connection);
        _connection.Open();
    }

    protected override void ConfigureDbContext(IServiceProvider services, DbContextOptionsBuilder options)
    {
        _ = options.UseSqlite(_connection);
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);

        if (disposing)
        {
            _connection.Dispose();
        }
    }
}

