// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

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

        await mediator.PublishAsync(ev, TestContext.Current.CancellationToken);

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

        await dispatcher.PublishEventsAsync([entity], TestContext.Current.CancellationToken);

        var state = provider.GetRequiredService<TestState>();

        Assert.True(state.ContainsKey(ev.Instance.ToString()));
        Assert.True(state.ContainsKey(bev.Instance.ToString()));
    }

    [Fact]
    public async Task Dispatcher_PublishEvents_ReentrantEvent_IsDispatchedInSamePass()
    {
        var services = new ServiceCollection();

        _ = services.AddLogging();
        _ = services.AddSingleton(new TestState());
        _ = services.AddDomainEventDispatcher();
        _ = services.AddMessageHandler<DomainEventHandlerFake, DomainEventFake>();
        _ = services.AddMessageHandler<DomainReentrantTriggerEventHandlerFake, DomainReentrantTriggerEventFake>();

        var provider = services.BuildServiceProvider();

        var dispatcher = provider.GetRequiredService<IDomainEventDispatcher>();

        var entity = new EventRaisingEntityFake<Guid>();

        var trigger = new DomainReentrantTriggerEventFake(entity);
        entity.AddDomainEvent(trigger);

        await dispatcher.PublishEventsAsync([entity], TestContext.Current.CancellationToken);

        var state = provider.GetRequiredService<TestState>();

        Assert.True(state.ContainsKey(trigger.Instance.ToString()));

        var followupKeys = state.Keys
            .Where(k => k.StartsWith(DomainReentrantTriggerEventHandlerFake.FollowupKeyPrefix, StringComparison.Ordinal))
            .ToList();

        Assert.Single(followupKeys);

        var followupInstance = followupKeys[0][DomainReentrantTriggerEventHandlerFake.FollowupKeyPrefix.Length..];
        Assert.True(state.ContainsKey(followupInstance));
        Assert.False(entity.HasEvents);
    }

    [Fact]
    public async Task Dispatcher_PublishEvents_HandlerLoopsForever_ThrowsAfterIterationCap()
    {
        var services = new ServiceCollection();

        _ = services.AddLogging();
        _ = services.AddSingleton(new TestState());
        _ = services.AddDomainEventDispatcher();
        _ = services.AddMessageHandler<DomainLoopingEventHandlerFake, DomainLoopingEventFake>();

        var provider = services.BuildServiceProvider();

        var dispatcher = provider.GetRequiredService<IDomainEventDispatcher>();

        var entity = new EventRaisingEntityFake<Guid>();
        entity.AddDomainEvent(new DomainLoopingEventFake(entity));

        _ = await Assert.ThrowsAsync<InvalidOperationException>(
            () => dispatcher.PublishEventsAsync([entity], TestContext.Current.CancellationToken));
    }
}
