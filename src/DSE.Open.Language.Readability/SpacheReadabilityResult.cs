// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Language.Readability;

/// <summary>
/// The result of a <see cref="SpacheReadabilityCalculator"/> scoring run,
/// bundling the computed <see cref="Level"/> with the inputs that produced it.
/// </summary>
public class SpacheReadabilityResult
{
    /// <summary>
    /// Initializes a new <see cref="SpacheReadabilityResult"/>.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="unfamiliarWords"/>
    /// is <see langword="null"/>.</exception>
    public SpacheReadabilityResult(double level, double averageSentenceLength,
        int distinctWordCount, IEnumerable<string> unfamiliarWords)
    {
        ArgumentNullException.ThrowIfNull(unfamiliarWords);

        Level = level;
        AverageSentenceLength = averageSentenceLength;
        DistinctWordCount = distinctWordCount;
        UnfamiliarWords = unfamiliarWords.ToList();
    }

    /// <summary>
    /// The mean number of words per sentence across the scored book.
    /// </summary>
    public double AverageSentenceLength { get; }

    /// <summary>
    /// The number of distinct words found in the scored book.
    /// </summary>
    public int DistinctWordCount { get; }

    /// <summary>
    /// The computed Spache readability level.
    /// </summary>
    public double Level { get; }

    /// <summary>
    /// The distinct unfamiliar words found in the scored book.
    /// </summary>
    public IReadOnlyList<string> UnfamiliarWords { get; }

    /// <summary>
    /// The number of distinct unfamiliar words, equivalent to
    /// <c>UnfamiliarWords.Count</c>.
    /// </summary>
    public int UnfamiliarWordCount => UnfamiliarWords.Count;
}
