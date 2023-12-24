// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using DSE.Open.DomainModel.Entities;
using DSE.Open.EntityFrameworkCore;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Xunit.Abstractions;

namespace DSE.Open.Testing.Xunit.EntityFrameworkCore;

public abstract class SqlServerStoredObjectPersistenceTestsBase<
    [DynamicallyAccessedMembers(TrimmingHelper.DbContextDynamicallyAccessedMemberTypes)] TDbContext,
    [DynamicallyAccessedMembers(TrimmingHelper.EntityDynamicallyAccessedMemberTypes)] TEntity>
    : SqlServerDbContextTestsBase<TDbContext>
    where TDbContext : DbContext
    where TEntity : class, IStoredObject
{
    protected SqlServerStoredObjectPersistenceTestsBase(ITestOutputHelper output) : base(output)
    {
    }

    protected virtual bool CanUpdate { get; }

    protected virtual Func<TDbContext, DbUpdateConcurrencyException, CancellationToken, Task<int>>? DeleteConcurrencyExceptionHandler => null;

    protected virtual Func<TDbContext, DbUpdateConcurrencyException, CancellationToken, Task<int>>? UpdateConcurrencyExceptionHandler => null;

    protected abstract Task<TEntity> CreateEntityAsync(TDbContext dataContext);

    protected virtual void AssertEquivalent(TEntity entity1, TEntity entity2)
    {
        _ = entity1.Should().BeEquivalentTo(entity2, config =>
        {
            if (typeof(TEntity).IsAssignableTo(typeof(IUpdateTimesTracked)))
            {
                config = config.Excluding((mi) => mi.Name == nameof(IUpdateTimesTracked.Created) || mi.Name == nameof(IUpdateTimesTracked.Updated));
            }

            if (typeof(TEntity).IsAssignableTo(typeof(ITimestamped)))
            {
                config = config.Excluding((mi) => mi.Name == nameof(ITimestamped.Timestamp));
            }

            return config.Excluding(o => o.Initialization);
        });
    }

    protected virtual Task UpdateAsync(TEntity entity, TDbContext dataContext)
    {
        return Task.CompletedTask;
    }

    /// <summary>
    /// Provides an opportunity to set up the database as needed for the test.
    /// </summary>
    /// <param name="dataContext"></param>
    /// <remarks>The context provided is different from the context used to create/add the entity and save changes.</remarks>
    protected virtual Task SetupAsync(TDbContext dataContext)
    {
        return Task.CompletedTask;
    }

    protected virtual Task CleanupAsync(TEntity? original)
    {
        return Task.CompletedTask;
    }

    protected virtual Task BeforeRemoveAsync(TDbContext dataContext, TEntity savedEntity)
    {
        return Task.CompletedTask;
    }

    /// <summary>
    /// Override to include related data.
    /// </summary>
    /// <param name="dataContext"></param>
    /// <returns></returns>
    protected virtual IQueryable<TEntity> GetEntityQuery(TDbContext dataContext)
    {
        ArgumentNullException.ThrowIfNull(dataContext);
        return dataContext.Set<TEntity>();
    }

    /// <summary>
    /// Override to include related data.
    /// </summary>
    /// <param name="dataContext"></param>
    /// <param name="original"></param>
    /// <returns></returns>
    protected abstract Task<TEntity?> GetSavedEntityAsync(TDbContext dataContext, TEntity original);

    [Fact]
    public virtual async Task CanAddQueryUpdateAndRemove()
    {
        TEntity? original = null;

        var dbContext = GetDbContext();

        try
        {
            await SetupAsync(dbContext).ConfigureAwait(false);

            // Add

            original = await CreateEntityAsync(dbContext).ConfigureAwait(false);

            if (original is null)
            {
                throw new InvalidOperationException("An entity is required.");
            }

            Assert.Equal(StoredObjectInitialization.Created, original.Initialization);

            _ = dbContext.Add(original);

            _ = await dbContext.SaveChangesAsync().ConfigureAwait(false);

            dbContext.Entry(original).State = EntityState.Detached;

            // EntityQuery

            var retrieved = await GetSavedEntityAsync(dbContext, original).ConfigureAwait(false);

            Assert.NotNull(retrieved);

            Assert.Equal(StoredObjectInitialization.Materialized, retrieved.Initialization);

            dbContext.Entry(retrieved).State = EntityState.Detached;

            AssertEquivalent(retrieved, original);

            // Update

            if (CanUpdate)
            {
                var toUpdate = await GetSavedEntityAsync(dbContext, original).ConfigureAwait(false);
                Assert.NotNull(toUpdate);
                await UpdateAsync(toUpdate, dbContext).ConfigureAwait(false);
                _ = UpdateConcurrencyExceptionHandler is not null
                    ? await dbContext.SaveChangesAsync(UpdateConcurrencyExceptionHandler).ConfigureAwait(false)
                    : await dbContext.SaveChangesAsync().ConfigureAwait(false);
                dbContext.Entry(toUpdate).State = EntityState.Detached;

                retrieved = await GetSavedEntityAsync(dbContext, original).ConfigureAwait(false);
                Assert.NotNull(retrieved);
                dbContext.Entry(retrieved).State = EntityState.Detached;

                AssertEquivalent(retrieved, toUpdate);
            }

            // Remove

            var toRemove = await GetSavedEntityAsync(dbContext, original).ConfigureAwait(false);

            if (toRemove is not null)
            {
                await BeforeRemoveAsync(dbContext, toRemove).ConfigureAwait(false);
                _ = dbContext.Remove(toRemove);
                _ = DeleteConcurrencyExceptionHandler is not null
                    ? await dbContext.SaveChangesAsync(DeleteConcurrencyExceptionHandler).ConfigureAwait(false)
                    : await dbContext.SaveChangesAsync().ConfigureAwait(false);
            }

            var deleted = await GetSavedEntityAsync(dbContext, original).ConfigureAwait(false);
            Assert.Null(deleted);
        }
        finally
        {
            await CleanupAsync(original).ConfigureAwait(false);
        }
    }
}

