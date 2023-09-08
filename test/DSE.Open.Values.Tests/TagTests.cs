// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Values.Tests;

public class TagTests
{
    [Fact]
    public void Parse_WithEmptySpan_ShouldReturnDefault()
    {
        var a = Tag.Parse(Span<char>.Empty, null);
        Assert.Equal(Tag.Empty, a);
    }

    [Fact]
    public void Parse_WithEmptyString_ShouldReturnDefault()
    {
        var a = Tag.Parse(string.Empty);
        Assert.Equal(Tag.Empty, a);
    }

    [Fact]
    public void Parse_WithNullString_ShouldThrowArgumentNull() => Assert.Throws<ArgumentNullException>(() => Tag.Parse(null!));

    [Fact]
    public void Parse_WithInvalidTag_ShouldThrowFormatException()
    {
        // Arrange
        const string tag = "a";

        // Act
        static void Parse()
        {
            _ = Tag.Parse(tag);
        }

        // Assert
        _ = Assert.Throws<FormatException>(Parse);
    }

    [Fact]
    public void TryParse_WithEmptySpan_ShouldReturnTrueAndDefaultResult()
    {
        var success = Tag.TryParse(Span<char>.Empty, null, out var result);
        Assert.True(success);
        Assert.Equal(Tag.Empty, result);
    }

    [Fact]
    public void TryParse_WithNullString_ShouldReturnFalseAndDefaultResult()
    {
        var success = Tag.TryParse(null, out var result);
        Assert.False(success);
        Assert.Equal(Tag.Empty, result);
    }

    [Fact]
    public void TryParse_WithEmptyString_ShouldReturnTrueAndDefaultResult()
    {
        var success = Tag.TryParse(string.Empty, out var result);
        Assert.True(success);
        Assert.Equal(Tag.Empty, result);
    }

    [Fact]
    public void TryFormat_WithEmptyCode_ShouldReturnTrueWithNothingWritten()
    {
        // Arrange
        var code = Tag.Empty;
        var buffer = new char[1];

        // Act
        var success = code.TryFormat(buffer, out var charsWritten, default, default);

        // Assert
        Assert.True(success);
        Assert.Equal(0, charsWritten);
    }

    [Fact]
    public void TryFormat_WithBufferTooSmall_ShouldReturnFalse()
    {
        // Arrange
        var code = Tag.Parse("tag");
        var buffer = Span<char>.Empty;

        // Act
        var success = code.TryFormat(buffer, out var charsWritten);

        // Assert
        Assert.False(success);
        Assert.Equal(0, charsWritten);
    }

    [Fact]
    public void ToString_WithEmptyTag_ShouldReturnEmptyString()
    {
        // Arrange
        var tag = Tag.Empty;

        // Act
        var result = tag.ToString();

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void EqualValuesAreEqual()
    {
        var v1 = Tag.Parse("tag");
        var v2 = Tag.Parse(v1.ToString());
        Assert.Equal(v1, v2);
    }

    [Fact]
    public void GetHashCodeReturnsEqualValues()
    {
        var v1 = Tag.Parse("tag");
        var v2 = Tag.Parse(v1.ToString());
        Assert.Equal(v1.GetHashCode(), v2.GetHashCode());
    }

    [Fact]
    public void EqualValuesAsObjectsAreEqual()
    {
        var v1 = (object)Tag.Parse("tag");
        var v2 = (object)Tag.Parse(v1.ToString()!);
        Assert.Equal(v1, v2);
    }

    [Fact]
    public void New_WithInvalidTag_ShouldThrowArgumentException()
    {
        // Arrange
        const string tag = "a";

        // Act
        static void New()
        {
            _ = new Tag(tag);
        }

        // Assert
        _ = Assert.Throws<ArgumentOutOfRangeException>(New);
    }

    [Fact]
    public void New_WithValidTag_ShouldReturnTag()
    {
        // Arrange
        const string tag = "tag";

        // Act
        var result = new Tag(tag);

        // Assert
        Assert.Equal(tag, result.ToString());
    }

    [Theory]
    [MemberData(nameof(ValidTagStrings))]
    public void CreateValidTag(string tagStr)
    {
        var tag = new Tag(tagStr);
        Assert.Equal(tagStr, tag.ToString());
    }

    public static readonly TheoryData<string> ValidTagStrings = new()
    {
        "tag",
        "TAG",
        "a-a",
        "1-1",
        "1:1",
        "a-longer-tag/divided-with-slash:and-colon",
        "reading/rr-reading-session-plan",
        "reading/r5-letter-sound-cards",
        "reading/r8-high-frequency-word-cards-first-100",
        "reading/r17-high-frequency-words-record-form-first-100"
    };
}
