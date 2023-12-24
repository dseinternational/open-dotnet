// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;
using DSE.Open.Text.Json;

namespace DSE.Open.Values.Tests;

public sealed class AlphaCodeTests
{
    [Theory]
    [InlineData("abc", "abc", true)]
    [InlineData("abc", "xyz", false)]
    [InlineData("abc", "ABC", false)]
    public void EqualsTest(string c1, string c2, bool eq)
    {
        var code1 = AlphaCode.Parse(c1, null);
        var code2 = AlphaCode.Parse(c2, null);
        if (eq)
        {
            Assert.Equal(code1, code2);
        }
        else
        {
            Assert.NotEqual(code1, code2);
        }
    }

    [Theory]
    [InlineData("abc")]
    [InlineData("ABC")]
    public void ToStringTest(string code)
    {
        var str = AlphaCode.Parse(code, null).ToString();
        Assert.Equal(code, str);
    }

    [Fact]
    public void Parse_WithCodeLongerThanMaxLength_ShouldThrowFormatException()
    {
        // Arrange
        var code = new string('a', AlphaCode.MaxLength + 1);

        // Assert
        _ = Assert.Throws<FormatException>(() => AlphaCode.Parse(code, null));
    }

    [Fact]
    public void Parse_WithCodeContainingInvalidCharacters_ShouldThrowFormatException()
    {
        // Arrange
        const string code = "abc!";

        // Assert
        _ = Assert.Throws<FormatException>(() => AlphaCode.Parse(code, null));
    }

    [Fact]
    public void Parse_WithNonAsciiChar_ShouldThrowFormatException()
    {
        // Arrange
        const string code = "abc\u00A9";

        // Act
        _ = Assert.Throws<FormatException>(() => AlphaCode.Parse(code.AsSpan(), null));
    }

    [Fact]
    public void TryParse_WithValidCode_ShouldReturnTrue()
    {
        // Arrange
        const string code = "abcde";

        // Act
        var result = AlphaCode.TryParse(code.AsSpan(), null, out var actual);

        // Assert
        Assert.True(result);
        Assert.Equal(code, actual.ToString());
    }

    [Fact]
    public void TryParse_WithNonAsciiLetterChar_ShouldReturnFalse()
    {
        // Arrange
        const string code = "1abcde";

        // Act
        Assert.False(AlphaCode.TryParse(code.AsSpan(), null, out _));
    }

    [Fact]
    public void TryFormat_WithValidCode_ShouldReturnTrue()
    {
        // Arrange
        const string value = "abcdef";
        var code = (AlphaCode)value;
        Span<char> buffer = stackalloc char[value.Length];

        // Act
        var result = code.TryFormat(buffer, out var charsWritten, null, null);

        // Assert
        Assert.True(result);
        Assert.Equal(6, charsWritten);
        Assert.Equal(value, new string(buffer));
    }

    [Fact]
    public void TryFormat_WithInsufficientBuffer_ShouldReturnFalse()
    {
        // Arrange
        const string value = "abcdef";
        var code = (AlphaCode)value;
        Span<char> buffer = stackalloc char[value.Length - 1];

        // Act
        var result = code.TryFormat(buffer, out var charsWritten, null, null);

        // Assert
        Assert.False(result);
        Assert.Equal(0, charsWritten);
    }

    [Fact]
    public void Span_ShouldReturnSpan()
    {
        // Arrange
        const string codeStr = "abcde";
        var code = (AlphaCode)codeStr;

        // Act
        var span = code.ToCharArray().AsSpan();

        // Assert
        Assert.Equal(codeStr, span.ToString());
    }

    [Fact]
    public void TryFormat_WithEmptyCode_throws_UninitializedValueException()
    {
        // Arrange
        var code = (AlphaCode)default;
        var buffer = new char[1];

        // Act
        Assert.Throws<UninitializedValueException<AlphaCode, AsciiString>>(() => code.TryFormat(buffer, out var charsWritten, default, default));
    }

    [Fact]
    public void EqualValuesAreEqual()
    {
        var v1 = AlphaCode.Parse("AA", null);
        var v2 = (AlphaCode)v1.ToString();
        Assert.Equal(v1, v2);
    }

    [Fact]
    public void EqualValuesAsObjectsAreEqual()
    {
        var v1 = (object)AlphaCode.Parse("AA", null);
        var v2 = (object)(AlphaCode)v1.ToString()!;
        Assert.Equal(v1, v2);
    }

    [Fact]
    public void SerializeDeserialize()
    {
        const string value = "abcde";

        var code = AlphaCode.Parse(value, CultureInfo.InvariantCulture);

        var json = JsonSerializer.Serialize(code, JsonSharedOptions.RelaxedJsonEscaping);
        var result = JsonSerializer.Deserialize<AlphaCode>(json, JsonSharedOptions.RelaxedJsonEscaping);

        Assert.Equal(value, result.ToString());
    }

    [Theory]
    [InlineData("\"123\"")]
    [InlineData("\"!abc\"")]
    public void Deserialize_WithInvalidCode_Throws(string code)
    {
        Assert.Throws<FormatException>(() => JsonSerializer.Deserialize<AlphaCode>(code, JsonSharedOptions.RelaxedJsonEscaping));
    }
}
