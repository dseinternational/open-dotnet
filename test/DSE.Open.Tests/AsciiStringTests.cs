// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Tests;

public class AsciiStringTests
{
    [Theory]
    [MemberData(nameof(AsciiTestData.ValidAsciiCharSequenceStrings), MemberType = typeof(AsciiTestData))]
    public void Cast_from_valid_value(string value)
    {
        var v = (AsciiString)value;
        Assert.Equal(value, v.ToString());
    }

    [Theory]
    [InlineData("£")]
    [InlineData("•")]
    [InlineData("abcdedfgsdjnhgfdlui34987trglih¦")]
    public void TryParse_returns_false_for_non_ascii_input(string value) => Assert.False(AsciiString.TryParse(value, out _));

    [Theory]
    [InlineData("a", "A", 0)]
    [InlineData("abcdEFG", "ABCdefg", 0)]
    [InlineData("abcdEFGb", "ABCdefg", 1)]
    [InlineData("100000", "100000", 0)]
    [InlineData("100000", "1000000", -1)]
    [InlineData("100000", "10000000", -2)]
    [InlineData("1000000", "100000", 1)]
    [InlineData("A", "a", 0)]
    [InlineData("A", "A", 0)]
    [InlineData("a", "a", 0)]
    [InlineData("a", "B", -1)]
    [InlineData("X", "B", 1)]
    [InlineData(":", ":", 0)]
    [InlineData("\t", "\t", 0)]
    public void Compare_case_insensitive(string a, string b, int expected)
    {
        var c = AsciiString.Parse(a).CompareToCaseInsensitive(AsciiString.Parse(b));
        Assert.Equal(expected, c);
    }

    [Theory]
    [InlineData("ABCDEFGHIJKLMNOPQRSTUVWXYZ", "ABCDEFGHIJKLMNOPQRSTUVWXYZ")]
    public void ToString_returns_string(string value, string expected)
    {
        var c = AsciiString.Parse(value);
        Assert.Equal(expected, c.ToString());
    }

    [Theory]
    [InlineData("ABCDEFGHIJKLMNOPQRSTUVWXYZ", "abcdefghijklmnopqrstuvwxyz")]
    public void ToStringLower_returns_lower_string(string value, string expected)
    {
        var c = AsciiString.Parse(value);
        Assert.Equal(expected, c.ToStringLower());
    }

    [Theory]
    [InlineData(10)]
    [InlineData(100)]
    [InlineData(541)]
    [InlineData(1000)]
    [InlineData(3456)]
    public void ToStringLower_ShouldReturnCorrectString(int length)
    {
        // Arrange
        var chars = new char[length];
        chars.AsSpan().Fill('A');

        // Act
        var result = AsciiString.Parse(chars).ToStringLower();

        // Assert
        Assert.Equal(length, result.Length);
        Assert.True(result.All(c => c == 'a'));
    }

    [Theory]
    [InlineData(100)]
    [InlineData(541)]
    [InlineData(1000)]
    [InlineData(3456)]
    public void ToStringUpper_ShouldReturnCorrectString(int length)
    {
        // Arrange
        var chars = new char[length];
        chars.AsSpan().Fill('a');

        // Act
        var result = AsciiString.Parse(chars).ToStringUpper();

        // Assert
        Assert.Equal(length, result.Length);
        Assert.True(result.All(c => c == 'A'));
    }


    [Theory]
    [InlineData("abcdefghijklmnopqrstuvwxyz", "ABCDEFGHIJKLMNOPQRSTUVWXYZ")]
    public void ToStringUpper_returns_upper_string(string value, string expected)
    {
        var c = AsciiString.Parse(value);
        Assert.Equal(expected, c.ToStringUpper());
    }

    [Theory]
    [InlineData("abcdefghijklmnopqrstuvwxyz", "xyz")]
    public void EndsWith(string value, string test)
    {
        var c = AsciiString.Parse(value);
        Assert.True(c.EndsWith(AsciiString.Parse(test)));
    }

    [Theory]
    [InlineData("abcdefghijklmnopqrstuvwxyz", "abc")]
    public void StartsWith(string value, string test)
    {
        var c = AsciiString.Parse(value);
        Assert.True(c.StartsWith(AsciiString.Parse(test)));
    }

    [Theory]
    [InlineData("abcdefghijklmnopqrstuvwxyz", "z")]
    [InlineData("abcdefghijklmnopqrstuvwxyz", "vwxyz")]
    [InlineData("abcdefghijklmnopqrstuvwxyz", "bcdefghijklmnopqrstuvwxyz")]
    [InlineData("abcdefghijklmnopqrstuvwxyz", "")]
    [InlineData("abcdefghijklmnopqrstuvwxyz", "abcdefghijklmnopqrstuvwxyz")]
    public void EndsWith_string(string value, string test)
    {
        var c = AsciiString.Parse(value);
        Assert.True(c.EndsWith(test));
    }

    [Theory]
    [InlineData("abcdefghijklmnopqrstuvwxyz", "abc")]
    [InlineData("abcdefghijklmnopqrstuvwxyz", "")]
    [InlineData("abcdefghijklmnopqrstuvwxyz", "abcdefghijklmnopqrstuvwxyz")]
    public void StartsWith_string(string value, string test)
    {
        var c = AsciiString.Parse(value);
        Assert.True(c.StartsWith(test));
    }

    [Theory]
    [InlineData("abcdefghijklmnopqrstuvwxyz")]
    public void Indexer_returns_indexed_asciichar(string value)
    {
        var c = AsciiString.Parse(value);

        for (var i = 0; i < value.Length; i++)
        {
            Assert.Equal(value[i], c[i]);
        }
    }

    [Theory]
    [InlineData("abcdefghijklmnopqrstuvwxyz")]
    public void GetEnumerator_returns_enumerator_that_enumerates(string value)
    {
        var c = AsciiString.Parse(value);

        var i = 0;

        foreach (var item in c)
        {
            Assert.Equal(value[i], item);
            i++;
        }
    }

    [Fact]
    public void ToCharArray_ShouldReturnCorrectArray()
    {
        // Arrange
        const string value = "abcdefghijklmnopqrstuvwxyz";
        var asciiString = AsciiString.Parse(value);

        // Act
        var result = asciiString.ToCharArray();

        // Assert
        Assert.Equal(value.ToCharArray(), result);
    }

    [Fact]
    public void ToCharSequence_ShouldReturnCorrectSequence()
    {
        // Arrange
        const string value = "abcdefghijklmnopqrstuvwxyz";
        var asciiString = AsciiString.Parse(value);

        // Act
        var result = asciiString.ToCharSequence();

        // Assert
        Assert.Equal(value.ToCharArray(), result.Span.ToArray());
    }

    [Fact]
    public void EqualsReturnsTrueForEqualValues()
    {
        // Arrange
        const string value = "abcdefghijklmnopqrstuvwxyz";
        var asciiString1 = AsciiString.Parse(value);
        var asciiString2 = AsciiString.Parse(value);

        // Assert
        Assert.Equal(asciiString1, asciiString2);
    }

    [Fact]
    public void GetHashCodeReturnsSameValueForEqualValues()
    {
        // Arrange
        const string value = "abcdefghijklmnopqrstuvwxyz";
        var asciiString1 = AsciiString.Parse(value);
        var asciiString2 = AsciiString.Parse(value);

        // Act
        var hashCode1 = asciiString1.GetHashCode();
        var hashCode2 = asciiString2.GetHashCode();

        // Assert
        Assert.Equal(hashCode1, hashCode2);
    }

    [Fact]
    public void TryParse_Utf8_WithValidBytes_ShouldReturnTrue()
    {
        // Arrange
        var bytes = "abcdefghijklmnopqrstuvwxyz"u8;

        // Act
        var result = AsciiString.TryParse(bytes, default, out var value);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void TryFormat_Utf8_WithCorrectBuffer_ShouldReturnTrue()
    {
        // Arrange
        var bytes = "abcdefghijklmnopqrstuvwxyz"u8;
        var value = AsciiString.Parse(bytes, null);
        Span<byte> buffer = stackalloc byte[bytes.Length];

        // Act
        var success = value.TryFormat(buffer, out var bytesWritten, default, default);

        // Assert
        Assert.True(success);
        Assert.Equal(bytes.Length, bytesWritten);
        Assert.True(buffer.SequenceEqual(bytes));
    }

    [Fact]
    public void TryFormat_Utf8_WithIncorrectBuffer_ShouldReturnFalse()
    {
        // Arrange
        var bytes = "abcdefghijklmnopqrstuvwxyz"u8;
        var value = AsciiString.Parse(bytes, null);
        Span<byte> buffer = stackalloc byte[bytes.Length - 1];

        // Act
        var success = value.TryFormat(buffer, out var bytesWritten, default, default);

        // Assert
        Assert.False(success);
        Assert.Equal(0, bytesWritten);
    }

    [Fact]
    public void TryParse_WithLongInput_ShouldCorrectlyParse()
    {
        // Arrange
        var chars = new char[1000];
        chars.AsSpan().Fill('a');

        // Act
        var result = AsciiString.TryParse(chars, default, out var value);

        // Assert
        Assert.True(result);
        Assert.Equal(1000, value.Length);
    }
}
