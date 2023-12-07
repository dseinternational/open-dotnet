// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Language.Readability;

public class SpacheReadabilityResult
{
    public SpacheReadabilityResult(double level, double averageSentenceLength,
        int distinctWordCount, IEnumerable<string> unfamiliarWords)
    {
        ArgumentNullException.ThrowIfNull(unfamiliarWords);

        Level = level;
        AverageSentenceLength = averageSentenceLength;
        DistinctWordCount = distinctWordCount;
        UnfamiliarWords = unfamiliarWords.ToList();
    }

    public double AverageSentenceLength { get; }

    public int DistinctWordCount { get; }

    public double Level { get; }

    public IReadOnlyList<string> UnfamiliarWords { get; }

    public int UnfamiliarWordCount => UnfamiliarWords.Count;
}
