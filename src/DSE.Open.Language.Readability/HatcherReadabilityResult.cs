// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Language.Readability;

/// <summary>
/// The result of a <see cref="HatcherReadabilityCalculator"/> scoring run,
/// bundling the computed <see cref="Level"/> with the inputs that produced it.
/// </summary>
public class HatcherReadabilityResult
{
    /// <summary>
    /// Initializes a new <see cref="HatcherReadabilityResult"/>.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">A word list contains
    /// a word whose length does not match the bucket it is assigned to.</exception>
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

    /// <summary>
    /// The computed Hatcher readability level.
    /// </summary>
    public double Level { get; }

    /// <summary>
    /// The number of pages in the scored book.
    /// </summary>
    public int PageCount { get; }

    /// <summary>
    /// The number of running words in the scored book (capped at 101 for
    /// scoring purposes).
    /// </summary>
    public int WordCount { get; }

    /// <summary>
    /// The maximum number of lines appearing on any page of the book.
    /// </summary>
    public int MaxLinesOnPage { get; }

    /// <summary>
    /// The length, in words, of the longest sentence in the book.
    /// </summary>
    public int MaxSentenceLength { get; }

    /// <summary>
    /// The syntactic complexity rating supplied to the calculator.
    /// </summary>
    public int SyntaxComplexityScore { get; }

    /// <summary>
    /// The distinct five-letter words found in the book.
    /// </summary>
    public IReadOnlyList<string> Words5Letters { get; }

    /// <summary>
    /// The distinct six-letter words found in the book.
    /// </summary>
    public IReadOnlyList<string> Words6Letters { get; }

    /// <summary>
    /// The distinct seven-letter words found in the book.
    /// </summary>
    public IReadOnlyList<string> Words7Letters { get; }

    /// <summary>
    /// The distinct words of eight or more letters found in the book.
    /// </summary>
    public IReadOnlyList<string> Words8OrMoreLetters { get; }
}
