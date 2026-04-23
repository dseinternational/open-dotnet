// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using Microsoft.Extensions.Logging;

namespace DSE.Open.Testing.Xunit;

public class LoggedTestsBaseTests
{
    [Fact]
    public void Ctor_NullOutput_Throws()
    {
        _ = Assert.Throws<ArgumentNullException>(() => new NoopSubclass(null!));
    }

    [Fact]
    public void LoggerFactory_BuiltLazilyAndCached()
    {
        using var subject = new NoopSubclass(new NullTestOutputHelper());
        var factory1 = subject.LoggerFactory;
        var factory2 = subject.LoggerFactory;
        Assert.Same(factory1, factory2);
    }

    [Fact]
    public void Logger_CategoryMatchesFullName()
    {
        using var subject = new NoopSubclass(new NullTestOutputHelper());
        Assert.NotNull(subject.Logger);
    }

    [Fact]
    public void Logger_CanLog()
    {
        var output = new CapturingTestOutputHelper();
        using var subject = new NoopSubclass(output);
        subject.Logger.LogInformation("hello");
        Assert.Contains(output.Lines, l => l.Contains("hello", StringComparison.Ordinal));
    }

    [Fact]
    public void Dispose_IsIdempotent()
    {
        var subject = new NoopSubclass(new NullTestOutputHelper());
        _ = subject.LoggerFactory;
        subject.Dispose();
        subject.Dispose();
    }

    [Fact]
    public void Dispose_NeverAccessedLoggerFactory_DoesNotThrow()
    {
        var subject = new NoopSubclass(new NullTestOutputHelper());
        subject.Dispose();
    }

    private sealed class NoopSubclass : LoggedTestsBase
    {
        public NoopSubclass(ITestOutputHelper output) : base(output)
        {
        }
    }

    private sealed class NullTestOutputHelper : ITestOutputHelper
    {
        public string Output => string.Empty;
        public void Write(string message) { }
        public void Write(string format, params object[] args) { }
        public void WriteLine(string message) { }
        public void WriteLine(string format, params object[] args) { }
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
