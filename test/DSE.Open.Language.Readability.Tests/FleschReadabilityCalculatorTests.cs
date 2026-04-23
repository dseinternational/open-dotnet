// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Language.Readability;

public class FleschReadabilityCalculatorTests
{
    // Reading ease = 206.835 - 1.015 * (words / sentences) - 84.6 * (syllables / words)

    [Theory]
    [InlineData(100, 5, 150, 59.635)]   // 206.835 - 1.015 * 20 - 84.6 * 1.5
    [InlineData(120, 8, 180, 64.71)]    // 206.835 - 1.015 * 15 - 84.6 * 1.5
    [InlineData(7, 2, 9, 94.511)]       // small sample sensitive to integer division (7/2 == 3.5, 9/7 ≈ 1.2857)
    public void CalculateReadingEase_ProducesFloatingPointResult(
        int wordCount,
        int sentenceCount,
        int syllableCount,
        double expected)
    {
        var ease = FleschReadabilityCalculator.CalculateReadingEase(wordCount, sentenceCount, syllableCount);
        Assert.Equal(expected, ease, 3);
    }

    // Reading grade = 0.39 * (words / sentences) + 11.8 * (syllables / words) - 15.59

    [Theory]
    [InlineData(100, 5, 150, 9.91)]     // 0.39 * 20 + 11.8 * 1.5 - 15.59
    [InlineData(120, 8, 180, 7.96)]     // 0.39 * 15 + 11.8 * 1.5 - 15.59
    [InlineData(7, 2, 9, 0.946)]        // 0.39 * 3.5 + 11.8 * (9/7) - 15.59
    public void CalculateReadingGrade_ProducesFloatingPointResult(
        int wordCount,
        int sentenceCount,
        int syllableCount,
        double expected)
    {
        var grade = FleschReadabilityCalculator.CalculateReadingGrade(wordCount, sentenceCount, syllableCount);
        Assert.Equal(expected, grade, 3);
    }

    [Fact]
    public void CalculateReadingEase_RatiosUseFloatingPointDivision()
    {
        // If integer division were used:
        //   (7 / 2) == 3 and (9 / 7) == 1
        //   => 206.835 - 1.015 * 3 - 84.6 * 1 == 119.19
        // With floating-point division the correct result is ~94.511.
        var ease = FleschReadabilityCalculator.CalculateReadingEase(7, 2, 9);
        Assert.NotEqual(119.19, ease, 2);
        Assert.Equal(94.511, ease, 3);
    }

    [Fact]
    public void CalculateReadingGrade_RatiosUseFloatingPointDivision()
    {
        // If integer division were used:
        //   (7 / 2) == 3 and (9 / 7) == 1
        //   => 0.39 * 3 + 11.8 * 1 - 15.59 == -2.62
        // With floating-point division the correct result is ~0.946.
        var grade = FleschReadabilityCalculator.CalculateReadingGrade(7, 2, 9);
        Assert.NotEqual(-2.62, grade, 2);
        Assert.Equal(0.946, grade, 3);
    }
}
