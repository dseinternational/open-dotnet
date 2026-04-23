// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Language.Readability;

/// <summary>
/// The result of computing a Spache readability score for a body of text.
/// </summary>
public class SpacheReadabilityResult
{
    /// <summary>
    /// Initialises a new <see cref="SpacheReadabilityResult"/>.
    /// </summary>
    /// <param name="level">The computed Spache grade level.</param>
    /// <param name="averageSentenceLength">The average number of words per sentence.</param>
    /// <param name="distinctWordCount">The number of distinct words across the text.</param>
    /// <param name="unfamiliarWords">The distinct words that are not in the Spache familiar-word list.</param>
    /// <exception cref="ArgumentNullException"><paramref name="unfamiliarWords"/> is <see langword="null"/>.</exception>
    public SpacheReadabilityResult(double level, double averageSentenceLength,
        int distinctWordCount, IEnumerable<string> unfamiliarWords)
    {
        ArgumentNullException.ThrowIfNull(unfamiliarWords);

        Level = level;
        AverageSentenceLength = averageSentenceLength;
        DistinctWordCount = distinctWordCount;
        UnfamiliarWords = unfamiliarWords.ToList();
    }

    /// <summary>The average number of words per sentence in the analysed text.</summary>
    public double AverageSentenceLength { get; }

    /// <summary>The number of distinct words observed across the analysed text.</summary>
    public int DistinctWordCount { get; }

    /// <summary>The computed Spache grade level.</summary>
    public double Level { get; }

    /// <summary>The distinct words that are not in the Spache familiar-word list.</summary>
    public IReadOnlyList<string> UnfamiliarWords { get; }

    /// <summary>The number of distinct unfamiliar words.</summary>
    public int UnfamiliarWordCount => UnfamiliarWords.Count;
}
