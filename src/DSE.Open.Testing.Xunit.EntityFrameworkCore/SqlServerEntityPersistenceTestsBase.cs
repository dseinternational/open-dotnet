// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using DSE.Open.DomainModel.Entities;
using DSE.Open.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DSE.Open.Testing.Xunit.EntityFrameworkCore;

/// <summary>
/// A SQL Server persistence test base for entities that implement <see cref="IEntity{TId}"/>,
/// providing an <see cref="GetSavedEntityAsync"/> implementation that retrieves the entity
/// by its identifier.
/// </summary>
/// <typeparam name="TDbContext">The type of the <see cref="DbContext"/>.</typeparam>
/// <typeparam name="TEntity">The entity type under test.</typeparam>
/// <typeparam name="TId">The type of the entity identifier.</typeparam>
[SuppressMessage("Design", "CA1005:Avoid excessive parameters on generic types", Justification = "<Pending>")]
public abstract class SqlServerEntityPersistenceTestsBase<
    [DynamicallyAccessedMembers(TrimmingHelper.DbContextDynamicallyAccessedMemberTypes)] TDbContext,
    [DynamicallyAccessedMembers(TrimmingHelper.EntityDynamicallyAccessedMemberTypes)] TEntity,
    TId>
    : SqlServerStoredObjectPersistenceTestsBase<TDbContext, TEntity>
    where TDbContext : DbContext
    where TEntity : class, IEntity<TId>
    where TId : struct, IEquatable<TId>
{
    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="SqlServerEntityPersistenceTestsBase{TDbContext, TEntity, TId}"/> class.
    /// </summary>
    /// <param name="output">The xUnit test output helper.</param>
    protected SqlServerEntityPersistenceTestsBase(ITestOutputHelper output) : base(output)
    {
    }

    /// <inheritdoc/>
    protected override Task<TEntity?> GetSavedEntityAsync(TDbContext dataContext, TEntity original)
    {
        ArgumentNullException.ThrowIfNull(dataContext);
        ArgumentNullException.ThrowIfNull(original);

        return dataContext.Set<TEntity>().SingleOrDefaultWithIdAsync(original.Id);
    }
}

