// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Language.Readability;

public class SpacheFamiliarWordTests
{
    [Theory]
    [InlineData("cat")]
    [InlineData("dog")]
    [InlineData("house")]
    [InlineData("apple")]
    public void IsFamiliarWord_accepts_list_entries(string word)
    {
        Assert.True(SpacheReadabilityCalculator.IsFamiliarWord(word));
    }

    [Theory]
    [InlineData("Cat")]
    [InlineData("DOG")]
    public void IsFamiliarWord_is_case_insensitive(string word)
    {
        Assert.True(SpacheReadabilityCalculator.IsFamiliarWord(word));
    }

    [Theory]
    [InlineData("cats")]   // plural: fw + "s"
    [InlineData("boxes")]  // fw + "es"
    [InlineData("jumped")] // fw + "ed"
    [InlineData("jumping")] // fw + "ing"
    [InlineData("running")] // fw + "ning"
    [InlineData("swimming")] // fw + "ming"
    public void IsFamiliarWord_accepts_simple_morphological_suffixes(string word)
    {
        Assert.True(SpacheReadabilityCalculator.IsFamiliarWord(word));
    }

    [Theory]
    [InlineData("xylophone")]
    [InlineData("antidisestablishmentarianism")]
    public void IsFamiliarWord_rejects_words_not_in_list(string word)
    {
        Assert.False(SpacheReadabilityCalculator.IsFamiliarWord(word));
    }

    [Fact]
    public void IsUnfamiliarWord_is_inverse_of_IsFamiliarWord()
    {
        Assert.False(SpacheReadabilityCalculator.IsUnfamiliarWord("cat"));
        Assert.True(SpacheReadabilityCalculator.IsUnfamiliarWord("xylophone"));
    }

    [Fact]
    public void IsFamiliarWord_throws_on_null()
    {
        _ = Assert.Throws<ArgumentNullException>(() =>
            SpacheReadabilityCalculator.IsFamiliarWord(null!));
    }

    [Fact]
    public void IsUnfamiliarWord_throws_on_null()
    {
        _ = Assert.Throws<ArgumentNullException>(() =>
            SpacheReadabilityCalculator.IsUnfamiliarWord(null!));
    }

    [Fact]
    public void GetUnfamiliarWordCount_counts_distinct_unfamiliar_words()
    {
        var words = new[] { "cat", "xylophone", "cat", "chrysanthemum", "house" };
        Assert.Equal(2, SpacheReadabilityCalculator.GetUnfamiliarWordCount(words));
    }

    [Fact]
    public void GetUnfamiliarWordCount_throws_on_null()
    {
        _ = Assert.Throws<ArgumentNullException>(() =>
            SpacheReadabilityCalculator.GetUnfamiliarWordCount(null!));
    }

    [Fact]
    public void FamiliarWords_list_is_nonempty()
    {
        Assert.NotEmpty(SpacheReadabilityCalculator.FamiliarWords);
        Assert.Contains("cat", SpacheReadabilityCalculator.FamiliarWords);
    }
}
