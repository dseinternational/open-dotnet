// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Testing.Xunit.Logging;
using Microsoft.Extensions.Logging;

namespace DSE.Open.Testing.Xunit.Tests.Logging;

public class XunitLoggerProviderTests
{
    [Fact]
    public void Ctor_NullOutput_Throws()
    {
        _ = Assert.Throws<ArgumentNullException>(() => new XunitLoggerProvider(null!));
    }

    [Fact]
    public void CreateLogger_ReturnsXunitLogger()
    {
        var output = new CapturingTestOutputHelper();
        using var provider = new XunitLoggerProvider(output);

        var logger = provider.CreateLogger("SomeCategory");

        Assert.IsType<XunitLogger>(logger);
    }

    [Fact]
    public void CreateLogger_RespectsMinLevel()
    {
        var output = new CapturingTestOutputHelper();
        using var provider = new XunitLoggerProvider(output, LogLevel.Warning);

        var logger = provider.CreateLogger("cat");
        logger.LogInformation("quiet");
        logger.LogWarning("loud");

        Assert.DoesNotContain(output.Lines, l => l.Contains("quiet", StringComparison.Ordinal));
        Assert.Contains(output.Lines, l => l.Contains("loud", StringComparison.Ordinal));
    }

    [Fact]
    public void Dispose_IsIdempotent()
    {
        var output = new CapturingTestOutputHelper();
        var provider = new XunitLoggerProvider(output);
        provider.Dispose();
        provider.Dispose();
    }

    private sealed class CapturingTestOutputHelper : ITestOutputHelper
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
}
