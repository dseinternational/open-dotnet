// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Language.Annotations.Books;

namespace DSE.Open.Language.Readability;

/// <summary>
/// Computes a readability score for a children's <see cref="Book"/> using
/// the Hatcher formula, which combines page count, maximum lines per page,
/// maximum sentence length, word count, a syntactic complexity rating, and
/// the number of distinct words of given lengths.
/// </summary>
public static class HatcherReadabilityCalculator
{
    private const double MaxLinesWeight = 0.221677;
    private const double NumberOfPagesWeight = 0.099218;
    private const double MaxSentenceLengthWeight = 0.235628;
    private const double WordCountWeight = 0.061626;
    private const double SyntaxComplexityWeight = 0.479433;
    private const double WordLengthOver5Weight = 0.200678;
    private const double WordCount5Weight = 0.272515;
    private const double AdjustmentFactor = -3.263162;

    /// <summary>
    /// Calculates the Hatcher readability score for <paramref name="book"/>.
    /// </summary>
    /// <param name="book">The book to score.</param>
    /// <param name="maxLinesOnPage">The maximum number of lines appearing
    /// on any page of the book.</param>
    /// <param name="syntaxComplexity">A rating of the syntactic complexity
    /// of the book's text.</param>
    /// <exception cref="ArgumentNullException"><paramref name="book"/> is <see langword="null"/>.</exception>
    public static HatcherReadabilityResult Calculate(
        Book book,
        int maxLinesOnPage = 1,
        int syntaxComplexity = 0)
    {
        ArgumentNullException.ThrowIfNull(book);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(maxLinesOnPage);
        ArgumentOutOfRangeException.ThrowIfNegative(syntaxComplexity);

        var pageCount = book.Pages.Count;

        if (pageCount == 0)
        {
            throw new ArgumentException("Must contain at least one page.", nameof(book));
        }

        var allSentences = book.Sentences;

        var allWords = allSentences.SelectMany(s => s.Words).ToArray();

        if (allWords.Length == 0)
        {
            throw new ArgumentException("Must contain at least one word.", nameof(book));
        }

        var allDistinctWords = allWords.Distinct().ToArray();

        var wordCount = allWords.Length;

        if (wordCount > 100)
        {
            wordCount = 101;
        }

        var words8 = allDistinctWords.Where(w => w.Form.Length >= 8).Select(w => w.Form.ToString());
        var words7 = allDistinctWords.Where(w => w.Form.Length == 7).Select(w => w.Form.ToString());
        var words6 = allDistinctWords.Where(w => w.Form.Length == 6).Select(w => w.Form.ToString());
        var words5 = allDistinctWords.Where(w => w.Form.Length == 5).Select(w => w.Form.ToString());

        var wordsLonger5Count = words8.Count() + words7.Count() + words6.Count();
        var words5Count = words5.Count();
        var maxSentenceLength = allSentences.Select(s => s.Words.Count).Max();

        var level = CalculateLevel(pageCount, wordCount, maxLinesOnPage,
            maxSentenceLength, syntaxComplexity, wordsLonger5Count, words5Count);

        return new(level, pageCount, wordCount, maxLinesOnPage,
            maxSentenceLength, syntaxComplexity, words5, words6, words7, words8);
    }

    /// <summary>
    /// Calculates the Hatcher readability level from precomputed totals.
    /// </summary>
    /// <param name="pageCount">The total number of pages in the book.</param>
    /// <param name="wordCount">The total number of running words.</param>
    /// <param name="maxLinesOnPage">The maximum number of lines appearing
    /// on any page.</param>
    /// <param name="maxSentenceLength">The length, in words, of the
    /// longest sentence in the book.</param>
    /// <param name="syntaxComplexityScore">A rating of the syntactic
    /// complexity of the book's text.</param>
    /// <param name="wordsLonger5Count">The count of distinct words whose
    /// length is greater than five characters.</param>
    /// <param name="words5Count">The count of distinct five-letter words.</param>
    public static double CalculateLevel(int pageCount, int wordCount, int maxLinesOnPage,
        int maxSentenceLength, int syntaxComplexityScore, int wordsLonger5Count, int words5Count)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(pageCount);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(wordCount);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(maxLinesOnPage);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(maxSentenceLength);
        ArgumentOutOfRangeException.ThrowIfNegative(syntaxComplexityScore);
        ArgumentOutOfRangeException.ThrowIfNegative(wordsLonger5Count);
        ArgumentOutOfRangeException.ThrowIfNegative(words5Count);

        var level = (maxLinesOnPage * MaxLinesWeight)
            + (pageCount * NumberOfPagesWeight)
            + (maxSentenceLength * MaxSentenceLengthWeight)
            + (wordCount * WordCountWeight)
            + (syntaxComplexityScore * SyntaxComplexityWeight)
            + (wordsLonger5Count * WordLengthOver5Weight)
            + (words5Count * WordCount5Weight)
            + AdjustmentFactor;

        return level;
    }

}
