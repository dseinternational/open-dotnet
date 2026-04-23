// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Language.Readability;

public class SyllableCountTests
{
    [Theory]
    [InlineData("cat", 1)]
    [InlineData("rabbit", 2)]
    [InlineData("elephant", 3)]
    public void GetSyllableCount_ReturnsKnownCount(string word, int expected)
    {
        // Also implicitly verifies that syllable-counts.txt is found in the
        // output directory — a regression test for the file path bug.
        Assert.Equal(expected, SyllableCount.GetSyllableCount(word));
    }

    [Theory]
    [InlineData("CAT", 1)]
    [InlineData("Rabbit", 2)]
    [InlineData(" elephant ", 3)]
    public void GetSyllableCount_NormalisesCaseAndWhitespace(string word, int expected)
    {
        Assert.Equal(expected, SyllableCount.GetSyllableCount(word));
    }

    [Theory]
    [InlineData("Emma's", "Emma")]
    [InlineData("dog's", "dog")]
    public void GetSyllableCount_StripsPossessiveSuffix(string possessive, string root)
    {
        var rootCount = SyllableCount.GetSyllableCount(root);
        Assert.Equal(rootCount, SyllableCount.GetSyllableCount(possessive));
    }

    [Fact]
    public void GetSyllableCount_UnknownWord_ReturnsMinusOne()
    {
        Assert.Equal(-1, SyllableCount.GetSyllableCount("xyzqfoonotaword"));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("\t")]
    public void GetSyllableCount_NullEmptyOrWhitespace_Throws(string? word)
    {
        _ = Assert.Throws<ArgumentException>(() => SyllableCount.GetSyllableCount(word!));
    }
}
