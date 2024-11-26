// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.


// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Security;

public class SecureTokenTests
{
    public ITestOutputHelper Output { get; }

    public SecureTokenTests(ITestOutputHelper output)
    {
        Output = output;
    }

    [Fact]
    public void Generate()
    {
        for (var i = 0; i < 50; i++)
        {
            var token = SecureToken.New();
            Output.WriteLine(token.ToString());
        }
    }

    [Fact]
    public void Parse_valid_tokens()
    {
        for (var i = 0; i < 50; i++)
        {
            var token = SecureToken.New();
            var token2 = SecureToken.Parse(token.ToString(), CultureInfo.InvariantCulture);
            Assert.Equal(token, token2);
        }
    }

    [Fact]
    public void HashCodesEqualForEqualCode()
    {
        for (var i = 0; i < 50; i++)
        {
            var token = SecureToken.New();
            Assert.Equal(token.GetHashCode(), SecureToken.Parse(token.ToString(), CultureInfo.InvariantCulture).GetHashCode());
        }
    }

    [Fact]
    public void IsValidTokenChar()
    {
        foreach (var c in SecureToken.TokenChars.ToArray())
        {
            Assert.True(SecureToken.IsValidTokenChar(c));
        }
    }

    [Fact]
    public void Parse_WithEmptySpan_ShouldReturnDefault()
    {
        var a = SecureToken.Parse([], CultureInfo.InvariantCulture);
        Assert.Equal(SecureToken.Empty, a);
    }

    [Fact]
    public void Parse_WithEmptyString_ReturnsDefault()
    {
        var a = SecureToken.Parse(string.Empty, CultureInfo.InvariantCulture);
        Assert.Equal(SecureToken.Empty, a);
    }

    [Fact]
    public void Parse_WithNullString_ShouldThrowArgumentNull()
    {
        _ = Assert.Throws<ArgumentNullException>(() => SecureToken.Parse(null!, CultureInfo.InvariantCulture));
    }

    [Fact]
    public void TryParse_WithEmptySpan_ShouldReturnTrueAndDefaultResult()
    {
        var success = SecureToken.TryParse([], CultureInfo.InvariantCulture, out var result);
        Assert.True(success);
        Assert.Equal(SecureToken.Empty, result);
    }

    [Fact]
    public void TryParse_WithNullString_ShouldReturnFalseAndDefaultResult()
    {
        var success = SecureToken.TryParse(null, out var result);
        Assert.False(success);
        Assert.Equal(SecureToken.Empty, result);
    }

    [Fact]
    public void TryParse_WithEmptyString_ShouldReturnTrueAndDefaultResult()
    {
        var success = SecureToken.TryParse(string.Empty, out var result);
        Assert.True(success);
        Assert.Equal(SecureToken.Empty, result);
    }

    [Fact]
    public void TryFormat_WithEmptyCode_ShouldReturnTrueWithNothingWritten()
    {
        // Arrange
        var code = SecureToken.Empty;
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
        var v1 = SecureToken.New();
        var v2 = SecureToken.Parse(v1.ToString(), CultureInfo.InvariantCulture);
        Assert.Equal(v1, v2);
    }

    [Fact]
    public void EqualValuesAsObjectsAreEqual()
    {
        var v1 = (object)SecureToken.New();
        var v2 = (object)SecureToken.Parse(v1.ToString()!, CultureInfo.InvariantCulture);
        Assert.Equal(v1, v2);
    }
}
