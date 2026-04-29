// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Data.Common;
using System.Diagnostics.CodeAnalysis;
using DSE.Open.EntityFrameworkCore;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DSE.Open.Testing.Xunit.EntityFrameworkCore;

/// <summary>
/// A <see cref="DbContextTestsBase{TContext}"/> that configures <typeparamref name="TContext"/>
/// to use a SQLite connection (defaulting to an in-memory database).
/// </summary>
/// <typeparam name="TContext">The type of the <see cref="DbContext"/>.</typeparam>
public abstract class SqliteDbContextTestsBase<[DynamicallyAccessedMembers(TrimmingHelper.DbContextDynamicallyAccessedMemberTypes)] TContext>
    : DbContextTestsBase<TContext>
    where TContext : DbContext
{
    private readonly DbConnection _connection;

    /// <summary>
    /// Initializes a new instance of the <see cref="SqliteDbContextTestsBase{TContext}"/> class
    /// using an in-memory SQLite database.
    /// </summary>
    /// <param name="output">The xUnit test output helper.</param>
    protected SqliteDbContextTestsBase(ITestOutputHelper output) : this(null, output)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SqliteDbContextTestsBase{TContext}"/> class.
    /// </summary>
    /// <param name="connection">
    /// An optional SQLite connection string. When <see langword="null"/>, an in-memory database
    /// (<c>Filename=:memory:</c>) is used.
    /// </param>
    /// <param name="output">The xUnit test output helper.</param>
    protected SqliteDbContextTestsBase(string? connection, ITestOutputHelper output) : base(output)
    {
        EnsureDatabaseCreated = true;
        EnsureDatabaseInitialized = true;

        connection ??= "Filename=:memory:";

        _connection = new SqliteConnection(connection);
        _connection.Open();
    }

    /// <inheritdoc/>
    protected override void ConfigureDbContext(IServiceProvider services, DbContextOptionsBuilder options)
    {
        _ = options.UseSqlite(_connection);
    }

    /// <inheritdoc/>
    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);

        if (disposing)
        {
            _connection.Dispose();
        }
    }
}

