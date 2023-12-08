// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Buffers;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using DSE.Open.Collections.Generic;

namespace DSE.Open.Language.Annotations;

/// <summary>
/// Carries data about a token in a sentence. Can be serialized to and from JSON
/// and CoNLL-U Format (<see href="https://universaldependencies.org/format.html"/>).
/// </summary>
/// <remarks>
/// Out token/word model is based on the CoNLL-U format and Stanza NLP
/// data objects (<see href="https://stanfordnlp.github.io/stanza/data_objects.html#token"/>.
/// </remarks>
public record class Token
    : ISpanFormattable,
      ISpanParsable<Token>
{
    /// <summary>
    /// Index of the token in the sentence.
    /// </summary>
    /// <remarks>Word index, integer starting at 1 for each new sentence; may be a range for
    /// multiword tokens; may be a decimal number for empty nodes (decimal numbers can be
    /// lower than 1 but must be greater than 0).</remarks>
    [JsonPropertyName("id")]
    public required TokenIndex Index { get; init; }

    [JsonPropertyName("text")]
    public required TokenText Text { get; init; }

    [JsonIgnore]
    public bool IsMultiwordToken => Index.IsMultiwordIndex;

    [JsonIgnore]
    public bool IsEmptyNode => Index.IsEmptyNode;

    [JsonPropertyName("words")]
    public ReadOnlyValueCollection<Word> Words { get; init; } = [];

    public static Token Parse(ReadOnlySpan<char> s)
    {
        return Parse(s, default);
    }

    public static Token ParseInvariant(ReadOnlySpan<char> s)
    {
        return Parse(s, CultureInfo.InvariantCulture);
    }

    public static Token Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
    {
        if (TryParse(s, provider, out var token))
        {
            return token;
        }

        ThrowHelper.ThrowFormatException($"Failed to parse {nameof(Token)}.");
        return null; // unreachable
    }

    public static Token Parse(string s)
    {
        return Parse(s, default);
    }

    public static Token ParseInvariant(string s)
    {
        return Parse(s, CultureInfo.InvariantCulture);
    }

    public static Token Parse(string s, IFormatProvider? provider)
    {
        ArgumentNullException.ThrowIfNull(s);

        if (TryParse(s, provider, out var token))
        {
            return token;
        }

        ThrowHelper.ThrowFormatException($"Failed to parse {nameof(Token)}.");
        return null; // unreachable
    }

    public static bool TryParse(
        ReadOnlySpan<char> s,
        IFormatProvider? provider,
        [MaybeNullWhen(false)] out Token result)
    {
        // TODO: handle multi-word tokens
        // https://universaldependencies.org/u/overview/tokenization.html
        // https://universaldependencies.org/format.html

        var words = new List<Word>(2);

        foreach (var line in s.EnumerateLines())
        {
            if (Word.TryParse(line, provider, out var word))
            {
                words.Add(word);
            }
            else
            {
                result = default;
                return false;
            }
        }

        if (words.Count > 0)
        {
            result = new Token
            {
                Index = new TokenIndex(words[0].Index), // TODO: multiword support
                Text = words[0].Form,
                Words = [.. words]
            };
            return true;
        }

        result = default;
        return false;
    }

    public static bool TryParse(
        [NotNullWhen(true)] string? s,
        IFormatProvider? provider,
        [MaybeNullWhen(false)] out Token result)
    {
        return TryParse(s.AsSpan(), provider, out result);
    }

    public override string ToString()
    {
        return ToString(default, default);
    }

    public string ToStringInvariant()
    {
        return ToString(default, CultureInfo.InvariantCulture);
    }

    internal int GetCharCount()
    {
        return Words.Sum(w => w.GetCharCount());
    }

    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        var charCount = GetCharCount();

        char[]? rented = null;

        Span<char> buffer = charCount > 128
            ? (rented = ArrayPool<char>.Shared.Rent(charCount))
            : stackalloc char[128];

        try
        {
            if (TryFormat(buffer, out var charsWritten, format, formatProvider))
            {
                return buffer[..charsWritten].ToString();
            }
        }
        finally
        {
            if (rented is not null)
            {
                ArrayPool<char>.Shared.Return(rented);
            }
        }

        ThrowHelper.ThrowFormatException($"Failed to format {nameof(Token)} to string.");
        return null!; // unreachable
    }

    public bool TryFormat(Span<char> destination, out int charsWritten)
    {
        return TryFormat(destination, out charsWritten, default, default);
    }

    public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
    {
        charsWritten = 0;

        // TODO: handle multi-word tokens
        // https://universaldependencies.org/u/overview/tokenization.html
        // https://universaldependencies.org/format.html

        foreach (var word in Words)
        {
            if (word.TryFormat(destination, out var wordChars, format, provider))
            {
                charsWritten += wordChars;
            }
            else
            {
                return false;
            }
        }

        return true;
    }
}
