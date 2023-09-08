// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Values.Tests;

public sealed class CodeTests
{
    [Theory]
    [InlineData("abc", "abc", true)]
    [InlineData("abc", "xyz", false)]
    [InlineData("abc", "ABC", false)]
    [InlineData(".", ".", true)]
    [InlineData("", "", true)]
    public void EqualsTest(string c1, string c2, bool eq)
    {
        var code1 = new Code(c1);
        var code2 = new Code(c2);
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
    [InlineData("abc", "abc", true)]
    [InlineData("abc", "xyz", false)]
    [InlineData("abc", "ABC", false)]
    [InlineData(".", ".", true)]
    [InlineData("", "", true)]
    public void GetHashCodeReturnsEqualValues(string c1, string c2, bool eq)
    {
        var code1 = new Code(c1);
        var code2 = new Code(c2);
        if (eq)
        {
            Assert.Equal(code1.GetHashCode(), code2.GetHashCode());
        }
        else
        {
            Assert.NotEqual(code1.GetHashCode(), code2.GetHashCode());
        }
    }

    [Theory]
    [InlineData("abc")]
    [InlineData("ABC")]
    [InlineData("123")]
    [InlineData("x.y:Z")]
    [InlineData("")]
    public void ToStringTest(string code)
    {
        var str = new Code(code).ToString();
        Assert.Equal(code, str);
    }

    [Fact]
    public void EmptyTest() => Assert.Equal(Code.Empty, new Code());

    [Fact]
    public void CompareTo_less_than()
    {
        var v1 = new Code("AAAAAA");
        var v2 = new Code("AAAAAB");
        Assert.True(v1.CompareTo(v2, StringComparison.Ordinal) < 0);
    }

    [Fact]
    public void CompareTo_greater_than()
    {
        var v1 = new Code("AAAAAB");
        var v2 = new Code("AAAAAA");
        Assert.True(v1.CompareTo(v2, StringComparison.Ordinal) > 0);
    }

    [Fact]
    public void CompareTo_equal()
    {
        var v1 = new Code("AAAAAB");
        var v2 = new Code("AAAAAB");
        Assert.True(v1.CompareTo(v2, StringComparison.Ordinal) == 0);
    }

    [Fact]
    public void Parse_WithEmptySpan_ShouldReturnDefault()
    {
        var a = Code.Parse(Span<char>.Empty, null);
        Assert.Equal(Code.Empty, a);
    }

    [Fact]
    public void Parse_WithEmptyString_ShouldReturnDefault()
    {
        var a = Code.Parse(string.Empty, null);
        Assert.Equal(Code.Empty, a);
    }

    [Fact]
    public void Parse_WithNullString_ShouldThrowArgumentNull() => Assert.Throws<ArgumentNullException>(() => Code.Parse(null!, null));

    [Fact]
    public void TryParse_WithEmptySpan_ShouldReturnTrueAndDefaultResult()
    {
        var success = Code.TryParse(Span<char>.Empty, null, out var result);
        Assert.True(success);
        Assert.Equal(Code.Empty, result);
    }

    [Fact]
    public void TryParse_WithNullString_ShouldReturnFalseAndDefaultResult()
    {
        var success = Code.TryParse(null, null, out var result);
        Assert.False(success);
        Assert.Equal(Code.Empty, result);
    }

    [Fact]
    public void TryParse_WithEmptyString_ShouldReturnTrueAndDefaultResult()
    {
        var success = Code.TryParse(string.Empty, null, out var result);
        Assert.True(success);
        Assert.Equal(Code.Empty, result);
    }

    [Fact]
    public void TryFormat_WithEmptyCode_ShouldReturnTrueWithNothingWritten()
    {
        // Arrange
        var code = Code.Empty;
        var buffer = new char[1];

        // Act
        var success = code.TryFormat(buffer, out var charsWritten, default, default);

        // Assert
        Assert.True(success);
        Assert.Equal(0, charsWritten);
    }

    [Fact]
    public void EqualValuesAreEqual()
    {
        var v1 = Code.Parse("AAAAAB", null);
        var v2 = Code.Parse(v1.ToString(), null);
        Assert.Equal(v1, v2);
    }

    [Fact]
    public void EqualValuesAsObjectsAreEqual()
    {
        var v1 = (object)Code.Parse("AAAAAB", null);
        var v2 = (object)Code.Parse(v1.ToString()!, null);
        Assert.Equal(v1, v2);
    }

    [Fact]
    public void New_WithInvalidCode_ShouldThrowFormatException()
    {
        // Arrange
        const string code = "123456789012345678901234567890121";

        // Act
        static void New()
        {
            _ = new Code(code);
        }

        // Assert
        _ = Assert.Throws<FormatException>(New);
    }

    [Fact]
    public void FromInt_WithValidCode_ShouldReturnCode()
    {
        // Arrange
        const int code = 1234567890;

        // Act
        var result = (Code)code;

        // Assert
        Assert.Equal(code.ToString(), result.ToString());
    }

    [Fact]
    public void ToString_WithEmptyCode_ShouldReturnEmptyString()
    {
        // Arrange
        var code = Code.Empty;

        // Act
        var result = code.ToString();

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void TryFormat_WithBufferTooSmall_ShouldReturnFalse()
    {
        // Arrange
        var code = Code.Parse("123", null);
        var buffer = Span<char>.Empty;

        // Act
        var success = code.TryFormat(buffer, out var charsWritten, default, default);

        // Assert
        Assert.False(success);
        Assert.Equal(0, charsWritten);
    }
}
