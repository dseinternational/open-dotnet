// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Language.Annotations;

namespace DSE.Open.Language.Readability;

public static class FleschReadabilityCalculator
{
    public const double EaseSentenceWeight = 1.015;
    public const double EaseWordWeight = 84.6;
    public const double EaseBaseScore = 206.835;

    public const double GradeSentenceWeight = 0.39;
    public const double GradeWordWeight = 11.8;
    public const double GradeBaseScore = -15.59;

    public static double CalculateReadingEase(IReadOnlyCollection<Sentence> sentences)
    {
        Guard.IsNotNull(sentences);

        var words = sentences.SelectMany(s => s.Words).ToArray();

        var unknown = words.FirstOrDefault(w => SyllableCount.GetSyllableCount(w.Form.ToString()) < 1);

        if (unknown != null)
        {
            throw new InvalidOperationException("Cannot determine syllable count for " + unknown.Form);
        }

        var syllableCount = words.Select(w => SyllableCount.GetSyllableCount(w.Form.ToString())).Sum();

        return CalculateReadingEase(words.Length, sentences.Count, syllableCount);
    }

    public static double CalculateReadingEase(int wordCount, int sentenceCount, int syllableCount)
    {
        return EaseBaseScore -
            (EaseSentenceWeight * (wordCount / sentenceCount)) -
            (EaseWordWeight * (syllableCount / wordCount));
    }

    public static double CalculateReadingGrade(IReadOnlyCollection<Sentence> sentences)
    {
        Guard.IsNotNull(sentences);

        var words = sentences.SelectMany(s => s.Words).ToArray();

        var unknown = words.FirstOrDefault(w => SyllableCount.GetSyllableCount(w.Form.ToString()) < 1);

        if (unknown != null)
        {
            throw new InvalidOperationException("Cannot determine syllable count for " + unknown.Form);
        }

        var syllableCount = words.Select(w => SyllableCount.GetSyllableCount(w.Form.ToString())).Sum();

        return CalculateReadingGrade(words.Length, sentences.Count, syllableCount);
    }

    public static double CalculateReadingGrade(int wordCount, int sentenceCount, int syllableCount)
    {
        return (GradeSentenceWeight * (wordCount / sentenceCount)) +
            (GradeWordWeight * (syllableCount / wordCount)) +
            GradeBaseScore;
    }
}
