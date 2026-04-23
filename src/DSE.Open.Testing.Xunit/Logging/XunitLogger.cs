// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using Microsoft.Extensions.Logging;
using Xunit;

namespace DSE.Open.Testing.Xunit.Logging;

/// <summary>
/// An <see cref="ILogger"/> that forwards log messages to an xUnit
/// <see cref="ITestOutputHelper"/>, prefixing each entry with a timestamp, thread id,
/// category and level.
/// </summary>
public class XunitLogger : ILogger
{
    private const string TimestampFormat = "yyyy-MM-dd HH:mm:ss.fffff";

    private static readonly string[] s_newLineChars = [Environment.NewLine];
    private static readonly string s_additionalLinePrefix = new(' ', TimestampFormat.Length + 1);

    private readonly string _category;
    private readonly LogLevel _minLogLevel;
    private readonly ITestOutputHelper _output;

    /// <summary>
    /// Initialises a new <see cref="XunitLogger"/>.
    /// </summary>
    /// <param name="output">The xUnit output helper to write to.</param>
    /// <param name="category">The category name that appears in each log line.</param>
    /// <param name="minLogLevel">The minimum <see cref="LogLevel"/> to emit. Entries below
    /// this level are suppressed.</param>
    /// <exception cref="ArgumentNullException"><paramref name="output"/> or
    /// <paramref name="category"/> is <see langword="null"/>.</exception>
    public XunitLogger(ITestOutputHelper output, string category, LogLevel minLogLevel)
    {
        ArgumentNullException.ThrowIfNull(output);
        ArgumentNullException.ThrowIfNull(category);

        _minLogLevel = minLogLevel;
        _category = category;
        _output = output;
    }

    /// <inheritdoc />
    public void Log<TState>(
        LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        if (!IsEnabled(logLevel))
        {
            return;
        }

        ArgumentNullException.ThrowIfNull(formatter);

        var firstLinePrefix = $"{DateTime.Now.ToString(TimestampFormat, CultureInfo.InvariantCulture)} <{Environment.CurrentManagedThreadId}> [{_category}] {logLevel}: ";

        var lines = formatter(state, exception).Split(s_newLineChars, StringSplitOptions.RemoveEmptyEntries);

        WriteLine(firstLinePrefix + lines.First());

        foreach (var line in lines.Skip(1))
        {
            WriteLine(s_additionalLinePrefix + line);
        }

        if (exception is not null)
        {
            lines = exception.ToString().Split(s_newLineChars, StringSplitOptions.RemoveEmptyEntries);
            foreach (var line in lines.Skip(1))
            {
                WriteLine(s_additionalLinePrefix + line);
            }
        }
    }

    /// <inheritdoc />
    public bool IsEnabled(LogLevel logLevel)
    {
        return logLevel >= _minLogLevel;
    }

    /// <inheritdoc />
    public IDisposable BeginScope<TState>(TState state) where TState : notnull
    {
        return new NullScope();
    }

    private void WriteLine(string message)
    {
#pragma warning disable CA1031 // Do not catch general exception types
        try
        {
            _output.WriteLine(message);
        }
        catch (Exception)
        {
            // We could fail because we're on a background thread and our captured ITestOutputHelper is
            // busted (if the test "completed" before the background thread fired).
            // So, ignore this. There isn't really anything we can do but hope the
            // caller has additional loggers registered
        }
#pragma warning restore CA1031 // Do not catch general exception types
    }

    private sealed class NullScope : IDisposable
    {
        public void Dispose()
        {
        }
    }
}
