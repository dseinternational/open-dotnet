// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Abstractions.Tests;

public class PaginationOptionsTests
{
    [Fact]
    public void Default_ShouldUseDefaultConstants()
    {
        // Arrange
        var expected = new PaginationOptions(PaginationOptions.DefaultPageSize, PaginationOptions.DefaultPageNumber);

        // Act
        var actual = PaginationOptions.Default;

        // Assert
        Assert.Equal(expected.PageSize, actual.PageSize);
        Assert.Equal(expected.PageNumber, actual.PageNumber);
    }

    [Fact]
    public void New_WithPageNumberZero_ShouldThrowArgumentOutOfRange()
    {
        // Arrange
        static void Act() => _ = new PaginationOptions(pageSize: 1, pageNumber: 0);

        // Assert
        _ = Assert.Throws<ArgumentOutOfRangeException>(Act);
    }

    [Fact]
    public void New_WithPageSizeZero_ShouldThrowArgumentOutOfRangeException()
    {
        // Arrange
        static void Act() => _ = new PaginationOptions(pageSize: 0, pageNumber: 1);

        // Assert
        _ = Assert.Throws<ArgumentOutOfRangeException>(Act);
    }

    [Theory]
    [InlineData(1, 1, 0)]
    [InlineData(1, 2, 1)]
    [InlineData(2, 1, 0)]
    [InlineData(2, 2, 2)]
    public void SkipCount(int pageSize, int pageNumber, int expected)
    {
        // Arrange
        var options = new PaginationOptions(pageSize, pageNumber);

        // Act
        var actual = options.SkipCount;

        // Assert
        Assert.Equal(expected, actual);
    }
}
