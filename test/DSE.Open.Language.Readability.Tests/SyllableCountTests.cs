// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Language.Readability;

public class SyllableCountTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void GetSyllableCount_throws_on_missing_word(string? input)
    {
        _ = Assert.Throws<ArgumentException>(() => SyllableCount.GetSyllableCount(input!));
    }

    [Fact]
    public void GetSyllableCount_is_case_insensitive()
    {
        var lower = SyllableCount.GetSyllableCount("cat");
        var upper = SyllableCount.GetSyllableCount("CAT");
        var mixed = SyllableCount.GetSyllableCount("Cat");

        Assert.Equal(lower, upper);
        Assert.Equal(lower, mixed);
    }

    [Fact]
    public void GetSyllableCount_trims_whitespace()
    {
        var trimmed = SyllableCount.GetSyllableCount("cat");
        var padded = SyllableCount.GetSyllableCount("  cat  ");
        Assert.Equal(trimmed, padded);
    }

    [Fact]
    public void GetSyllableCount_strips_ascii_possessive_s()
    {
        var bare = SyllableCount.GetSyllableCount("dog");
        var possessive = SyllableCount.GetSyllableCount("dog's");
        Assert.Equal(bare, possessive);
    }

    [Fact]
    public void GetSyllableCount_strips_typographic_possessive_s()
    {
        var bare = SyllableCount.GetSyllableCount("dog");
        var possessive = SyllableCount.GetSyllableCount("dog’s");
        Assert.Equal(bare, possessive);
    }

    [Fact]
    public void GetSyllableCount_returns_negative_one_for_unknown_word()
    {
        var result = SyllableCount.GetSyllableCount("qwertyuiopnotarealword");
        Assert.Equal(-1, result);
    }
}
