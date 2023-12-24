// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Data.Common;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace DSE.Open.EntityFrameworkCore.Tests;

public abstract class SqliteInMemoryTestBase<TContext> : IDisposable
    where TContext : DbContext
{
    private readonly DbConnection _connection;
    private readonly DbContextOptions<TContext> _contextOptions;
    private bool _disposed;

    protected SqliteInMemoryTestBase()
    {
        _connection = new SqliteConnection("Filename=:memory:");
        _connection.Open();

        _contextOptions = new DbContextOptionsBuilder<TContext>()
            .UseSqlite(_connection)
            .Options;

#pragma warning disable CA2214 // Do not call overridable methods in constructors
        using var db = CreateContext();
#pragma warning restore CA2214 // Do not call overridable methods in constructors

        _ = db.Database.EnsureCreated();
    }

    public virtual TContext CreateContext()
    {
        return CreateContext(_contextOptions);
    }

    public abstract TContext CreateContext(DbContextOptions<TContext> contextOptions);

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _connection.Dispose();
            }

            _disposed = true;
        }
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
