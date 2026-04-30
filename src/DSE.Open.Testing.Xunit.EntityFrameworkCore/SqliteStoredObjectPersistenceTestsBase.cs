// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using DSE.Open.DomainModel.Entities;
using DSE.Open.EntityFrameworkCore;
using AwesomeAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DSE.Open.Testing.Xunit.EntityFrameworkCore;

/// <summary>
/// A SQLite persistence test base for objects that implement <see cref="IStoredObject"/>,
/// exercising the create, query, update and remove lifecycle.
/// </summary>
/// <typeparam name="TDbContext">The type of the <see cref="DbContext"/>.</typeparam>
/// <typeparam name="TEntity">The stored object type under test.</typeparam>
public abstract class SqliteStoredObjectPersistenceTestsBase<
    [DynamicallyAccessedMembers(TrimmingHelper.DbContextDynamicallyAccessedMemberTypes)] TDbContext,
    [DynamicallyAccessedMembers(TrimmingHelper.EntityDynamicallyAccessedMemberTypes)] TEntity>
    : SqliteDbContextTestsBase<TDbContext>
    where TDbContext : DbContext
    where TEntity : class, IStoredObject
{
    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="SqliteStoredObjectPersistenceTestsBase{TDbContext, TEntity}"/> class.
    /// </summary>
    /// <param name="output">The xUnit test output helper.</param>
    protected SqliteStoredObjectPersistenceTestsBase(ITestOutputHelper output) : base(output)
    {
    }

    /// <summary>
    /// Gets a value indicating whether the update phase of the lifecycle test should be executed.
    /// </summary>
    protected virtual bool CanUpdate { get; }

    /// <summary>
    /// Gets an optional handler invoked when a <see cref="DbUpdateConcurrencyException"/> is
    /// thrown while saving a delete.
    /// </summary>
    protected virtual Func<TDbContext, DbUpdateConcurrencyException, CancellationToken, Task<int>>? DeleteConcurrencyExceptionHandler => null;

    /// <summary>
    /// Gets an optional handler invoked when a <see cref="DbUpdateConcurrencyException"/> is
    /// thrown while saving an update.
    /// </summary>
    protected virtual Func<TDbContext, DbUpdateConcurrencyException, CancellationToken, Task<int>>? UpdateConcurrencyExceptionHandler => null;

    /// <summary>
    /// Creates a new <typeparamref name="TEntity"/> instance to be persisted by the test.
    /// </summary>
    /// <param name="dataContext">The data context that will be used to add the entity.</param>
    /// <returns>A new <typeparamref name="TEntity"/>.</returns>
    protected abstract Task<TEntity> CreateEntityAsync(TDbContext dataContext);

    /// <summary>
    /// Asserts that <paramref name="entity1"/> and <paramref name="entity2"/> are equivalent,
    /// excluding tracking and timestamp members that are expected to differ across saves.
    /// </summary>
    /// <param name="entity1">The first entity.</param>
    /// <param name="entity2">The second entity.</param>
    protected virtual void AssertEquivalent(TEntity entity1, TEntity entity2)
    {
        _ = entity1.Should().BeEquivalentTo(entity2, config =>
        {
            if (typeof(TEntity).IsAssignableTo(typeof(IUpdateTimesTracked)))
            {
                config = config.Excluding(mi => mi.Name == nameof(IUpdateTimesTracked.Created) || mi.Name == nameof(IUpdateTimesTracked.Updated));
            }

            if (typeof(TEntity).IsAssignableTo(typeof(ITimestamped)))
            {
                config = config.Excluding(mi => mi.Name == nameof(ITimestamped.Timestamp));
            }

            return config.Excluding(o => o.Initialization);
        });
    }

    /// <summary>
    /// Mutates <paramref name="entity"/> to produce changes that the lifecycle test should
    /// persist during the update phase.
    /// </summary>
    /// <param name="entity">The entity retrieved from the data context to be updated.</param>
    /// <param name="dataContext">The data context tracking the entity.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
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

    /// <summary>
    /// Provides an opportunity to clean up after the test has finished, regardless of outcome.
    /// </summary>
    /// <param name="original">The entity that was created by the test, if any.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    protected virtual Task CleanupAsync(TEntity? original)
    {
        return Task.CompletedTask;
    }

    /// <summary>
    /// Invoked before the saved entity is removed, allowing related data to be deleted first.
    /// </summary>
    /// <param name="dataContext">The data context that will perform the remove.</param>
    /// <param name="savedEntity">The entity that is about to be removed.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
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

    /// <summary>
    /// Verifies that a <typeparamref name="TEntity"/> can be created, persisted, retrieved,
    /// optionally updated, and removed using the configured <typeparamref name="TDbContext"/>.
    /// </summary>
    /// <returns>A task representing the asynchronous test.</returns>
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

