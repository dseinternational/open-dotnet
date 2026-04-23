// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Language.Readability;

public class FleschReadabilityCalculatorTests
{
    // Reference numbers derived from 206.835 - 1.015*(W/S) - 84.6*(Syl/W)
    [Theory]
    [InlineData(100, 10, 150, 69.785)]
    [InlineData(200, 20, 400, 27.485)]
    [InlineData(120, 6, 180, 59.635)]
    public void CalculateReadingEase_matches_flesch_formula(
        int wordCount,
        int sentenceCount,
        int syllableCount,
        double expected)
    {
        var actual = FleschReadabilityCalculator.CalculateReadingEase(wordCount, sentenceCount, syllableCount);
        Assert.Equal(expected, actual, 3);
    }

    // Reference numbers derived from 0.39*(W/S) + 11.8*(Syl/W) - 15.59
    [Theory]
    [InlineData(100, 10, 150, 6.01)]
    [InlineData(200, 20, 400, 11.91)]
    [InlineData(120, 6, 180, 9.91)]
    public void CalculateReadingGrade_matches_flesch_kincaid_formula(
        int wordCount,
        int sentenceCount,
        int syllableCount,
        double expected)
    {
        var actual = FleschReadabilityCalculator.CalculateReadingGrade(wordCount, sentenceCount, syllableCount);
        Assert.Equal(expected, actual, 3);
    }

    [Fact]
    public void Reading_ease_is_higher_for_shorter_words_and_sentences()
    {
        var simple = FleschReadabilityCalculator.CalculateReadingEase(wordCount: 20, sentenceCount: 5, syllableCount: 25);
        var complex = FleschReadabilityCalculator.CalculateReadingEase(wordCount: 20, sentenceCount: 2, syllableCount: 60);
        Assert.True(simple > complex);
    }

    [Fact]
    public void Reading_grade_is_higher_for_longer_words_and_sentences()
    {
        var simple = FleschReadabilityCalculator.CalculateReadingGrade(wordCount: 20, sentenceCount: 5, syllableCount: 25);
        var complex = FleschReadabilityCalculator.CalculateReadingGrade(wordCount: 20, sentenceCount: 2, syllableCount: 60);
        Assert.True(complex > simple);
    }

    [Fact]
    public void CalculateReadingEase_throws_on_null_sentences()
    {
        _ = Assert.Throws<ArgumentNullException>(() =>
            FleschReadabilityCalculator.CalculateReadingEase(null!));
    }

    [Fact]
    public void CalculateReadingGrade_throws_on_null_sentences()
    {
        _ = Assert.Throws<ArgumentNullException>(() =>
            FleschReadabilityCalculator.CalculateReadingGrade(null!));
    }

    [Fact]
    public void Ease_weights_match_published_constants()
    {
        Assert.Equal(206.835, FleschReadabilityCalculator.EaseBaseScore);
        Assert.Equal(1.015, FleschReadabilityCalculator.EaseSentenceWeight);
        Assert.Equal(84.6, FleschReadabilityCalculator.EaseWordWeight);
    }

    [Fact]
    public void Grade_weights_match_published_constants()
    {
        Assert.Equal(-15.59, FleschReadabilityCalculator.GradeBaseScore);
        Assert.Equal(0.39, FleschReadabilityCalculator.GradeSentenceWeight);
        Assert.Equal(11.8, FleschReadabilityCalculator.GradeWordWeight);
    }
}
