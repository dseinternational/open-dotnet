// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Tests;

public class AsciiCharTests
{
    [Theory]
    [MemberData(nameof(AsciiTestData.ValidAsciiCharBytes), MemberType = typeof(AsciiTestData))]
    public void Cast_from_valid_value(byte value)
    {
        var v = (AsciiChar)value;
        Assert.Equal(value, (byte)v);
    }

    [Theory]
    [InlineData('a', 'A', 0)]
    [InlineData('A', 'a', 0)]
    [InlineData('A', 'A', 0)]
    [InlineData('a', 'a', 0)]
    [InlineData('a', 'B', -1)]
    [InlineData('X', 'B', 1)]
    [InlineData('0', '0', 0)]
    [InlineData('1', '1', 0)]
    [InlineData('0', '1', -1)]
    [InlineData('1', '0', 1)]
    [InlineData('6', '2', 1)]
    [InlineData(':', ':', 0)]
    [InlineData('\t', '\t', 0)]
    [InlineData('?', '?', 0)]
    public void Compare_case_insensitive(char a, char b, int expected)
    {
        var c = ((AsciiChar)a).CompareToCaseInsensitive((AsciiChar)b);
        Assert.Equal(expected, c);
    }

    [Fact]
    public void TryParse_WithAsciiByte_ShouldReturnTrue()
    {
        // Arrange
        var input = "a"u8.ToArray();

        // Act
        var result = AsciiChar.TryParse(input, default, out var value);

        // Assert
        Assert.True(result);
        Assert.Equal('a', value);
    }

    [Fact]
    public void TryParse_WithNonAsciiByte_ShouldReturnFalse()
    {
        // Arrange
        var input = "ä"u8.ToArray();

        // Act
        var result = AsciiChar.TryParse(input, default, out var value);

        // Assert
        Assert.False(result);
        Assert.Equal(default, value);
    }

    [Fact]
    public void TryFormat_WithCorrectBuffer_ShouldReturnTrue()
    {
        // Arrange
        var value = (AsciiChar)'a';
        Span<char> buffer = stackalloc char[1];

        // Act
        var result = value.TryFormat(buffer, out var charsWritten, default, default);

        // Assert
        Assert.True(result);
        Assert.Equal(1, charsWritten);
    }

    [Fact]
    public void TryFormat_WithInvalidBuffer_ShouldReturnFalse()
    {
        // Arrange
        var value = (AsciiChar)'a';
        Span<char> buffer = stackalloc char[0];

        // Act
        var result = value.TryFormat(buffer, out var charsWritten, default, default);

        // Assert
        Assert.False(result);
        Assert.Equal(0, charsWritten);
    }
}
