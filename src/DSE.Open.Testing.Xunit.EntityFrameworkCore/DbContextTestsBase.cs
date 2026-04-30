// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using DSE.Open.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace DSE.Open.Testing.Xunit.EntityFrameworkCore;

/// <summary>
/// Provides <see cref="DbContext"/> instances from a constructed <see cref="IServiceProvider"/>,
/// enabling lifetime management and the addition of other required services.
/// </summary>
/// <typeparam name="TContext">The type of the <see cref="DbContext"/>.</typeparam>
public abstract class DbContextTestsBase<[DynamicallyAccessedMembers(TrimmingHelper.DbContextDynamicallyAccessedMemberTypes)] TContext>
    : ServiceProviderTestsBase
    where TContext : DbContext
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DbContextTestsBase{TContext}"/> class.
    /// </summary>
    /// <param name="output">The xUnit test output helper.</param>
    protected DbContextTestsBase(ITestOutputHelper output) : base(output)
    {
    }

    /// <summary>
    /// Gets or sets a value indicating whether the database should be created (via
    /// <c>EnsureCreatedAsync</c>) before returning a <typeparamref name="TContext"/>.
    /// </summary>
    public bool EnsureDatabaseCreated { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether <see cref="InitializeDbContext(TContext)"/>
    /// should be invoked before returning a <typeparamref name="TContext"/>.
    /// </summary>
    public bool EnsureDatabaseInitialized { get; set; }

    /// <inheritdoc/>
    protected override void ConfigureServices(IServiceCollection services)
    {
        _ = services.AddLogging(ConfigureLogging);

        AddDbContext(services);
    }

    /// <summary>
    /// Adds the <typeparamref name="TContext"/> to the supplied service collection.
    /// </summary>
    /// <param name="services">The service collection to add the context to.</param>
    protected virtual void AddDbContext(IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        _ = services.AddDbContext<TContext>(ConfigureDbContext, ServiceLifetime.Transient);
    }

    /// <summary>
    /// Configures the <see cref="DbContextOptionsBuilder"/> used to build options for
    /// <typeparamref name="TContext"/>.
    /// </summary>
    /// <param name="services">The service provider used to resolve dependencies.</param>
    /// <param name="options">The options builder to configure.</param>
    protected virtual void ConfigureDbContext(IServiceProvider services, DbContextOptionsBuilder options)
    {
        ArgumentNullException.ThrowIfNull(options);

        _ = options
            .EnableDetailedErrors()
            .EnableSensitiveDataLogging();
    }

    /// <summary>
    /// Gets a <typeparamref name="TContext"/> instance, synchronously waiting on
    /// <see cref="GetDbContextAsync"/>.
    /// </summary>
    /// <returns>A configured <typeparamref name="TContext"/>.</returns>
    protected virtual TContext GetDbContext()
    {
        return GetDbContextAsync().GetAwaiter().GetResult();
    }

    /// <summary>
    /// Asynchronously resolves a <typeparamref name="TContext"/> from the service provider,
    /// honouring <see cref="EnsureDatabaseCreated"/> and <see cref="EnsureDatabaseInitialized"/>.
    /// </summary>
    /// <returns>A configured <typeparamref name="TContext"/>.</returns>
    [UnconditionalSuppressMessage("AOT",
        "IL3050:Calling members annotated with 'RequiresDynamicCodeAttribute' may break functionality when AOT compiling.",
        Justification = "Test that does not require Native AOT")]
    protected virtual async Task<TContext> GetDbContextAsync()
    {
        var context = ServiceProvider.GetRequiredService<TContext>();

        if (EnsureDatabaseCreated)
        {
            _ = await context.Database.EnsureCreatedAsync().ConfigureAwait(false);
        }

        if (EnsureDatabaseInitialized)
        {
            await InitializeDbContext(context).ConfigureAwait(false);
        }

        return context;
    }

    /// <summary>
    /// Provides an opportunity for derived classes to initialize the database (for example,
    /// to seed data) when <see cref="EnsureDatabaseInitialized"/> is <see langword="true"/>.
    /// </summary>
    /// <param name="context">The <typeparamref name="TContext"/> to initialize.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    protected virtual Task InitializeDbContext(TContext context)
    {
        return Task.CompletedTask;
    }
}
