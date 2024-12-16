// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.


// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open;

namespace DSE.Open;

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
    public void TryParse_returns_false_for_non_ascii_input(string value)
    {
        Assert.False(AsciiString.TryParse(value, out _));
    }

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
        var c = AsciiString.Parse(a, CultureInfo.InvariantCulture).CompareToIgnoreCase(AsciiString.Parse(b, CultureInfo.InvariantCulture));
        Assert.Equal(expected, c);
    }

    [Theory]
    [InlineData("ABCDEFGHIJKLMNOPQRSTUVWXYZ", "ABCDEFGHIJKLMNOPQRSTUVWXYZ")]
    public void ToString_returns_string(string value, string expected)
    {
        var c = AsciiString.Parse(value, CultureInfo.InvariantCulture);
        Assert.Equal(expected, c.ToString());
    }

    [Theory]
    [InlineData("ABCDEFGHIJKLMNOPQRSTUVWXYZ", "abcdefghijklmnopqrstuvwxyz")]
    public void ToStringLower_returns_lower_string(string value, string expected)
    {
        var c = AsciiString.Parse(value, CultureInfo.InvariantCulture);
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
        var result = AsciiString.Parse(chars, CultureInfo.InvariantCulture).ToStringLower();

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
        var result = AsciiString.Parse(chars, CultureInfo.InvariantCulture).ToStringUpper();

        // Assert
        Assert.Equal(length, result.Length);
        Assert.True(result.All(c => c == 'A'));
    }

    [Theory]
    [InlineData("abcdefghijklmnopqrstuvwxyz", "ABCDEFGHIJKLMNOPQRSTUVWXYZ")]
    public void ToStringUpper_returns_upper_string(string value, string expected)
    {
        var c = AsciiString.Parse(value, CultureInfo.InvariantCulture);
        Assert.Equal(expected, c.ToStringUpper());
    }

    [Theory]
    [InlineData("abcdefghijklmnopqrstuvwxyz", "xyz")]
    public void EndsWith(string value, string test)
    {
        var c = AsciiString.Parse(value, CultureInfo.InvariantCulture);
        Assert.True(c.EndsWith(AsciiString.Parse(test, CultureInfo.InvariantCulture)));
    }

    [Fact]
    public void StartsWith_String_WithTooLongValue_ShouldReturnFalse()
    {
        // Arrange
        var value = AsciiString.Parse("a", CultureInfo.InvariantCulture);

        // Act
        var result = value.StartsWith("ab");

        // Assert
        Assert.False(result);
    }

    [Theory]
    [InlineData("abcdefghijklmnopqrstuvwxyz", "abc")]
    public void StartsWith(string value, string test)
    {
        var c = AsciiString.Parse(value, CultureInfo.InvariantCulture);
        Assert.True(c.StartsWith(AsciiString.Parse(test, CultureInfo.InvariantCulture)));
    }

    [Fact]
    public void EndsWith_String_WithTooLongValue_ShouldReturnFalse()
    {
        // Arrange
        var value = AsciiString.Parse("a", CultureInfo.InvariantCulture);

        // Act
        var result = value.EndsWith("ab");

        // Assert
        Assert.False(result);
    }

    [Theory]
    [InlineData("abcdefghijklmnopqrstuvwxyz", "z")]
    [InlineData("abcdefghijklmnopqrstuvwxyz", "vwxyz")]
    [InlineData("abcdefghijklmnopqrstuvwxyz", "bcdefghijklmnopqrstuvwxyz")]
    [InlineData("abcdefghijklmnopqrstuvwxyz", "")]
    [InlineData("abcdefghijklmnopqrstuvwxyz", "abcdefghijklmnopqrstuvwxyz")]
    public void EndsWith_string(string value, string test)
    {
        var c = AsciiString.Parse(value, CultureInfo.InvariantCulture);
        Assert.True(c.EndsWith(test));
    }

    [Theory]
    [InlineData("abcdefghijklmnopqrstuvwxyz", "abc")]
    [InlineData("abcdefghijklmnopqrstuvwxyz", "")]
    [InlineData("abcdefghijklmnopqrstuvwxyz", "abcdefghijklmnopqrstuvwxyz")]
    public void StartsWith_string(string value, string test)
    {
        var c = AsciiString.Parse(value, CultureInfo.InvariantCulture);
        Assert.True(c.StartsWith(test));
    }

    [Theory]
    [InlineData("abcdefghijklmnopqrstuvwxyz")]
    public void Indexer_returns_indexed_asciichar(string value)
    {
        var c = AsciiString.Parse(value, CultureInfo.InvariantCulture);

        for (var i = 0; i < value.Length; i++)
        {
            Assert.Equal(value[i], c[i]);
        }
    }

    [Theory]
    [InlineData("abcdefghijklmnopqrstuvwxyz")]
    public void GetEnumerator_returns_enumerator_that_enumerates(string value)
    {
        var c = AsciiString.Parse(value, CultureInfo.InvariantCulture);

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
        var asciiString = AsciiString.Parse(value, CultureInfo.InvariantCulture);

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
        var asciiString = AsciiString.Parse(value, CultureInfo.InvariantCulture);

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
        var asciiString1 = AsciiString.Parse(value, CultureInfo.InvariantCulture);
        var asciiString2 = AsciiString.Parse(value, CultureInfo.InvariantCulture);

        // Assert
        Assert.Equal(asciiString1, asciiString2);
    }

    [Fact]
    public void GetHashCodeReturnsSameValueForEqualValues()
    {
        // Arrange
        const string value = "abcdefghijklmnopqrstuvwxyz";
        var asciiString1 = AsciiString.Parse(value, CultureInfo.InvariantCulture);
        var asciiString2 = AsciiString.Parse(value, CultureInfo.InvariantCulture);

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
        var result = AsciiString.TryParse(bytes, default, out _);

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

    [Theory]
    [InlineData("abcdefghijklmnopqrstuvwxyz", "abcdefghijklmnopqrstuvwxyz")]
    [InlineData("zyxwvutsrqponmlkjihgfedcba", "a")]
    [InlineData("a", "")]
    public void Contains_True(string h, string n)
    {
        var haystack = AsciiString.Parse(h, CultureInfo.InvariantCulture);
        var needle = AsciiString.Parse(n, CultureInfo.InvariantCulture);
        Assert.True(haystack.Contains(needle));
    }

    [Theory]
    [InlineData("abcdefghijklmnopqrstuvwxyz", " ")]
    [InlineData("abcdefghijklmnopqrstuvwxyz", "a ")]
    [InlineData("abcdefghijklmnopqrstuvwxyz", "zyxwvutsrqponmlkjihgfedcb")]
    [InlineData("abcdefghijklmnopqrstuvwxyz", "abcdefghijklmnopqrstuvwxyz ")]
    [InlineData("abcdefghijklmnopqrstuvwxyz", " abcdefghijklmnopqrstuvwxyz")]
    [InlineData("z", "abcdefghijklmnopqrstuvwxy")]
    [InlineData("", "a")]
    public void Contains_False(string h, string n)
    {
        // Arrange
        var haystack = AsciiString.Parse(h, CultureInfo.InvariantCulture);
        var needle = AsciiString.Parse(n, CultureInfo.InvariantCulture);

        // Assert
        Assert.False(haystack.Contains(needle));
    }

    [Fact]
    public void LastIndexOf_ShouldReturnLastIndex()
    {
        // Arrange
        var value = AsciiString.Parse("abcdefghijklmnopqrstuvwxyza", CultureInfo.InvariantCulture);
        var needle = AsciiChar.FromChar('a');

        // Act
        var result = value.LastIndexOf(needle);

        // Assert
        Assert.Equal(value.Length - 1, result);
    }

    [Fact]
    public void IndexOf_ShouldReturnFirstIndex()
    {
        // Arrange
        var value = AsciiString.Parse("abcdefghijklmnopqrstuvwxyza", CultureInfo.InvariantCulture);
        var needle = AsciiChar.FromChar('a');

        // Act
        var result = value.IndexOf(needle);

        // Assert
        Assert.Equal(0, result);
    }

    [Fact]
    public void GetRepeatableHashCode_ReturnsExpectedValue()
    {
        var value = AsciiString.Parse("abcdefghijklmnopqrstuvwxyza", CultureInfo.InvariantCulture);
        Assert.Equal(10941012414329798000u, value.GetRepeatableHashCode());
    }
}
