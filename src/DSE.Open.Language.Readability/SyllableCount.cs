// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections.Frozen;

namespace DSE.Open.Language.Readability;

/// <summary>
/// Looks up the number of syllables in an English word using a precomputed
/// dictionary file (<c>syllable-counts.txt</c>) loaded from the application
/// base directory on first use.
/// </summary>
public static class SyllableCount
{
    private static FrozenDictionary<string, int>? s_counts;
    private static readonly object s_countsLock = new();

    /// <summary>
    /// Returns the number of syllables in <paramref name="textWord"/>, or
    /// <c>-1</c> if the word is not found in the dictionary.
    /// </summary>
    /// <param name="textWord">The word to look up. Leading and trailing
    /// whitespace is ignored, the word is folded to lower case, and a
    /// trailing possessive <c>'s</c> (ASCII or typographic apostrophe) is
    /// stripped before lookup.</param>
    /// <exception cref="ArgumentException"><paramref name="textWord"/> is
    /// <see langword="null"/>, empty or whitespace.</exception>
    public static int GetSyllableCount(string textWord)
    {
        if (string.IsNullOrWhiteSpace(textWord))
        {
            throw new ArgumentException("A word is required.", nameof(textWord));
        }

        textWord = textWord.Trim().ToLowerInvariant();

        textWord = textWord.Replace("'s", string.Empty, StringComparison.Ordinal);
        textWord = textWord.Replace("’s", string.Empty, StringComparison.Ordinal);

        if (s_counts is null)
        {
            lock (s_countsLock)
            {
                s_counts = ReadCounts();
            }
        }

        return s_counts.GetValueOrDefault(textWord, -1);
    }

    private static FrozenDictionary<string, int> ReadCounts()
    {
        var countsPath = Path.Combine(AppContext.BaseDirectory, "syllable-counts.txt");

        using var reader = File.OpenText(countsPath);

        var counts = new List<KeyValuePair<string, int>>(135000);

        while (!reader.EndOfStream)
        {
            var line = reader.ReadLine();
            if (line is not null)
            {
                var parts = line.Split(' ');

                if (parts.Length == 2 && int.TryParse(parts[1], out var count))
                {
                    counts.Add(KeyValuePair.Create(parts[0], count));
                }
            }
        }

        return counts.ToFrozenDictionary();
    }
}
