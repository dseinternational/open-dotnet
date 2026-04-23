// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using Microsoft.Extensions.Logging;
using Moq;

namespace DSE.Open.Notifications.Tests;

public sealed class NotificationLoggerExtensionsTests
{
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

        logger.Object.Log(n);

        VerifyLogCalled(logger, expected);
    }

    [Fact]
    public void Log_WithExplicitLogLevel_UsesThatLevel()
    {
        var logger = new Mock<ILogger>();
        logger.Setup(l => l.IsEnabled(LogLevel.Error)).Returns(true);
        var n = Notification.Information("TEST123001", "m");

        logger.Object.Log(LogLevel.Error, n);

        VerifyLogCalled(logger, LogLevel.Error);
    }

    [Fact]
    public void LogTrace_DelegatesToTrace()
    {
        var logger = new Mock<ILogger>();
        logger.Setup(l => l.IsEnabled(LogLevel.Trace)).Returns(true);
        var n = Notification.Trace("TEST123001", "m");

        logger.Object.LogTrace(n);

        VerifyLogCalled(logger, LogLevel.Trace);
    }

    [Fact]
    public void LogDebug_DelegatesToDebug()
    {
        var logger = new Mock<ILogger>();
        logger.Setup(l => l.IsEnabled(LogLevel.Debug)).Returns(true);
        var n = Notification.Debug("TEST123001", "m");

        logger.Object.LogDebug(n);

        VerifyLogCalled(logger, LogLevel.Debug);
    }

    [Fact]
    public void LogInformation_DelegatesToInformation()
    {
        var logger = new Mock<ILogger>();
        logger.Setup(l => l.IsEnabled(LogLevel.Information)).Returns(true);
        var n = Notification.Information("TEST123001", "m");

        logger.Object.LogInformation(n);

        VerifyLogCalled(logger, LogLevel.Information);
    }

    [Fact]
    public void LogWarning_DelegatesToWarning()
    {
        var logger = new Mock<ILogger>();
        logger.Setup(l => l.IsEnabled(LogLevel.Warning)).Returns(true);
        var n = Notification.Warning("TEST123001", "m");

        logger.Object.LogWarning(n);

        VerifyLogCalled(logger, LogLevel.Warning);
    }

    [Fact]
    public void LogError_DelegatesToError()
    {
        var logger = new Mock<ILogger>();
        logger.Setup(l => l.IsEnabled(LogLevel.Error)).Returns(true);
        var n = Notification.Error("TEST123001", "m");

        logger.Object.LogError(n);

        VerifyLogCalled(logger, LogLevel.Error);
    }

    [Fact]
    public void LogCritical_DelegatesToCritical()
    {
        var logger = new Mock<ILogger>();
        logger.Setup(l => l.IsEnabled(LogLevel.Critical)).Returns(true);
        var n = Notification.Critical("TEST123001", "m");

        logger.Object.LogCritical(n);

        VerifyLogCalled(logger, LogLevel.Critical);
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
}
