// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace DSE.Open.Testing.Xunit;

/// <summary>
/// Base implementation for tests that build a service provider.
/// </summary>
public abstract class ServiceProviderTestsBase : LoggedTestsBase
{
    private IServiceProvider? _serviceProvider;

    protected ServiceProviderTestsBase(ITestOutputHelper output) : base(output)
    {
    }

    public IServiceProvider ServiceProvider
    {
        get
        {
            if (_serviceProvider is not null)
            {
                return _serviceProvider;
            }

            var services = new ServiceCollection();

            ConfigureServices(services);

            _serviceProvider = services.BuildServiceProvider();

            return _serviceProvider;
        }
    }

    protected abstract void ConfigureServices(IServiceCollection services);

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            ((ServiceProvider)ServiceProvider).Dispose();
        }

        base.Dispose(disposing);
    }
}
