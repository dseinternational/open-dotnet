// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using Microsoft.Extensions.Logging;
using Moq;

namespace DSE.Open.Notifications.Tests;

public sealed class NotificationLoggerTests
{
    [Fact]
    public void LogTrace_WritesAtTrace()
    {
        var logger = new Mock<ILogger>();
        logger.Setup(l => l.IsEnabled(LogLevel.Trace)).Returns(true);
        var n = Notification.Trace("TEST123001", "m");

        NotificationLogger.LogTrace(logger.Object, n);

        VerifyLogCalled(logger, LogLevel.Trace);
    }

    [Fact]
    public void LogDebug_WritesAtDebug()
    {
        var logger = new Mock<ILogger>();
        logger.Setup(l => l.IsEnabled(LogLevel.Debug)).Returns(true);
        var n = Notification.Debug("TEST123001", "m");

        NotificationLogger.LogDebug(logger.Object, n);

        VerifyLogCalled(logger, LogLevel.Debug);
    }

    [Fact]
    public void LogInformation_WritesAtInformation()
    {
        var logger = new Mock<ILogger>();
        logger.Setup(l => l.IsEnabled(LogLevel.Information)).Returns(true);
        var n = Notification.Information("TEST123001", "m");

        NotificationLogger.LogInformation(logger.Object, n);

        VerifyLogCalled(logger, LogLevel.Information);
    }

    [Fact]
    public void LogWarning_WritesAtWarning()
    {
        var logger = new Mock<ILogger>();
        logger.Setup(l => l.IsEnabled(LogLevel.Warning)).Returns(true);
        var n = Notification.Warning("TEST123001", "m");

        NotificationLogger.LogWarning(logger.Object, n);

        VerifyLogCalled(logger, LogLevel.Warning);
    }

    [Fact]
    public void LogError_WritesAtError()
    {
        var logger = new Mock<ILogger>();
        logger.Setup(l => l.IsEnabled(LogLevel.Error)).Returns(true);
        var n = Notification.Error("TEST123001", "m");

        NotificationLogger.LogError(logger.Object, n);

        VerifyLogCalled(logger, LogLevel.Error);
    }

    [Fact]
    public void LogCritical_WritesAtCritical()
    {
        var logger = new Mock<ILogger>();
        logger.Setup(l => l.IsEnabled(LogLevel.Critical)).Returns(true);
        var n = Notification.Critical("TEST123001", "m");

        NotificationLogger.LogCritical(logger.Object, n);

        VerifyLogCalled(logger, LogLevel.Critical);
    }

    [Theory]
    [InlineData(NotificationLevel.Trace, LogLevel.Trace)]
    [InlineData(NotificationLevel.Debug, LogLevel.Debug)]
    [InlineData(NotificationLevel.Information, LogLevel.Information)]
    [InlineData(NotificationLevel.Warning, LogLevel.Warning)]
    [InlineData(NotificationLevel.Error, LogLevel.Error)]
    [InlineData(NotificationLevel.Critical, LogLevel.Critical)]
    public void Log_MapsNotificationLevelToLogLevel(NotificationLevel level, LogLevel expected)
    {
        var logger = new Mock<ILogger>();
        logger.Setup(l => l.IsEnabled(expected)).Returns(true);
        var n = new Notification("TEST123001", level, "m");

        NotificationLogger.Log(logger.Object, n);

        VerifyLogCalled(logger, expected);
    }

    [Fact]
    public void Log_WithExplicitLogLevel_UsesThatLevel()
    {
        var logger = new Mock<ILogger>();
        logger.Setup(l => l.IsEnabled(LogLevel.Warning)).Returns(true);
        var n = Notification.Information("TEST123001", "m");

        NotificationLogger.Log(logger.Object, LogLevel.Warning, n);

        VerifyLogCalled(logger, LogLevel.Warning);
    }

    [Fact]
    public void Log_InvalidNotificationLevel_ThrowsArgumentOutOfRange()
    {
        var logger = Mock.Of<ILogger>();
        var n = new FakeNotification { Level = (NotificationLevel)999 };

        _ = Assert.Throws<ArgumentOutOfRangeException>(() => NotificationLogger.Log(logger, n));
    }

    [Fact]
    public void LogTrace_NullLogger_Throws()
    {
        var n = Notification.Trace("TEST123001", "m");
        _ = Assert.Throws<ArgumentNullException>(() => NotificationLogger.LogTrace(null!, n));
    }

    [Fact]
    public void LogTrace_NullNotification_Throws()
    {
        var logger = Mock.Of<ILogger>();
        _ = Assert.Throws<ArgumentNullException>(() => NotificationLogger.LogTrace(logger, null!));
    }

    [Fact]
    public void LogDebug_NullLogger_Throws()
    {
        var n = Notification.Debug("TEST123001", "m");
        _ = Assert.Throws<ArgumentNullException>(() => NotificationLogger.LogDebug(null!, n));
    }

    [Fact]
    public void LogDebug_NullNotification_Throws()
    {
        var logger = Mock.Of<ILogger>();
        _ = Assert.Throws<ArgumentNullException>(() => NotificationLogger.LogDebug(logger, null!));
    }

    [Fact]
    public void LogInformation_NullLogger_Throws()
    {
        var n = Notification.Information("TEST123001", "m");
        _ = Assert.Throws<ArgumentNullException>(() => NotificationLogger.LogInformation(null!, n));
    }

    [Fact]
    public void LogInformation_NullNotification_Throws()
    {
        var logger = Mock.Of<ILogger>();
        _ = Assert.Throws<ArgumentNullException>(() => NotificationLogger.LogInformation(logger, null!));
    }

    [Fact]
    public void LogWarning_NullLogger_Throws()
    {
        var n = Notification.Warning("TEST123001", "m");
        _ = Assert.Throws<ArgumentNullException>(() => NotificationLogger.LogWarning(null!, n));
    }

    [Fact]
    public void LogWarning_NullNotification_Throws()
    {
        var logger = Mock.Of<ILogger>();
        _ = Assert.Throws<ArgumentNullException>(() => NotificationLogger.LogWarning(logger, null!));
    }

    [Fact]
    public void LogError_NullLogger_Throws()
    {
        var n = Notification.Error("TEST123001", "m");
        _ = Assert.Throws<ArgumentNullException>(() => NotificationLogger.LogError(null!, n));
    }

    [Fact]
    public void LogError_NullNotification_Throws()
    {
        var logger = Mock.Of<ILogger>();
        _ = Assert.Throws<ArgumentNullException>(() => NotificationLogger.LogError(logger, null!));
    }

    [Fact]
    public void LogCritical_NullLogger_Throws()
    {
        var n = Notification.Critical("TEST123001", "m");
        _ = Assert.Throws<ArgumentNullException>(() => NotificationLogger.LogCritical(null!, n));
    }

    [Fact]
    public void LogCritical_NullNotification_Throws()
    {
        var logger = Mock.Of<ILogger>();
        _ = Assert.Throws<ArgumentNullException>(() => NotificationLogger.LogCritical(logger, null!));
    }

    [Fact]
    public void Log_MapLevel_NullLogger_Throws()
    {
        var n = Notification.Information("TEST123001", "m");
        _ = Assert.Throws<ArgumentNullException>(() => NotificationLogger.Log(null!, n));
    }

    [Fact]
    public void Log_MapLevel_NullNotification_Throws()
    {
        var logger = Mock.Of<ILogger>();
        _ = Assert.Throws<ArgumentNullException>(() => NotificationLogger.Log(logger, (INotification)null!));
    }

    [Fact]
    public void Log_ExplicitLevel_NullLogger_Throws()
    {
        var n = Notification.Information("TEST123001", "m");
        _ = Assert.Throws<ArgumentNullException>(() => NotificationLogger.Log(null!, LogLevel.Information, n));
    }

    [Fact]
    public void Log_ExplicitLevel_NullNotification_Throws()
    {
        var logger = Mock.Of<ILogger>();
        _ = Assert.Throws<ArgumentNullException>(
            () => NotificationLogger.Log(logger, LogLevel.Information, (INotification)null!));
    }

    private static void VerifyLogCalled(Mock<ILogger> logger, LogLevel expected)
    {
#pragma warning disable CA1873 // Moq expression tree — arguments are matchers, not real values
        logger.Verify(
            l => l.Log(
                expected,
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception?>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.AtLeastOnce);
#pragma warning restore CA1873
    }

    private sealed class FakeNotification : INotification
    {
        public Diagnostics.DiagnosticCode Code { get; init; }
        public NotificationLevel Level { get; init; }
        public string Message { get; init; } = "m";
    }
}
