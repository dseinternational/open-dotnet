// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Language.Annotations;

namespace DSE.Open.Language.Readability;

/// <summary>
/// Computes Flesch reading ease and Flesch–Kincaid grade-level scores for English text.
/// </summary>
/// <remarks>
/// <para>
/// Reading ease: <c>206.835 - 1.015 * (words / sentences) - 84.6 * (syllables / words)</c>.
/// Higher values indicate text that is easier to read.
/// </para>
/// <para>
/// Reading grade: <c>0.39 * (words / sentences) + 11.8 * (syllables / words) - 15.59</c>.
/// The result approximates the U.S. school grade level required to comprehend the text.
/// </para>
/// </remarks>
public static class FleschReadabilityCalculator
{
    /// <summary>The coefficient applied to average sentence length in the reading-ease formula.</summary>
    public const double EaseSentenceWeight = 1.015;

    /// <summary>The coefficient applied to average syllables-per-word in the reading-ease formula.</summary>
    public const double EaseWordWeight = 84.6;

    /// <summary>The constant added in the reading-ease formula.</summary>
    public const double EaseBaseScore = 206.835;

    /// <summary>The coefficient applied to average sentence length in the grade-level formula.</summary>
    public const double GradeSentenceWeight = 0.39;

    /// <summary>The coefficient applied to average syllables-per-word in the grade-level formula.</summary>
    public const double GradeWordWeight = 11.8;

    /// <summary>The constant added in the grade-level formula.</summary>
    public const double GradeBaseScore = -15.59;

    /// <summary>
    /// Calculates the Flesch reading-ease score for a collection of sentences.
    /// </summary>
    /// <param name="sentences">The sentences to score. Must contain at least one sentence with at least one word.</param>
    /// <returns>The reading-ease score; higher values indicate easier text.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="sentences"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException"><paramref name="sentences"/> is empty or contains no words.</exception>
    /// <exception cref="InvalidOperationException">A word's syllable count cannot be determined.</exception>
    public static double CalculateReadingEase(IReadOnlyCollection<Sentence> sentences)
    {
        ArgumentNullException.ThrowIfNull(sentences);

        if (sentences.Count == 0)
        {
            throw new ArgumentException("Must contain at least one sentence.", nameof(sentences));
        }

        var words = sentences.SelectMany(s => s.Words).ToArray();

        if (words.Length == 0)
        {
            throw new ArgumentException("Must contain at least one word.", nameof(sentences));
        }

        var unknown = words.FirstOrDefault(w => SyllableCount.GetSyllableCount(w.Form.ToString()) < 1);

        if (unknown != null)
        {
            throw new InvalidOperationException("Cannot determine syllable count for " + unknown.Form);
        }

        var syllableCount = words.Select(w => SyllableCount.GetSyllableCount(w.Form.ToString())).Sum();

        return CalculateReadingEase(words.Length, sentences.Count, syllableCount);
    }

    /// <summary>
    /// Calculates the Flesch reading-ease score from raw counts.
    /// </summary>
    /// <param name="wordCount">The total number of words. Must be greater than zero.</param>
    /// <param name="sentenceCount">The number of sentences. Must be greater than zero.</param>
    /// <param name="syllableCount">The total number of syllables across all words.</param>
    /// <returns>The reading-ease score; higher values indicate easier text.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="wordCount"/> or <paramref name="sentenceCount"/> is less than or equal to zero.</exception>
    public static double CalculateReadingEase(int wordCount, int sentenceCount, int syllableCount)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(wordCount);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(sentenceCount);

        return EaseBaseScore -
            (EaseSentenceWeight * ((double)wordCount / sentenceCount)) -
            (EaseWordWeight * ((double)syllableCount / wordCount));
    }

    /// <summary>
    /// Calculates the Flesch–Kincaid grade-level score for a collection of sentences.
    /// </summary>
    /// <param name="sentences">The sentences to score. Must contain at least one sentence with at least one word.</param>
    /// <returns>The grade-level score; an approximation of the U.S. school grade required to read the text.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="sentences"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException"><paramref name="sentences"/> is empty or contains no words.</exception>
    /// <exception cref="InvalidOperationException">A word's syllable count cannot be determined.</exception>
    public static double CalculateReadingGrade(IReadOnlyCollection<Sentence> sentences)
    {
        ArgumentNullException.ThrowIfNull(sentences);

        if (sentences.Count == 0)
        {
            throw new ArgumentException("Must contain at least one sentence.", nameof(sentences));
        }

        var words = sentences.SelectMany(s => s.Words).ToArray();

        if (words.Length == 0)
        {
            throw new ArgumentException("Must contain at least one word.", nameof(sentences));
        }

        var unknown = words.FirstOrDefault(w => SyllableCount.GetSyllableCount(w.Form.ToString()) < 1);

        if (unknown != null)
        {
            throw new InvalidOperationException("Cannot determine syllable count for " + unknown.Form);
        }

        var syllableCount = words.Select(w => SyllableCount.GetSyllableCount(w.Form.ToString())).Sum();

        return CalculateReadingGrade(words.Length, sentences.Count, syllableCount);
    }

    /// <summary>
    /// Calculates the Flesch–Kincaid grade-level score from raw counts.
    /// </summary>
    /// <param name="wordCount">The total number of words. Must be greater than zero.</param>
    /// <param name="sentenceCount">The number of sentences. Must be greater than zero.</param>
    /// <param name="syllableCount">The total number of syllables across all words.</param>
    /// <returns>The grade-level score; an approximation of the U.S. school grade required to read the text.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="wordCount"/> or <paramref name="sentenceCount"/> is less than or equal to zero.</exception>
    public static double CalculateReadingGrade(int wordCount, int sentenceCount, int syllableCount)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(wordCount);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(sentenceCount);

        return (GradeSentenceWeight * ((double)wordCount / sentenceCount)) +
            (GradeWordWeight * ((double)syllableCount / wordCount)) +
            GradeBaseScore;
    }
}
