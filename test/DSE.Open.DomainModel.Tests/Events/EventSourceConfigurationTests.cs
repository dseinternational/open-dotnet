// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.DomainModel.Events;

namespace DSE.Open.DomainModel.Tests.Events;

public class EventSourceConfigurationTests
{
    [Fact]
    public void GetEventSource_ReturnsAbsoluteUri()
    {
        var uri = EventSourceConfiguration.GetEventSource(typeof(DomainEventFake));

        Assert.True(uri.IsAbsoluteUri);
    }

    [Fact]
    public void GetEventSource_ReflectsCurrentSourceProvider()
    {
        var previous = EventSourceConfiguration.SourceProvider;
        try
        {
            var custom = new Uri("https://example.invalid/events");
            EventSourceConfiguration.SourceProvider = _ => custom;

            var uri = EventSourceConfiguration.GetEventSource(typeof(DomainEventFake));

            Assert.Equal(custom, uri);
        }
        finally
        {
            EventSourceConfiguration.SourceProvider = previous;
        }
    }

    [Fact]
    public void SourceProvider_SetNull_Throws()
    {
        _ = Assert.Throws<ArgumentNullException>(() =>
            EventSourceConfiguration.SourceProvider = null!);
    }
}
