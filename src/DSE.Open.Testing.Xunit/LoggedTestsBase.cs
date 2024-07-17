// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Testing.Xunit.Logging;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace DSE.Open.Testing.Xunit;

public abstract class LoggedTestsBase : IDisposable
{
    private LoggerFactory? _loggerFactory;
    private ILogger? _logger;
    private bool _disposed;

    protected LoggedTestsBase(ITestOutputHelper output)
    {
        ArgumentNullException.ThrowIfNull(output);
        Output = output;
    }

    protected virtual void ConfigureLogging(ILoggingBuilder builder)
    {
        _ = builder.AddProvider(GetLoggerProvider());
        _ = builder.SetMinimumLevel(LogLevel.Debug);
    }

    protected virtual ILoggerProvider GetLoggerProvider()
    {
        return new XunitLoggerProvider(Output);
    }

    protected virtual IEnumerable<ILoggerProvider> GetLoggerProviders()
    {
#pragma warning disable CA2000 // Dispose objects before losing scope
        return [new XunitLoggerProvider(Output)];
#pragma warning restore CA2000 // Dispose objects before losing scope
    }

    public ILoggerFactory LoggerFactory => _loggerFactory ??= new(GetLoggerProviders());

    public ITestOutputHelper Output { get; }

    public ILogger Logger => _logger ??= LoggerFactory.CreateLogger(GetType().FullName ?? GetType().Name);

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

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}

