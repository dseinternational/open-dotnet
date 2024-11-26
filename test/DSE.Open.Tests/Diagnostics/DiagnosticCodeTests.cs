// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.


// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Diagnostics;

public sealed class DiagnosticCodeTests
{
    public static TheoryData<string> ValidCodes { get; } = new()
    {
        "ABC123456",
        "ABC1234567",
        "ABC12345678",
        "ABCD123456",
        "ABCD1234567",
        "ABCD12345678",
        "ZZZ000000",
        "XYX1111111",
        "WWWW11111111",
    };

    public static TheoryData<string> InvalidCodes { get; } = new()
    {
        "AB123456",
        "AB1234567",
        "AB12345678",
        "ABC12345",
        "ABCD12345",
        "ABCD123456789",
        "zzz000000",
        "xxx1111111",
        "WWWW11111!1",
    };

    [Theory]
    [MemberData(nameof(ValidCodes))]
    public void InitialiseWithValidCode(string code)
    {
        var diagnosticCode = new DiagnosticCode(code);
        Assert.Equal(code, diagnosticCode.ToString());
    }

    [Theory]
    [InlineData("WWWW11111111", "WWWW11111111", true)]
    [InlineData("ABC1234567", "ABC1234567", true)]
    [InlineData("ABCD12345678", "ABCD1234567", false)]
    [InlineData("ABCD12345678", "ABCD12345679", false)]
    [InlineData("XBCD12345678", "ABCD12345678", false)]
    public void EqualsTest(string c1, string c2, bool eq)
    {
        var code1 = new DiagnosticCode(c1);
        var code2 = new DiagnosticCode(c2);
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
    [InlineData("WWWW11111111", "WWWW11111111", true)]
    [InlineData("ABC1234567", "ABC1234567", true)]
    [InlineData("ABCD12345678", "ABCD1234567", false)]
    [InlineData("ABCD12345678", "ABCD12345679", false)]
    [InlineData("XBCD12345678", "ABCD12345678", false)]
    public void GetHashCodeTest(string c1, string c2, bool eq)
    {
        var code1 = new DiagnosticCode(c1).GetHashCode();
        var code2 = new DiagnosticCode(c2).GetHashCode();
        if (eq)
        {
            Assert.Equal(code1, code2);
        }
        else
        {
            Assert.NotEqual(code1, code2);
        }
    }

    [Fact]
    public void EmptyTest()
    {
        Assert.Equal(DiagnosticCode.Empty, new());
    }

    [Fact]
    public void EqualsStringOperatorTest()
    {
        Assert.True(DiagnosticCode.Empty == string.Empty);
        Assert.True(new DiagnosticCode("ABCD12345678") == "ABCD12345678");
        Assert.False((DiagnosticCode)"ABCD12345678" == "ABD12345678");
    }

    [Fact]
    public void NotEqualsStringOperatorTest()
    {
        Assert.False(DiagnosticCode.Empty != string.Empty);
        Assert.False(new DiagnosticCode("ABCD12345678") != "ABCD12345678");
        Assert.True(new DiagnosticCode("ABCD12345678") != "ABCD45678");
    }

    [Fact]
    public void New_WithCodeLongerThanMaxLength_ShouldThrowArgumentException()
    {
        // Arrange
        var code = "ABCD123456789";

        // Assert
        _ = Assert.Throws<ArgumentOutOfRangeException>(() => new DiagnosticCode(code));
    }

    [Theory]
    [MemberData(nameof(InvalidCodes))]
    public void New_WithInvalidCodes_ShouldThrowArgumentException(string code)
    {
        // Assert
        _ = Assert.Throws<ArgumentOutOfRangeException>(() => new DiagnosticCode(code));
    }

    [Fact]
    public void Parse_WithNonAsciiChar_ShouldThrowFormatException()
    {
        // Arrange
        const string code = "ABCD12345\u00A9";

        // Act
        _ = Assert.Throws<FormatException>(() => DiagnosticCode.Parse(code.AsSpan(), null));
    }

    [Theory]
    [MemberData(nameof(ValidCodes))]
    public void TryParse_WithValidCode_ShouldReturnTrue(string code)
    {
        // Act
        var result = DiagnosticCode.TryParse(code.AsSpan(), null, out var alphaNumericCode);

        // Assert
        Assert.True(result);
        Assert.Equal(code, alphaNumericCode.ToString());
    }

    [Fact]
    public void TryParse_WithNonAsciiChar_ShouldReturnFalse()
    {
        // Arrange
        const string code = "ABCD12345\u00A9";

        // Act
        Assert.False(DiagnosticCode.TryParse(code.AsSpan(), null, out _));
    }

    [Theory]
    [MemberData(nameof(ValidCodes))]
    public void TryFormat_WithValidCode_ShouldReturnTrue(string codeStr)
    {
        // Arrange
        var code = new DiagnosticCode(codeStr);
        Span<char> buffer = stackalloc char[DiagnosticCode.MaxLength];

        // Act
        var result = code.TryFormat(buffer, out var charsWritten, null, null);

        // Assert
        Assert.True(result);
        Assert.Equal(codeStr.Length, charsWritten);
        Assert.Equal(codeStr, buffer[..charsWritten].ToString());
    }

    [Fact]
    public void TryFormat_WithInsufficientBuffer_ShouldReturnFalse()
    {
        // Arrange
        var code = new DiagnosticCode("ABC123456");
        Span<char> buffer = stackalloc char[8];

        // Act
        var result = code.TryFormat(buffer, out var charsWritten, null, null);

        // Assert
        Assert.False(result);
        Assert.Equal(0, charsWritten);
    }

    [Theory]
    [MemberData(nameof(ValidCodes))]
    public void Span_ShouldReturnSpan(string codeStr)
    {
        // Arrange
        var code = new DiagnosticCode(codeStr);

        // Act
        var span = code.AsSpan();

        // Assert
        Assert.Equal(codeStr, span.ToString());
    }

    [Fact]
    public void TryFormat_WithEmptyCode_ShouldReturnTrueWithNothingWritten()
    {
        // Arrange
        var code = DiagnosticCode.Empty;
        var buffer = new char[1];

        // Act
        var success = code.TryFormat(buffer, out var charsWritten, default, default);

        // Assert
        Assert.True(success);
        Assert.Equal(0, charsWritten);
    }

    [Theory]
    [MemberData(nameof(ValidCodes))]
    public void EqualValuesAreEqual(string code)
    {
        var v1 = DiagnosticCode.Parse(code, null);
        var v2 = new DiagnosticCode(v1.ToString());
        Assert.Equal(v1, v2);
    }

    [Theory]
    [MemberData(nameof(ValidCodes))]
    public void EqualValuesAsObjectsAreEqual(string code)
    {
        var v1 = (object)DiagnosticCode.Parse(code, null);
        var v2 = (object)new DiagnosticCode(v1.ToString()!);
        Assert.Equal(v1, v2);
    }
}
