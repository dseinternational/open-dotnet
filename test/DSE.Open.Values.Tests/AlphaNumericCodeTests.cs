// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Values.Tests;

public sealed class AlphaNumericCodeTests
{
    [Theory]
    [InlineData("abc", "abc", true)]
    [InlineData("abc5497", "abc5497", true)]
    [InlineData("abc", "xyz", false)]
    [InlineData("abc", "ABC", false)]
    [InlineData("456abc", "456ABC", false)]
    public void EqualsTest(string c1, string c2, bool eq)
    {
        var code1 = new AlphaNumericCode(c1);
        var code2 = new AlphaNumericCode(c2);
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
    [InlineData("A123456789BC")]
    public void ToStringTest(string code)
    {
        var str = new AlphaNumericCode(code).ToString();
        Assert.Equal(code, str);
    }

    [Fact]
    public void EmptyTest() => Assert.Equal(default, new AlphaNumericCode());

    [Fact]
    public void EqualsStringOperatorTest()
    {
        Assert.True(((AlphaNumericCode)default) == string.Empty);
        Assert.True(new AlphaNumericCode("abC12") == "abC12");
        Assert.False((AlphaNumericCode)"abC33" == "abc33");
    }

    [Fact]
    public void NotEqualsStringOperatorTest()
    {
        Assert.False((AlphaNumericCode)default != string.Empty);
        Assert.False(new AlphaNumericCode("abC565") != "abC565");
        Assert.True(new AlphaNumericCode("abC565") != "abc565");
    }

    [Fact]
    public void New_WithCodeLongerThanMaxLength_ShouldThrowArgumentException()
    {
        // Arrange
        var code = new string('a', AlphaNumericCode.MaxLength + 1);

        // Assert
        _ = Assert.Throws<ArgumentOutOfRangeException>(() => new AlphaNumericCode(code));
    }

    [Fact]
    public void New_WithCodeContainingInvalidCharacters_ShouldThrowArgumentException()
    {
        // Arrange
        const string code = "abc123!";

        // Assert
        _ = Assert.Throws<ArgumentOutOfRangeException>(() => new AlphaNumericCode(code));
    }

    [Fact]
    public void Parse_WithNonAsciiChar_ShouldThrowFormatException()
    {
        // Arrange
        const string code = "abc123\u00A9";

        // Act
        _ = Assert.Throws<FormatException>(() => AlphaNumericCode.Parse(code.AsSpan(), null));
    }

    [Fact]
    public void TryParse_WithValidCode_ShouldReturnTrue()
    {
        // Arrange
        const string code = "abc123";

        // Act
        var result = AlphaNumericCode.TryParse(code.AsSpan(), null, out var alphaNumericCode);

        // Assert
        Assert.True(result);
        Assert.Equal(code, alphaNumericCode.ToString());
    }

    [Fact]
    public void TryParse_WithNonAsciiChar_ShouldReturnFalse()
    {
        // Arrange
        const string code = "abc123\u00A9";

        // Act
        Assert.False(AlphaNumericCode.TryParse(code.AsSpan(), null, out _));
    }

    [Fact]
    public void TryFormat_WithValidCode_ShouldReturnTrue()
    {
        // Arrange
        var code = new AlphaNumericCode("abc123");
        Span<char> buffer = stackalloc char[6];

        // Act
        var result = code.TryFormat(buffer, out var charsWritten, null, null);

        // Assert
        Assert.True(result);
        Assert.Equal(6, charsWritten);
        Assert.Equal("abc123", new string(buffer));
    }

    [Fact]
    public void TryFormat_WithInsufficientBuffer_ShouldReturnFalse()
    {
        // Arrange
        var code = new AlphaNumericCode("abc123");
        Span<char> buffer = stackalloc char[5];

        // Act
        var result = code.TryFormat(buffer, out var charsWritten, null, null);

        // Assert
        Assert.False(result);
        Assert.Equal(0, charsWritten);
    }

    [Fact]
    public void TryFormat_WithEmptyCode_Should_throw()
    {
        // Arrange
        var code = (AlphaNumericCode)default;
        var buffer = new char[1];

        _ = Assert.Throws<UninitializedValueException<AlphaNumericCode, AsciiString>>(
            () => code.TryFormat(buffer, out var charsWritten, default, default));
    }

    [Fact]
    public void EqualValuesAreEqual()
    {
        var v1 = AlphaNumericCode.Parse("AA", null);
        var v2 = new AlphaNumericCode(v1.ToString());
        Assert.Equal(v1, v2);
    }

    [Fact]
    public void EqualValuesAsObjectsAreEqual()
    {
        var v1 = (object)AlphaNumericCode.Parse("AA", null);
        var v2 = (object)new AlphaNumericCode(v1.ToString()!);
        Assert.Equal(v1, v2);
    }
}
