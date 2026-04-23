// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace DSE.Open.Testing.Xunit;

/// <summary>
/// Base implementation for tests that build an <see cref="IServiceProvider"/> from a
/// <see cref="ServiceCollection"/> configured via <see cref="ConfigureServices"/>.
/// </summary>
/// <remarks>
/// The service provider is built lazily on the first access to
/// <see cref="ServiceProvider"/> and is disposed together with this instance. Tests that
/// never touch <see cref="ServiceProvider"/> do not pay the cost of building it.
/// </remarks>
public abstract class ServiceProviderTestsBase : LoggedTestsBase
{
    private IServiceProvider? _serviceProvider;

    /// <summary>
    /// Initialises a new <see cref="ServiceProviderTestsBase"/>.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="output"/> is
    /// <see langword="null"/>.</exception>
    protected ServiceProviderTestsBase(ITestOutputHelper output) : base(output)
    {
    }

    /// <summary>
    /// Gets the <see cref="IServiceProvider"/>, building it on first access.
    /// </summary>
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

    /// <summary>
    /// Registers services with the supplied <paramref name="services"/> collection.
    /// </summary>
    protected abstract void ConfigureServices(IServiceCollection services);

    /// <inheritdoc />
    protected override void Dispose(bool disposing)
    {
        if (disposing && _serviceProvider is IDisposable disposable)
        {
            disposable.Dispose();
            _serviceProvider = null;
        }

        base.Dispose(disposing);
    }
}
