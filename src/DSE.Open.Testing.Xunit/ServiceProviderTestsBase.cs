// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace DSE.Open.Testing.Xunit;

/// <summary>
/// Base implementation for tests that build a service provider.
/// </summary>
public abstract class ServiceProviderTestsBase : LoggedTestsBase
{
    protected ServiceProviderTestsBase(ITestOutputHelper output) : base(output)
    {
        var services = new ServiceCollection();

#pragma warning disable CA2214 // Do not call overridable methods in constructors
        ConfigureServices(services);
#pragma warning restore CA2214 // Do not call overridable methods in constructors

        ServiceProvider = services.BuildServiceProvider();
    }

    public IServiceProvider ServiceProvider { get; }

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

