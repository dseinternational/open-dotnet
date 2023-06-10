// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Tests;

public class DateTimeOffsetExtensionsTests
{
    [Fact]
    public void Truncate_ToMilliseconds()
    {
        // Arrange
        var dateTimeOffset = new DateTimeOffset(DateTime.MaxValue.Ticks, TimeSpan.Zero);

        // Act
        var result = dateTimeOffset.Truncate(DateTimeTruncation.Millisecond);

        // Assert
        Assert.Equal(0, result.Microsecond);
        Assert.Equal(999, result.Millisecond);
    }

    [Fact]
    public void Truncate_ToSeconds()
    {
        // Arrange
        const int seconds = 5;
        var dateTimeOffset = new DateTimeOffset(2023, 1, 1, 12, 0, seconds, 123, TimeSpan.Zero);

        // Act
        var result = dateTimeOffset.Truncate(DateTimeTruncation.Second);

        // Assert
        Assert.Equal(0, result.Microsecond);
        Assert.Equal(0, result.Millisecond);
        Assert.Equal(seconds, result.Second);
    }

    [Fact]
    public void Truncate_ToMinutes()
    {
        // Arrange
        const int minutes = 5;
        var dateTimeOffset = new DateTimeOffset(2023, 1, 1, 12, minutes, 0, 123, TimeSpan.Zero);

        // Act
        var result = dateTimeOffset.Truncate(DateTimeTruncation.Minute);

        // Assert
        Assert.Equal(0, result.Microsecond);
        Assert.Equal(0, result.Millisecond);
        Assert.Equal(0, result.Second);
        Assert.Equal(minutes, result.Minute);
    }

    [Fact]
    public void Truncate_ToHours()
    {
        // Arrange
        const int hours = 5;
        var dateTimeOffset = new DateTimeOffset(2023, 1, 1, hours, 0, 0, 123, TimeSpan.Zero);

        // Act
        var result = dateTimeOffset.Truncate(DateTimeTruncation.Hour);

        // Assert
        Assert.Equal(0, result.Microsecond);
        Assert.Equal(0, result.Millisecond);
        Assert.Equal(0, result.Second);
        Assert.Equal(0, result.Minute);
        Assert.Equal(hours, result.Hour);
    }

    [Fact]
    public void Truncate_ToDays()
    {
        // Arrange
        const int days = 5;
        var dateTimeOffset = new DateTimeOffset(2023, 1, days, 0, 0, 0, 123, TimeSpan.Zero);

        // Act
        var result = dateTimeOffset.Truncate(DateTimeTruncation.Day);

        // Assert
        Assert.Equal(0, result.Microsecond);
        Assert.Equal(0, result.Millisecond);
        Assert.Equal(0, result.Second);
        Assert.Equal(0, result.Minute);
        Assert.Equal(0, result.Hour);
        Assert.Equal(days, result.Day);
    }

    [Fact]
    public void Truncate_ToMonths()
    {
        // Arrange
        const int months = 5;
        var dateTimeOffset = new DateTimeOffset(2023, months, 1, 0, 0, 0, 123, TimeSpan.Zero);

        // Act
        var result = dateTimeOffset.Truncate(DateTimeTruncation.Month);

        // Assert
        Assert.Equal(0, result.Microsecond);
        Assert.Equal(0, result.Millisecond);
        Assert.Equal(0, result.Second);
        Assert.Equal(0, result.Minute);
        Assert.Equal(0, result.Hour);
        Assert.Equal(1, result.Day);
        Assert.Equal(months, result.Month);
    }

    [Fact]
    public void Truncate_ToYears()
    {
        // Arrange
        const int years = 5;
        var dateTimeOffset = new DateTimeOffset(years, 1, 1, 0, 0, 0, 123, TimeSpan.Zero);

        // Act
        var result = dateTimeOffset.Truncate(DateTimeTruncation.Year);

        // Assert
        Assert.Equal(0, result.Microsecond);
        Assert.Equal(0, result.Millisecond);
        Assert.Equal(0, result.Second);
        Assert.Equal(0, result.Minute);
        Assert.Equal(0, result.Hour);
        Assert.Equal(1, result.Day);
        Assert.Equal(1, result.Month);
        Assert.Equal(years, result.Year);
    }
}
