// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using Microsoft.Extensions.Logging;
using Xunit;

namespace DSE.Open.Testing.Xunit.Logging;

/// <summary>
/// An <see cref="ILoggerProvider"/> that creates <see cref="XunitLogger"/> instances
/// writing to a shared <see cref="ITestOutputHelper"/>.
/// </summary>
public class XunitLoggerProvider : ILoggerProvider
{
    private readonly ITestOutputHelper _output;
    private readonly LogLevel _minLevel;

    /// <summary>
    /// Initialises a new <see cref="XunitLoggerProvider"/>.
    /// </summary>
    /// <param name="output">The xUnit output helper to write to.</param>
    /// <param name="minLevel">The minimum <see cref="LogLevel"/> for created loggers.
    /// Defaults to <see cref="LogLevel.Trace"/>.</param>
    /// <exception cref="ArgumentNullException"><paramref name="output"/> is
    /// <see langword="null"/>.</exception>
    public XunitLoggerProvider(ITestOutputHelper output, LogLevel minLevel = LogLevel.Trace)
    {
        ArgumentNullException.ThrowIfNull(output);
        _output = output;
        _minLevel = minLevel;
    }

    /// <inheritdoc />
    public ILogger CreateLogger(string categoryName)
    {
        return new XunitLogger(_output, categoryName, _minLevel);
    }

    /// <summary>
    /// Releases any resources held by this provider. The default implementation is a
    /// no-op; subclasses may override to release their own state.
    /// </summary>
    protected virtual void Dispose(bool disposing)
    {
    }

    /// <inheritdoc />
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
