// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Testing.Xunit.Logging;
using Microsoft.Extensions.Logging;
using Xunit;

namespace DSE.Open.Testing.Xunit;

/// <summary>
/// Base class for tests that need an <see cref="ILogger"/> that forwards output to
/// xUnit via <see cref="ITestOutputHelper"/>.
/// </summary>
/// <remarks>
/// The <see cref="LoggerFactory"/> and <see cref="Logger"/> are created lazily on first
/// access and disposed with this instance. Subclasses that wire up their own service
/// collection may pass <see cref="ConfigureLogging"/> to
/// <c>services.AddLogging(ConfigureLogging)</c> to route logs through the same xUnit
/// provider.
/// </remarks>
public abstract class LoggedTestsBase : IDisposable
{
    private LoggerFactory? _loggerFactory;
    private ILogger? _logger;
    private bool _disposed;

    /// <summary>
    /// Initialises a new <see cref="LoggedTestsBase"/>.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="output"/> is
    /// <see langword="null"/>.</exception>
    protected LoggedTestsBase(ITestOutputHelper output)
    {
        ArgumentNullException.ThrowIfNull(output);
        Output = output;
    }

    /// <summary>
    /// Configures an <see cref="ILoggingBuilder"/> to add the xUnit logger provider and
    /// set a minimum level of <see cref="LogLevel.Debug"/>.
    /// </summary>
    protected virtual void ConfigureLogging(ILoggingBuilder builder)
    {
        _ = builder.AddProvider(GetLoggerProvider());
        _ = builder.SetMinimumLevel(LogLevel.Debug);
    }

    /// <summary>
    /// Returns the <see cref="ILoggerProvider"/> to use when
    /// <see cref="ConfigureLogging"/> is invoked. Override to supply a custom provider.
    /// </summary>
    protected virtual ILoggerProvider GetLoggerProvider()
    {
        return new XunitLoggerProvider(Output);
    }

    /// <summary>
    /// Returns the <see cref="ILoggerProvider"/>s used by the lazily created
    /// <see cref="LoggerFactory"/>. Override to supply custom providers.
    /// </summary>
    protected virtual IEnumerable<ILoggerProvider> GetLoggerProviders()
    {
#pragma warning disable CA2000 // Dispose objects before losing scope
        return [new XunitLoggerProvider(Output)];
#pragma warning restore CA2000 // Dispose objects before losing scope
    }

    /// <summary>
    /// Gets a lazily created <see cref="ILoggerFactory"/> configured from
    /// <see cref="GetLoggerProviders"/>.
    /// </summary>
    public ILoggerFactory LoggerFactory => _loggerFactory ??= new(GetLoggerProviders());

    /// <summary>
    /// Gets the <see cref="ITestOutputHelper"/> supplied by the xUnit runner.
    /// </summary>
    public ITestOutputHelper Output { get; }

    /// <summary>
    /// Gets a lazily created <see cref="ILogger"/> whose category is this test class's
    /// full name.
    /// </summary>
    public ILogger Logger => _logger ??= LoggerFactory.CreateLogger(GetType().FullName ?? GetType().Name);

    /// <summary>
    /// Disposes the <see cref="LoggerFactory"/> when <paramref name="disposing"/> is
    /// <see langword="true"/>. Safe to call multiple times.
    /// </summary>
    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
        {
            return;
        }

        if (disposing && _loggerFactory is not null)
        {
            _loggerFactory.Dispose();
        }

        _disposed = true;
    }

    /// <inheritdoc />
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
