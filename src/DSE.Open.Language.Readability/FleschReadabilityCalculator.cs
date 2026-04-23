// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Language.Annotations;

namespace DSE.Open.Language.Readability;

/// <summary>
/// Computes the Flesch Reading Ease score and the Flesch-Kincaid Grade Level
/// for a body of text, based on average sentence length (in words) and
/// average word length (in syllables).
/// </summary>
/// <remarks>
/// See <see href="https://en.wikipedia.org/wiki/Flesch%E2%80%93Kincaid_readability_tests"/>.
/// </remarks>
public static class FleschReadabilityCalculator
{
    /// <summary>
    /// The weight applied to the average words-per-sentence term when
    /// calculating the Reading Ease score.
    /// </summary>
    public const double EaseSentenceWeight = 1.015;

    /// <summary>
    /// The weight applied to the average syllables-per-word term when
    /// calculating the Reading Ease score.
    /// </summary>
    public const double EaseWordWeight = 84.6;

    /// <summary>
    /// The base score from which the Reading Ease terms are subtracted.
    /// </summary>
    public const double EaseBaseScore = 206.835;

    /// <summary>
    /// The weight applied to the average words-per-sentence term when
    /// calculating the Reading Grade level.
    /// </summary>
    public const double GradeSentenceWeight = 0.39;

    /// <summary>
    /// The weight applied to the average syllables-per-word term when
    /// calculating the Reading Grade level.
    /// </summary>
    public const double GradeWordWeight = 11.8;

    /// <summary>
    /// The constant added to the weighted terms when calculating the
    /// Reading Grade level.
    /// </summary>
    public const double GradeBaseScore = -15.59;

    /// <summary>
    /// Calculates the Flesch Reading Ease score for the supplied
    /// <paramref name="sentences"/>. Higher scores indicate easier text;
    /// typical English scores range from roughly <c>0</c> (very difficult)
    /// to <c>100</c> (very easy).
    /// </summary>
    /// <param name="sentences">The sentences to score.</param>
    /// <exception cref="ArgumentNullException"><paramref name="sentences"/> is <see langword="null"/>.</exception>
    /// <exception cref="InvalidOperationException">The syllable count for
    /// a word cannot be determined.</exception>
    public static double CalculateReadingEase(IReadOnlyCollection<Sentence> sentences)
    {
        ArgumentNullException.ThrowIfNull(sentences);

        var words = sentences.SelectMany(s => s.Words).ToArray();

        var unknown = words.FirstOrDefault(w => SyllableCount.GetSyllableCount(w.Form.ToString()) < 1);

        if (unknown != null)
        {
            throw new InvalidOperationException("Cannot determine syllable count for " + unknown.Form);
        }

        var syllableCount = words.Select(w => SyllableCount.GetSyllableCount(w.Form.ToString())).Sum();

        return CalculateReadingEase(words.Length, sentences.Count, syllableCount);
    }

    /// <summary>
    /// Calculates the Flesch Reading Ease score from precomputed totals.
    /// </summary>
    /// <param name="wordCount">The total number of words.</param>
    /// <param name="sentenceCount">The total number of sentences.</param>
    /// <param name="syllableCount">The total number of syllables.</param>
    public static double CalculateReadingEase(int wordCount, int sentenceCount, int syllableCount)
    {
        return EaseBaseScore -
            (EaseSentenceWeight * ((double)wordCount / sentenceCount)) -
            (EaseWordWeight * ((double)syllableCount / wordCount));
    }

    /// <summary>
    /// Calculates the Flesch-Kincaid Grade Level for the supplied
    /// <paramref name="sentences"/>. The result approximates the US school
    /// grade required to understand the text.
    /// </summary>
    /// <param name="sentences">The sentences to score.</param>
    /// <exception cref="ArgumentNullException"><paramref name="sentences"/> is <see langword="null"/>.</exception>
    /// <exception cref="InvalidOperationException">The syllable count for
    /// a word cannot be determined.</exception>
    public static double CalculateReadingGrade(IReadOnlyCollection<Sentence> sentences)
    {
        ArgumentNullException.ThrowIfNull(sentences);

        var words = sentences.SelectMany(s => s.Words).ToArray();

        var unknown = words.FirstOrDefault(w => SyllableCount.GetSyllableCount(w.Form.ToString()) < 1);

        if (unknown != null)
        {
            throw new InvalidOperationException("Cannot determine syllable count for " + unknown.Form);
        }

        var syllableCount = words.Select(w => SyllableCount.GetSyllableCount(w.Form.ToString())).Sum();

        return CalculateReadingGrade(words.Length, sentences.Count, syllableCount);
    }

    /// <summary>
    /// Calculates the Flesch-Kincaid Grade Level from precomputed totals.
    /// </summary>
    /// <param name="wordCount">The total number of words.</param>
    /// <param name="sentenceCount">The total number of sentences.</param>
    /// <param name="syllableCount">The total number of syllables.</param>
    public static double CalculateReadingGrade(int wordCount, int sentenceCount, int syllableCount)
    {
        return (GradeSentenceWeight * ((double)wordCount / sentenceCount)) +
            (GradeWordWeight * ((double)syllableCount / wordCount)) +
            GradeBaseScore;
    }
}
