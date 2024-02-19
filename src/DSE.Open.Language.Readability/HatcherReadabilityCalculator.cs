// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Language.Annotations.Books;

namespace DSE.Open.Language.Readability;

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

    public static HatcherReadabilityResult Calculate(
        Book book,
        int maxLinesOnPage = 1,
        int syntaxComplexity = 0)
    {
        Guard.IsNotNull(book);

        var pageCount = book.Pages.Count;

        var allSentences = book.Sentences;

        var allWords = allSentences.SelectMany(s => s.Words).ToArray();

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

        return new HatcherReadabilityResult(level, pageCount, wordCount, maxLinesOnPage,
            maxSentenceLength, syntaxComplexity, words5, words6, words7, words8);
    }

    public static double CalculateLevel(int pageCount, int wordCount, int maxLinesOnPage,
        int maxSentenceLength, int syntaxComplexityScore, int wordsLonger5Count, int words5Count)
    {
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
