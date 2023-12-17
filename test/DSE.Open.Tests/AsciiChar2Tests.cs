// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Tests;

public class AsciiChar2Tests
{
    [Fact]
    public void TryParse_WithInvalidLength_ShouldReturnFalse()
    {
        // Arrange
        var input = "abc"u8.ToArray();

        // Act
        var result = AsciiChar2.TryParse(input, default, out var value);

        // Assert
        Assert.False(result);
        Assert.Equal(default, value);
    }

    [Fact]
    public void TryParse_WithInvalidChars_ShouldReturnFalse()
    {
        // Arrange
        const char ch = (char)129;
        Span<char> input = [ch, ch];

        // Act
        var result = AsciiChar2.TryParse(input, default, out var value);

        // Assert
        Assert.False(result);
        Assert.Equal(default, value);
    }
}
