// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using DSE.Open.DomainModel.Abstractions;
using DSE.Open.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Xunit.Abstractions;

namespace DSE.Open.Testing.Xunit.EntityFrameworkCore;

[SuppressMessage("Design", "CA1005:Avoid excessive parameters on generic types", Justification = "<Pending>")]
public abstract class SqliteEntityPersistenceTestsBase<
    [DynamicallyAccessedMembers(TrimmingHelper.DbContextDynamicallyAccessedMemberTypes)] TDbContext,
    [DynamicallyAccessedMembers(TrimmingHelper.EntityDynamicallyAccessedMemberTypes)] TEntity,
    TId>
    : SqliteStoredObjectPersistenceTestsBase<TDbContext, TEntity>
    where TDbContext : DbContext
    where TEntity : class, IEntity<TId>
    where TId : struct, IEquatable<TId>
{
    protected SqliteEntityPersistenceTestsBase(ITestOutputHelper output) : base(output)
    {
    }

    protected override Task<TEntity?> GetSavedEntityAsync(TDbContext dataContext, TEntity original)
    {
        Guard.IsNotNull(dataContext);
        Guard.IsNotNull(original);

        return dataContext.Set<TEntity>().SingleOrDefaultWithIdAsync(original.Id);
    }
}

