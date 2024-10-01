// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Testing.Xunit;
using DSE.Open.Values;
using Microsoft.Extensions.DependencyInjection;

namespace DSE.Open.Mediators.Tests;

public class MessageDispatcherTests : LoggedTestsBase
{
    public MessageDispatcherTests(ITestOutputHelper output) : base(output)
    {
    }

    private async Task RunTest(IServiceProvider provider)
    {
        var msg = new Message();

        var dispatcher = provider.GetRequiredService<IMessageDispatcher>();

        await dispatcher.PublishAsync(msg);

        Assert.True(msg.Handled);
        Assert.Equal(2, msg.HandledCount);
    }

    [Fact]
    public async Task AllMessagesHandled()
    {
        var services = new ServiceCollection();

        _ = services.AddLogging(ConfigureLogging);
        _ = services.AddScoped<IMessageDispatcher, MessageDispatcher>();
        _ = services.AddScoped<IMessageHandler<Message>, MessageHandler>();
        _ = services.AddScoped<IMessageHandler<DerivedMessage>, DerivedMessageHandler>();

        using var scope = services.BuildServiceProvider().GetRequiredService<IServiceScopeFactory>().CreateScope();

        var messagesToSend = new List<IMessage>
        {
            new Message(),
            new DerivedMessage()
        };

        var dispatcher = scope.ServiceProvider.GetRequiredService<IMessageDispatcher>();

        foreach (var message in messagesToSend)
        {
            await dispatcher.PublishAsync(message, TestContext.Current.CancellationToken);
        }

        foreach (var message in messagesToSend.OfType<Message>())
        {
            Assert.True(message.Handled);
            Assert.Equal(1, message.HandledCount);
        }
    }

    [Fact]
    public async Task SendsMessageTransientDispatcherTransientHandlers()
    {
        var services = new ServiceCollection();

        _ = services.AddLogging(ConfigureLogging);
        _ = services.AddTransient<IMessageDispatcher, MessageDispatcher>();
        _ = services.AddTransient<IMessageHandler<Message>, MessageHandler>();
        _ = services.AddTransient<IMessageHandler<Message>, MessageHandler>();

        using var scope = services.BuildServiceProvider().GetRequiredService<IServiceScopeFactory>().CreateScope();

        await RunTest(scope.ServiceProvider);
    }

    [Fact]
    public async Task SendsMessageScopedDispatcherScopedHandlers()
    {
        var services = new ServiceCollection();

        _ = services.AddLogging(ConfigureLogging);
        _ = services.AddMessageDispatcher();
        _ = services.AddMessageHandler<MessageHandler, Message>();
        _ = services.AddMessageHandler<MessageHandler, Message>();

        using var scope = services.BuildServiceProvider().GetRequiredService<IServiceScopeFactory>().CreateScope();

        await RunTest(scope.ServiceProvider);
    }

    [Fact]
    public async Task SendsMessageSingletonDispatcherSingletonHandlers()
    {
        var services = new ServiceCollection();

        _ = services.AddLogging(ConfigureLogging);
        _ = services.AddSingleton<IMessageDispatcher, MessageDispatcher>();
        _ = services.AddSingleton<IMessageHandler<Message>, MessageHandler>();
        _ = services.AddSingleton<IMessageHandler<Message>, MessageHandler>();

        using var scope = services.BuildServiceProvider().GetRequiredService<IServiceScopeFactory>().CreateScope();

        await RunTest(scope.ServiceProvider);
    }

    [Fact]
    public async Task SendsMessageSingletonDispatcherTransientHandlers()
    {
        var services = new ServiceCollection();

        _ = services.AddLogging(ConfigureLogging);
        _ = services.AddSingleton<IMessageDispatcher, MessageDispatcher>();
        _ = services.AddTransient<IMessageHandler<Message>, MessageHandler>();
        _ = services.AddTransient<IMessageHandler<Message>, MessageHandler>();

        using var scope = services.BuildServiceProvider().GetRequiredService<IServiceScopeFactory>().CreateScope();

        await RunTest(scope.ServiceProvider);
    }

    [Fact]
    public async Task SendsMessageSingletonDispatcherScopedHandlers()
    {
        var services = new ServiceCollection();

        _ = services.AddLogging(ConfigureLogging);
        _ = services.AddSingleton<IMessageDispatcher, MessageDispatcher>();
        _ = services.AddScoped<IMessageHandler<Message>, MessageHandler>();
        _ = services.AddScoped<IMessageHandler<Message>, MessageHandler>();

        using var scope = services.BuildServiceProvider().GetRequiredService<IServiceScopeFactory>().CreateScope();

        await RunTest(scope.ServiceProvider);
    }

    [Fact]
    public async Task SendsMessageScopedDispatcherTransientHandlers()
    {
        var services = new ServiceCollection();

        _ = services.AddLogging(ConfigureLogging);
        _ = services.AddScoped<IMessageDispatcher, MessageDispatcher>();
        _ = services.AddTransient<IMessageHandler<Message>, MessageHandler>();
        _ = services.AddTransient<IMessageHandler<Message>, MessageHandler>();

        using var scope = services.BuildServiceProvider().GetRequiredService<IServiceScopeFactory>().CreateScope();

        await RunTest(scope.ServiceProvider);
    }
}

public class Message : IMessage
{
    public Identifier Id { get; } = Identifier.New();

    public bool Handled => HandledCount > 0;

    public int HandledCount { get; private set; }

    public void SetHandled()
    {
        HandledCount++;
    }
}

public sealed class DerivedMessage : Message;

public sealed class MessageHandler : IMessageHandler<Message>
{
    public ValueTask HandleAsync(Message message, CancellationToken cancellationToken = default)
    {
        message.SetHandled();
        return ValueTask.CompletedTask;
    }
}

public sealed class DerivedMessageHandler : IMessageHandler<DerivedMessage>
{
    public ValueTask HandleAsync(DerivedMessage message, CancellationToken cancellationToken = default)
    {
        message.SetHandled();
        return ValueTask.CompletedTask;
    }
}

