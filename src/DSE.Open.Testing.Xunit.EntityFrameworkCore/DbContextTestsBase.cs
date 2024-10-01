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
    protected DbContextTestsBase(ITestOutputHelper output) : base(output)
    {
    }

    public bool EnsureDatabaseCreated { get; set; }

    public bool EnsureDatabaseInitialized { get; set; }

    protected override void ConfigureServices(IServiceCollection services)
    {
        _ = services.AddLogging(ConfigureLogging);

        AddDbContext(services);
    }

    protected virtual void AddDbContext(IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        _ = services.AddDbContext<TContext>(ConfigureDbContext, ServiceLifetime.Transient);
    }

    protected virtual void ConfigureDbContext(IServiceProvider services, DbContextOptionsBuilder options)
    {
        ArgumentNullException.ThrowIfNull(options);

        _ = options
            .EnableDetailedErrors()
            .EnableSensitiveDataLogging();
    }

    protected virtual TContext GetDbContext()
    {
        return GetDbContextAsync().GetAwaiter().GetResult();
    }

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

    protected virtual Task InitializeDbContext(TContext context)
    {
        return Task.CompletedTask;
    }
}
