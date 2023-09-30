// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Values.Tests;

public class LabelTests
{
    [Fact]
    public void Parse_WithEmptySpan_ShouldThrowFormatException()
    {
        // Act
        static void Act() => _ = Label.Parse(Span<char>.Empty, null);

        // Assert
        Assert.Throws<FormatException>(Act);
    }

    [Fact]
    public void Parse_WithEmptyString_ShouldThrowFormatException()
    {
        // Assert
        Assert.Throws<FormatException>(Act);

        // Act
        static void Act() => Label.Parse(string.Empty);
    }

    [Fact]
    public void Parse_WithNullString_ShouldThrowArgumentNull() => Assert.Throws<ArgumentNullException>(static () => _ = Label.Parse(null!));

    [Fact]
    public void TryParse_WithEmptySpan_ShouldReturnFalseAndDefaultResult()
    {
        // Act
        var success = Label.TryParse(Span<char>.Empty, null, out var result);

        // Assert
        Assert.False(success);
        Assert.Equal(Label.Empty, result);
    }

    [Fact]
    public void TryParse_WithNullString_ShouldReturnFalseAndDefaultResult()
    {
        // Act
        var success = Label.TryParse(null, out var result);

        // Assert
        Assert.False(success);
        Assert.Equal(Label.Empty, result);
    }

    [Fact]
    public void TryParse_WithEmptyString_ShouldReturnFalseAndDefaultResult()
    {
        // Act
        var success = Label.TryParse(string.Empty, out var result);

        // Assert
        Assert.False(success);
        Assert.Equal(Label.Empty, result);
    }

    [Fact]
    public void TryFormat_WithEmptyCode_ShouldThrowUninitializedValueException()
    {
        // Arrange
        var code = Label.Empty;
        var buffer = new char[1];

        // Act
        void Act() => _ = code.TryFormat(buffer, out _, default, default);

        // Assert
        Assert.Throws<UninitializedValueException<Label, CharSequence>>(Act);
    }

    [Fact]
    public void EqualValuesAreEqual()
    {
        var v1 = Label.Parse("A Label: 1");
        var v2 = Label.Parse(v1.ToString());
        Assert.Equal(v1, v2);
    }

    [Fact]
    public void EqualValueHashCodesAreEqual()
    {
        var v1 = Label.Parse("A Label: 1");
        var v2 = Label.Parse(v1.ToString());
        var h1 = v1.GetHashCode();
        var h2 = v2.GetHashCode();
        Assert.Equal(h1, h2);
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

        var p = l.GetPrefix().ToString();

        Assert.Equal(prefix, p);
    }

    [Fact]
    public void New_WithValue_SetsIntialized()
    {
        var l = new Label("a label");
        Assert.True(l.IsInitialized);
    }
}
