// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;
using DSE.Open.Text.Json;

namespace DSE.Open.Values.Web;

public sealed class EtagTests
{
    [Theory]
    [InlineData("abc", "abc", true)]
    [InlineData("abc", "xyz", false)]
    [InlineData("abc", "ABC", false)]
    public void EqualsTest(string c1, string c2, bool eq)
    {
        var code1 = Etag.Parse(c1, null);
        var code2 = Etag.Parse(c2, null);
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
        var str = Etag.Parse(code, null).ToString();
        Assert.Equal(code, str);
    }

    [Fact]
    public void Parse_WithCodeLongerThanMaxLength_ShouldThrowFormatException()
    {
        // Arrange
        var code = new string('a', Etag.MaxLength + 1);

        // Assert
        _ = Assert.Throws<FormatException>(() => Etag.Parse(code, null));
    }

    [Fact]
    public void Parse_WithCodeContainingInvalidCharacters_ShouldThrowFormatException()
    {
        // Arrange
        const string code = "⅓⅔⅛⅜ΩꜲ";

        // Assert
        _ = Assert.Throws<FormatException>(() => Etag.Parse(code, null));
    }

    [Fact]
    public void Parse_WithNonAsciiChar_ShouldThrowFormatException()
    {
        // Arrange
        const string code = "abc\u00A9";

        // Act
        _ = Assert.Throws<FormatException>(() => Etag.Parse(code.AsSpan(), null));
    }

    [Fact]
    public void TryParse_WithValidCode_ShouldReturnTrue()
    {
        // Arrange
        const string code = "abcde";

        // Act
        var result = Etag.TryParse(code.AsSpan(), null, out var actual);

        // Assert
        Assert.True(result);
        Assert.Equal(code, actual.ToString());
    }

    [Fact]
    public void TryParse_WithNonAsciiChar_ShouldReturnFalse()
    {
        // Arrange
        const string code = "something Ꜳ";

        // Act
        Assert.False(Etag.TryParse(code.AsSpan(), null, out _));
    }

    [Fact]
    public void TryFormat_WithValidCode_ShouldReturnTrue()
    {
        // Arrange
        const string value = "abcdef";
        var code = (Etag)value;
        Span<char> buffer = stackalloc char[value.Length];

        // Act
        var result = code.TryFormat(buffer, out var charsWritten, null, null);

        // Assert
        Assert.True(result);
        Assert.Equal(6, charsWritten);
        Assert.Equal(value, new(buffer));
    }

    [Fact]
    public void TryFormat_WithInsufficientBuffer_ShouldReturnFalse()
    {
        // Arrange
        const string value = "abcdef";
        var code = (Etag)value;
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
        var code = (Etag)codeStr;

        // Act
        var span = code.ToCharArray().AsSpan();

        // Assert
        Assert.Equal(codeStr, span.ToString());
    }

    [Fact]
    public void EqualValuesAreEqual()
    {
        var v1 = Etag.Parse("AA", null);
        var v2 = (Etag)v1.ToString();
        Assert.Equal(v1, v2);
    }

    [Fact]
    public void EqualValuesAsObjectsAreEqual()
    {
        var v1 = (object)Etag.Parse("AA", null);
        var v2 = (object)(Etag)v1.ToString()!;
        Assert.Equal(v1, v2);
    }

    [Fact]
    public void SerializeDeserialize()
    {
        const string value = "abcde";

        var code = Etag.Parse(value, CultureInfo.InvariantCulture);

        var json = JsonSerializer.Serialize(code, JsonSharedOptions.RelaxedJsonEscaping);
        var result = JsonSerializer.Deserialize<Etag>(json, JsonSharedOptions.RelaxedJsonEscaping);

        Assert.Equal(value, result.ToString());
    }

    [Theory]
    [InlineData("\"Ễẞ‰\"")]
    [InlineData("\"⅓⅔⅛⅜ΩꜲ\"")]
    public void Deserialize_WithInvalidCode_Throws(string code)
    {
        _ = Assert.Throws<FormatException>(() => JsonSerializer.Deserialize<Etag>(code, JsonSharedOptions.RelaxedJsonEscaping));
    }

    [Theory]
    [InlineData("\"abcdefg123456\"", "abcdefg123456")]
    [InlineData("\"abcde\\\\fg123456\"", "abcde\\fg123456")]
    [InlineData("\"\\\"6a004564-0000-1100-0000-68e77d630000\\\"\"", "\"6a004564-0000-1100-0000-68e77d630000\"")]
    [InlineData("\"\\\\\\\"6a004564-0000-1100-0000-68e77d630000\\\\\\\"\"", "\\\"6a004564-0000-1100-0000-68e77d630000\\\"")]
    public void Deserialize_Json(string json, string expected)
    {
        var deserialized = JsonSerializer.Deserialize<Etag>(json);
        Assert.Equal(expected, deserialized.ToString());
    }
}
