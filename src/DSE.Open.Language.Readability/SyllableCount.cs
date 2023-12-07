// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections.Frozen;

namespace DSE.Open.Language.Readability;

public static class SyllableCount
{
    private static FrozenDictionary<string, int>? s_counts;
    private static readonly object s_countsLock = new();

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

        if (s_counts.TryGetValue(textWord, out var count))
        {
            return count;
        }

        return -1;
    }

    private static FrozenDictionary<string, int> ReadCounts()
    {
        var countsPath = Path.Combine(AppContext.BaseDirectory, "Readability/syllable-counts.txt");

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
