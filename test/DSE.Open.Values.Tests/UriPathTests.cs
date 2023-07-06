// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Values.Tests;

public class UriPathTests
{
    [Theory]
    [InlineData("/")]
    [InlineData("/a")]
    [InlineData("/a/")]
    [InlineData("/a/b")]
    [InlineData("a/b/")]
    [InlineData("/a/b/")]
    public void TryParse_WithInvalidValue_ShouldReturnFalseWithDefaultResult(string value)
    {
        // Arrange
        var expected = default(UriPath);

        // Act
        var actual = UriPath.TryParse(value, out var result);

        // Assert
        Assert.False(actual);
        Assert.Equal(expected, result);
    }
}
