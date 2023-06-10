// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Values.Tests;

public class LabelTests
{
    [Fact]
    public void Parse_WithEmptySpan_ShouldReturnDefault()
    {
        var a = Label.Parse(Span<char>.Empty, null);
        Assert.Equal(Label.Empty, a);
    }

    [Fact]
    public void Parse_WithEmptyString_ShouldReturnDefault()
    {
        var a = Label.Parse(string.Empty);
        Assert.Equal(Label.Empty, a);
    }

    [Fact]
    public void Parse_WithNullString_ShouldThrowArgumentNull() => Assert.Throws<ArgumentNullException>(() => Label.Parse(null!));

    [Fact]
    public void TryParse_WithEmptySpan_ShouldReturnTrueAndDefaultResult()
    {
        var success = Label.TryParse(Span<char>.Empty, null, out var result);
        Assert.True(success);
        Assert.Equal(Label.Empty, result);
    }

    [Fact]
    public void TryParse_WithNullString_ShouldReturnFalseAndDefaultResult()
    {
        var success = Label.TryParse(null, out var result);
        Assert.False(success);
        Assert.Equal(Label.Empty, result);
    }

    [Fact]
    public void TryParse_WithEmptyString_ShouldReturnTrueAndDefaultResult()
    {
        var success = Label.TryParse(string.Empty, out var result);
        Assert.True(success);
        Assert.Equal(Label.Empty, result);
    }

    [Fact]
    public void TryFormat_WithEmptyCode_ShouldReturnTrueWithNothingWritten()
    {
        // Arrange
        var code = Label.Empty;
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
        var v1 = Label.Parse("A Label: 1");
        var v2 = Label.Parse(v1.ToString());
        Assert.Equal(v1, v2);
    }

    [Fact]
    public void EqualValuesAsObjectsAreEqual()
    {
        var v1 = (object)Label.Parse("A Label: 1");
        var v2 = (object)Label.Parse(v1.ToString()!);
        Assert.Equal(v1, v2);
    }

    [Theory]
    [InlineData("type: Online Course", "type")]
    [InlineData("topic: Reading: Literacy", "topic")]
    public void GetPrefixReturnsPrefix(string label, string prefix)
    {
        var l = Label.Parse(label);

        Assert.True(l.HasPrefix);

        var p = l.GetPrefix();

        Assert.Equal(prefix, p);
    }
}
