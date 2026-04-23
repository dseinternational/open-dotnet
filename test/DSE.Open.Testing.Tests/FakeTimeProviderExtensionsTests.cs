// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using Microsoft.Extensions.Time.Testing;

namespace DSE.Open.Testing;

public class FakeTimeProviderExtensionsTests
{
    [Fact]
    public void AddMinutes_AdvancesClock()
    {
        var start = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero);
        var provider = new FakeTimeProvider(start);

        _ = provider.AddMinutes(5);

        Assert.Equal(start.AddMinutes(5), provider.GetUtcNow());
    }

    [Fact]
    public void AddMinutes_NegativeValue_Throws()
    {
        var provider = new FakeTimeProvider();
        _ = Assert.Throws<ArgumentOutOfRangeException>(() => provider.AddMinutes(-1));
    }

    [Fact]
    public void AddMinutes_ReturnsSameInstance()
    {
        var provider = new FakeTimeProvider();
        var returned = provider.AddMinutes(1);
        Assert.Same(provider, returned);
    }

    [Fact]
    public void AddMinutes_Chained_AccumulatesOffsets()
    {
        var start = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero);
        var provider = new FakeTimeProvider(start);

        _ = provider.AddMinutes(1).AddMinutes(2).AddMinutes(3);

        Assert.Equal(start.AddMinutes(6), provider.GetUtcNow());
    }

    [Fact]
    public void AddMinutes_NullProvider_Throws()
    {
        _ = Assert.Throws<ArgumentNullException>(
            () => FakeTimeProviderExtensions.AddMinutes(null!, 1));
    }

    [Fact]
    public void AddSeconds_AdvancesClock()
    {
        var start = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero);
        var provider = new FakeTimeProvider(start);

        _ = provider.AddSeconds(45);

        Assert.Equal(start.AddSeconds(45), provider.GetUtcNow());
    }

    [Fact]
    public void AddSeconds_NegativeValue_Throws()
    {
        var provider = new FakeTimeProvider();
        _ = Assert.Throws<ArgumentOutOfRangeException>(() => provider.AddSeconds(-1));
    }

    [Fact]
    public void AddSeconds_ReturnsSameInstance()
    {
        var provider = new FakeTimeProvider();
        var returned = provider.AddSeconds(1);
        Assert.Same(provider, returned);
    }

    [Fact]
    public void AddSeconds_NullProvider_Throws()
    {
        _ = Assert.Throws<ArgumentNullException>(
            () => FakeTimeProviderExtensions.AddSeconds(null!, 1));
    }
}
