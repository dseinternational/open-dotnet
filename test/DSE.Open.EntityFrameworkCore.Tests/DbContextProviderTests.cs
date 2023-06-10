// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Testing.Xunit;
using Microsoft.Extensions.DependencyInjection;

namespace DSE.Open.EntityFrameworkCore.Tests;

public class DbContextProviderTests : LoggedTestsBase
{
    public DbContextProviderTests(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void AddDbContextProviderConfiguresServices()
    {
        var services = new ServiceCollection();

        _ = services.AddLogging(ConfigureLogging);

        _ = services.AddDbContextProvider<StorageModelDbContextFake>();

        var provider = services.BuildServiceProvider();

        var contextProvider = provider.GetRequiredService<IDbContextProvider>();

        Assert.NotNull(contextProvider);
    }

    [Fact]
    public void DbContextProviderProvidesDefaultConfiguredContext()
    {
        var services = new ServiceCollection();

        _ = services.AddLogging(ConfigureLogging);

        _ = services.AddDbContext<StorageModelDbContextFake>();

        _ = services.AddDbContextProvider<StorageModelDbContextFake>();

        var provider = services.BuildServiceProvider();

        var contextProvider = provider.GetRequiredService<IDbContextProvider>();

        var context = contextProvider.GetDbContext<StorageModelDbContextFake>();

        Assert.NotNull(context);
    }

    [Fact]
    public void DbContextProviderProvidesNamedConfiguredContext()
    {
        var services = new ServiceCollection();

        _ = services.AddLogging(ConfigureLogging);

        string? configRequested = null;

        _ = services.AddDbContext<StorageModelDbContextFake>((provider, options) =>
        {
            var config = provider.GetRequiredService<IDbContextConfiguration<StorageModelDbContextFake>>();
            configRequested = config.Name;
        });

        _ = services.AddDbContextProvider<StorageModelDbContextFake>();

        var provider = services.BuildServiceProvider();

        var contextProvider = provider.GetRequiredService<IDbContextProvider>();

        var context = contextProvider.GetDbContext<StorageModelDbContextFake>("test");

        Assert.NotNull(context);
        Assert.NotNull(configRequested);
        Assert.Equal("test", configRequested);
    }
}
