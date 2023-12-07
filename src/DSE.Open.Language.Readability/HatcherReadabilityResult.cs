// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Language.Readability;

public class HatcherReadabilityResult
{
    public HatcherReadabilityResult(double level, int pageCount, int wordCount,
        int maxLinesOnPage, int maxSentenceLength, int syntaxComplexityScore,
        IEnumerable<string> words5Letters, IEnumerable<string> words6Letters,
        IEnumerable<string> words7Letters, IEnumerable<string> words8OrMoreLetters)
    {
        Level = level;
        PageCount = pageCount;
        WordCount = wordCount;
        MaxLinesOnPage = maxLinesOnPage;
        MaxSentenceLength = maxSentenceLength;
        SyntaxComplexityScore = syntaxComplexityScore;

        Words5Letters = words5Letters.ToList().AsReadOnly();
        if (Words5Letters.Any(w => w.Length != 5))
        {
            throw new ArgumentOutOfRangeException(nameof(words5Letters),
                "Should only include words with 5 characters.");
        }

        Words6Letters = words6Letters.ToList().AsReadOnly();
        if (Words6Letters.Any(w => w.Length != 6))
        {
            throw new ArgumentOutOfRangeException(nameof(words6Letters),
                "Should only include words with 6 characters.");
        }

        Words7Letters = words7Letters.ToList().AsReadOnly();
        if (Words7Letters.Any(w => w.Length != 7))
        {
            throw new ArgumentOutOfRangeException(nameof(words7Letters),
                "Should only include words with 7 characters.");
        }

        Words8OrMoreLetters = words8OrMoreLetters.ToList().AsReadOnly();
        if (Words8OrMoreLetters.Any(w => w.Length < 8))
        {
            throw new ArgumentOutOfRangeException(nameof(words8OrMoreLetters),
                "Should only include words with 8 or more characters.");
        }
    }

    public double Level { get; }

    public int PageCount { get; }

    public int WordCount { get; }

    public int MaxLinesOnPage { get; }

    public int MaxSentenceLength { get; }

    public int SyntaxComplexityScore { get; }

    public IReadOnlyList<string> Words5Letters { get; }

    public IReadOnlyList<string> Words6Letters { get; }

    public IReadOnlyList<string> Words7Letters { get; }

    public IReadOnlyList<string> Words8OrMoreLetters { get; }
}
