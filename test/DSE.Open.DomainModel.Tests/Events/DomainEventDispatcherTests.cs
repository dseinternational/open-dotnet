// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.DomainModel.Abstractions.Events;
using DSE.Open.DomainModel.Events;
using DSE.Open.DomainModel.Tests.Entities;
using DSE.Open.Mediators;
using Microsoft.Extensions.DependencyInjection;

namespace DSE.Open.DomainModel.Tests.Events;

public class DomainEventDispatcherTests
{
    [Fact]
    public async Task Mediator_Publish()
    {
        var services = new ServiceCollection();

        _ = services.AddLogging();
        _ = services.AddSingleton(new TestState());
        _ = services.AddMessageDispatcher();
        _ = services.AddMessageHandler<DomainEventHandlerFake, DomainEventFake>();

        var provider = services.BuildServiceProvider();

        var mediator = provider.GetRequiredService<IMessageDispatcher>();

        var ev = new DomainEventFake("Test");

        await mediator.PublishAsync(ev);

        var state = provider.GetRequiredService<TestState>();

        Assert.True(state.ContainsKey(ev.Instance.ToString()));
    }

    [Fact]
    public async Task Dispatcher_PublishEvents()
    {
        var services = new ServiceCollection();

        _ = services.AddLogging();
        _ = services.AddSingleton(new TestState());
        _ = services.AddDomainEventDispatcher();
        _ = services.AddMessageHandler<DomainEventHandlerFake, DomainEventFake>();
        _ = services.AddMessageHandler<DomainBackgroundEventHandlerFake, DomainBackgroundEventFake>();

        var provider = services.BuildServiceProvider();

        var dispatcher = provider.GetRequiredService<IDomainEventDispatcher>();

        var entity = new EventRaisingEntityFake<Guid>();

        var ev = entity.AddFakeEvent();

        var bev = entity.AddFakeBackgroundEvent();

        await dispatcher.PublishEventsAsync(new[] { entity });

        var state = provider.GetRequiredService<TestState>();

        Assert.True(state.ContainsKey(ev.Instance.ToString()));
        Assert.True(state.ContainsKey(bev.Instance.ToString()));
    }
}
