// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using Microsoft.Extensions.DependencyInjection;

namespace DSE.Open.Testing.Xunit;

public class ServiceProviderTestsBaseTests
{
    [Fact]
    public void ServiceProvider_BuiltLazilyAndCached()
    {
        using var subject = new TrackingSubclass(new NullTestOutputHelper());
        Assert.Equal(0, subject.ConfigureCount);

        var provider1 = subject.ServiceProvider;
        Assert.Equal(1, subject.ConfigureCount);

        var provider2 = subject.ServiceProvider;
        Assert.Same(provider1, provider2);
        Assert.Equal(1, subject.ConfigureCount);
    }

    [Fact]
    public void ServiceProvider_ResolvesRegisteredService()
    {
        using var subject = new TrackingSubclass(new NullTestOutputHelper());
        var resolved = subject.ServiceProvider.GetRequiredService<SampleService>();
        Assert.NotNull(resolved);
    }

    [Fact]
    public void Dispose_NeverAccessedProvider_DoesNotBuildIt()
    {
        var subject = new TrackingSubclass(new NullTestOutputHelper());
        subject.Dispose();
        Assert.Equal(0, subject.ConfigureCount);
    }

    [Fact]
    public void Dispose_AccessedProvider_DisposesIt()
    {
        var subject = new TrackingSubclass(new NullTestOutputHelper());
        var service = subject.ServiceProvider.GetRequiredService<DisposableSampleService>();
        Assert.False(service.Disposed);

        subject.Dispose();

        Assert.True(service.Disposed);
    }

    [Fact]
    public void Dispose_IsIdempotent()
    {
        var subject = new TrackingSubclass(new NullTestOutputHelper());
        _ = subject.ServiceProvider;
        subject.Dispose();
        subject.Dispose();
    }

    private sealed class SampleService;

    private sealed class DisposableSampleService : IDisposable
    {
        public bool Disposed { get; private set; }
        public void Dispose() => Disposed = true;
    }

    private sealed class TrackingSubclass : ServiceProviderTestsBase
    {
        public TrackingSubclass(ITestOutputHelper output) : base(output)
        {
        }

        public int ConfigureCount { get; private set; }

        protected override void ConfigureServices(IServiceCollection services)
        {
            ConfigureCount++;
            _ = services.AddSingleton<SampleService>();
            _ = services.AddSingleton<DisposableSampleService>();
        }
    }

    private sealed class NullTestOutputHelper : ITestOutputHelper
    {
        public string Output => string.Empty;
        public void Write(string message) { }
        public void Write(string format, params object[] args) { }
        public void WriteLine(string message) { }
        public void WriteLine(string format, params object[] args) { }
    }
}
