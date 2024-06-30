// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

namespace DSE.Open.Values.Tests;

[SuppressMessage("ReSharper", "EqualExpressionComparison")] // For testing equality operators
#pragma warning disable CS1718 // Comparison made to same variable - for testing equality operators
public class AgeInMonthsTests
{
    [Fact]
    public void Constructor_sets_total_months()
    {
        var age = new AgeInMonths(1);
        Assert.Equal(1, age.TotalMonths);
    }

    [Fact]
    public void Constructor_sets_total_months_from_year()
    {
        var age = new AgeInMonths(1, 0);
        Assert.Equal(12, age.TotalMonths);
    }

    [Theory]
    [InlineData(1, 0, "1:0")]
    [InlineData(0, 1, "0:1")]
    [InlineData(0, 13, "1:1")]
    [InlineData(10, 24, "12:0")]
    [InlineData(178956970, 0, "178956970:0")]
    [InlineData(0, int.MaxValue, "178956970:7")]
    public void ToString_outputs_correct_format(int years, int months, string expected)
    {
        var age = new AgeInMonths(years, months);
        Assert.Equal(expected, age.ToString());
    }

    [Theory]
    [InlineData("0:1", 1)]
    [InlineData("1:0", 12)]
    [InlineData("10:1", 121)]
    public void Parse_parses_correctly(string value, int expectedTotalMonths)
    {
        var age = AgeInMonths.Parse(value, null);
        Assert.Equal(expectedTotalMonths, age.TotalMonths);
    }

    [Fact]
    public void Serialize_deserialize()
    {
        var age = new AgeInMonths(1, 2);
        var json = JsonSerializer.Serialize(age);
        var age2 = JsonSerializer.Deserialize<AgeInMonths>(json);
        Assert.Equal(age, age2);
    }

    [Theory]
    [InlineData(12, 1, 0)]
    [InlineData(13, 1, 1)]
    [InlineData(24, 2, 0)]
    [InlineData(25, 2, 1)]
    public void YearsAndMonths_ShouldReturnCorrectValues(int totalMonths, int expectedYears, int expectedMonths)
    {
        // Arrange
        var age = new AgeInMonths(totalMonths);

        // Act
        var (years, months) = age.YearsAndMonths();

        // Assert
        Assert.Equal(expectedYears, years);
        Assert.Equal(expectedMonths, months);
    }

    [Fact]
    public void AddMonths_WithOverflow_ShouldThrowException()
    {
        // Arrange
        var age = new AgeInMonths(int.MaxValue);

        // Act
        void Act() => age.AddMonths(1);

        // Assert
        Assert.Throws<OverflowException>(Act);
    }

    [Fact]
    public void AddYears_WithOverflow_ShouldThrowException()
    {
        // Arrange
        var age = new AgeInMonths(int.MaxValue);

        // Act
        void Act() => age.AddYears(1);

        // Assert
        Assert.Throws<OverflowException>(Act);
    }

    [Theory]
    [InlineData(0, int.MaxValue)]
    [InlineData(int.MaxValue / 12, int.MaxValue % 12)]
    public void New_WithMaxValues_ShouldWork(int years, int months)
    {
        _ = new AgeInMonths(years, months);
    }

    [Theory]
    [InlineData(1, int.MaxValue - 11)]
    [InlineData(int.MaxValue / 12, (int.MaxValue % 12) + 1)]
    public void New_WithValueTooLarge_ShouldThrowArgumentOutOfRange(int years, int months)
    {
        // Arrange
        // Act
        void Act() => _ = new AgeInMonths(years, months);

        // Assert
        Assert.Throws<ArgumentOutOfRangeException>(Act);
    }

    [Fact]
    public void New_WithNegativeValue_ShouldThrowArgumentOutOfRangeException()
    {
        // Act
        void Act() => _ = new AgeInMonths(-1, 0);
        void Act2() => _ = new AgeInMonths(-1);

        // Assert
        Assert.Throws<ArgumentOutOfRangeException>(Act);
        Assert.Throws<ArgumentOutOfRangeException>(Act2);
    }

    [Theory]
    [InlineData(1, 0, 1)]
    [InlineData(0, 1, -1)]
    [InlineData(1, 1, 0)]
    public void CompareTo_ShouldCorrectlyCompare(int left, int right, int expected)
    {
        // Arrange
        var age1 = new AgeInMonths(left);
        var age2 = new AgeInMonths(right);

        // Act
        var result = age1.CompareTo(age2);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void GreaterThan()
    {
        // Arrange
        var left = new AgeInMonths(1);
        var right = new AgeInMonths(0);

        // Act
        var result = left > right;

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void LessThan()
    {
        // Arrange
        var left = new AgeInMonths(0);
        var right = new AgeInMonths(1);

        // Act
        var result = left < right;

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void GreaterThanOrEqual()
    {
        // Arrange
        var left = new AgeInMonths(1);
        var right = new AgeInMonths(0);

        // Act
        var result = left >= right && left >= left && right >= right;

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void LessThanOrEqual()
    {
        // Arrange
        var left = new AgeInMonths(0);
        var right = new AgeInMonths(1);

        // Act
        var result = left <= right && left <= left && right <= right;

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Equal()
    {
        // Arrange
        var left = new AgeInMonths(1);
        var right = new AgeInMonths(1);

        // Act
        var result = left == right && right == left;

        // Assert
        Assert.True(result);
    }
}
