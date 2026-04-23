// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Testing.Xunit.Logging;
using Microsoft.Extensions.Logging;

namespace DSE.Open.Testing.Xunit.Tests.Logging;

public class XunitLoggerTests
{
    [Fact]
    public void Ctor_NullOutput_Throws()
    {
        _ = Assert.Throws<ArgumentNullException>(() => new XunitLogger(null!, "c", LogLevel.Debug));
    }

    [Fact]
    public void Ctor_NullCategory_Throws()
    {
        var output = new RecordingTestOutputHelper();
        _ = Assert.Throws<ArgumentNullException>(() => new XunitLogger(output, null!, LogLevel.Debug));
    }

    [Theory]
    [InlineData(LogLevel.Trace, LogLevel.Trace, true)]
    [InlineData(LogLevel.Debug, LogLevel.Trace, true)]
    [InlineData(LogLevel.Trace, LogLevel.Debug, false)]
    [InlineData(LogLevel.Information, LogLevel.Warning, false)]
    [InlineData(LogLevel.Warning, LogLevel.Warning, true)]
    [InlineData(LogLevel.Critical, LogLevel.Warning, true)]
    public void IsEnabled_ComparesAgainstMinimum(LogLevel requested, LogLevel min, bool expected)
    {
        var output = new RecordingTestOutputHelper();
        var logger = new XunitLogger(output, "cat", min);
        Assert.Equal(expected, logger.IsEnabled(requested));
    }

    [Fact]
    public void Log_BelowMinimum_DoesNothing()
    {
        var output = new RecordingTestOutputHelper();
        var logger = new XunitLogger(output, "cat", LogLevel.Warning);
        logger.LogInformation("quiet");
        Assert.Empty(output.Lines);
    }

    [Fact]
    public void Log_AtOrAboveMinimum_WritesSingleLine()
    {
        var output = new RecordingTestOutputHelper();
        var logger = new XunitLogger(output, "cat", LogLevel.Debug);
        logger.LogInformation("hello");
        var line = Assert.Single(output.Lines);
        Assert.Contains("[cat]", line, StringComparison.Ordinal);
        Assert.Contains("Information", line, StringComparison.Ordinal);
        Assert.EndsWith("hello", line, StringComparison.Ordinal);
    }

    [Fact]
    public void Log_MultilineMessage_WritesContinuationLines()
    {
        var output = new RecordingTestOutputHelper();
        var logger = new XunitLogger(output, "cat", LogLevel.Debug);
        var message = string.Join(Environment.NewLine, "first", "second", "third");
#pragma warning disable CA2254 // Template is parameterised only to avoid literal warnings
        logger.Log(LogLevel.Information, message);
#pragma warning restore CA2254
        Assert.Equal(3, output.Lines.Count);
        Assert.EndsWith("first", output.Lines[0], StringComparison.Ordinal);
        Assert.EndsWith("second", output.Lines[1], StringComparison.Ordinal);
        Assert.EndsWith("third", output.Lines[2], StringComparison.Ordinal);
    }

    [Fact]
    public void Log_WithException_IncludesStackLines()
    {
        var output = new RecordingTestOutputHelper();
        var logger = new XunitLogger(output, "cat", LogLevel.Debug);

        Exception captured;
        try
        {
            throw new InvalidOperationException("boom");
        }
        catch (InvalidOperationException ex)
        {
            captured = ex;
        }

        logger.LogError(captured, "something failed");

        Assert.True(output.Lines.Count >= 2,
            "Expected at least a main line plus one stack line");
        Assert.Contains(output.Lines, l => l.Contains("something failed", StringComparison.Ordinal));
    }

    [Fact]
    public void Log_SwallowsTestOutputHelperExceptions()
    {
        var output = new ThrowingTestOutputHelper();
        var logger = new XunitLogger(output, "cat", LogLevel.Debug);

        // Must not throw.
        logger.LogInformation("hello");
    }

    [Fact]
    public void BeginScope_ReturnsDisposable()
    {
        var output = new RecordingTestOutputHelper();
        var logger = new XunitLogger(output, "cat", LogLevel.Debug);
        var scope = logger.BeginScope("state");
        Assert.NotNull(scope);
        scope.Dispose();
    }

    private sealed class RecordingTestOutputHelper : ITestOutputHelper
    {
        public List<string> Lines { get; } = [];
        public string Output => string.Join(Environment.NewLine, Lines);

        public void Write(string message) => Lines.Add(message);

        public void Write(string format, params object[] args)
            => Lines.Add(string.Format(CultureInfo.InvariantCulture, format, args));

        public void WriteLine(string message) => Lines.Add(message);

        public void WriteLine(string format, params object[] args)
            => Lines.Add(string.Format(CultureInfo.InvariantCulture, format, args));
    }

    private sealed class ThrowingTestOutputHelper : ITestOutputHelper
    {
        public string Output => string.Empty;

        public void Write(string message) => throw new InvalidOperationException("boom");
        public void Write(string format, params object[] args) => throw new InvalidOperationException("boom");
        public void WriteLine(string message) => throw new InvalidOperationException("boom");
        public void WriteLine(string format, params object[] args) => throw new InvalidOperationException("boom");
    }
}
