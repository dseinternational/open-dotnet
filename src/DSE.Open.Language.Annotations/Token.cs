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
    private static readonly TokenIndex s_one = new(1);
    /// <summary>
    /// Index of the token in the sentence.
    /// </summary>
    /// <remarks>Word index, integer starting at 1 for each new sentence; may be a range for
    /// multiword tokens; may be a decimal number for empty nodes (decimal numbers can be
    /// lower than 1 but must be greater than 0).</remarks>
    [JsonPropertyName("id")]
    public TokenIndex Index
    {
        get
        {
            if (Words.Count == 0)
            {
                return s_one;
            }

            if (Words.Count == 1)
            {
                return new TokenIndex(Words[0].Index);
            }

            return new TokenIndex(Words[0].Index, Words[^1].Index);
        }
    }

    [JsonPropertyName("text")]
    public required TokenText Text { get; init; }

    /// <summary>
    /// Gets or sets misc annotations/attributes associated with the token.
    /// </summary>
    [JsonPropertyName("misc")]
    public ReadOnlyAttributeValueCollection Attributes { get; init; } = [];

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
        var multiword = false;
        var lineIndex = 0;

        (TokenIndex Index, TokenText Text, ReadOnlyAttributeValueCollection Attributes) tokenData = default;

        foreach (var line in s.EnumerateLines())
        {
            var ci = 0;
            var token = false;

            if (lineIndex == 0)
            {
                while (ci < line.Length)
                {
                    if (char.IsAsciiDigit(line[ci]))
                    {
                        ci++;
                        continue;
                    }

                    if (line[ci] == '-')
                    {
                        if (token)
                        {
                            // >1 "-" in line
                            result = default;
                            return false;
                        }

                        token = true;
                        ci++;
                        continue;
                    }

                    if (line[ci] == '\t')
                    {
                        if (token)
                        {
                            multiword = true;
                        }
                        break;
                    }

                    // invalid character
                    result = default;
                    return false;
                }
            }

            if (multiword && lineIndex == 0)
            {
#pragma warning disable CA2014 // Do not use stackalloc in loops [only used once]
                Span<Range> fields = stackalloc Range[10];
#pragma warning restore CA2014 // Do not use stackalloc in loops

                var count = line.Split(fields, '\t', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

                if (count != 10)
                {
                    result = default;
                    return false;
                }

                if (TokenIndex.TryParse(s[fields[ConlluFieldIndex.Id]], provider, out var id))
                {
                    tokenData.Index = id;
                }
                else
                {
                    result = default;
                    return false;
                }

                if (TokenText.TryParse(s[fields[ConlluFieldIndex.Form]], provider, out var word))
                {
                    tokenData.Text = word;
                }
                else
                {
                    result = default;
                    return false;
                }

                var miscSpan = s[fields[ConlluFieldIndex.Misc]];

                if (miscSpan.Length == 1 && miscSpan[0] == '_')
                {
                    tokenData.Attributes = [];
                }
                else
                {
                    if (ReadOnlyAttributeValueCollection.TryParse(miscSpan, provider, out var miscValue))
                    {
                        tokenData.Attributes = miscValue;
                    }
                    else
                    {
                        result = default;
                        return false;
                    }
                }
            }
            else
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

            lineIndex++;
        }

        if (words.Count > 0)
        {
            if (words.Count == 1)
            {
                result = new Token
                {
                    Text = words[0].Form,
                    Words = [.. words]
                };
                return true;
            }
            else
            {
                result = new Token
                {
                    Text = tokenData.Text,
                    Words = [.. words],
                    Attributes = tokenData.Attributes!,
                };

                if (result.Index != tokenData.Index)
                {
                    result = default;
                    return false;
                }

                return true;
            }
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
        if (Words.Count == 1)
        {
            return Words[0].GetCharCount();
        }

        return Words.Sum(w => w.GetCharCount())
            + 6                         // "10-11\t"
            + Text.Length
            + 18                        // 9 x "\t_"
            + (Attributes.Count * 16)
            + 2;                        // "_\n"
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

    public bool TryFormat(
        Span<char> destination,
        out int charsWritten,
        ReadOnlySpan<char> format,
        IFormatProvider? provider)
    {
        charsWritten = 0;

        if (Words.Count == 1)
        {
            if (Words[0].TryFormat(destination, out var wordChars, format, provider))
            {
                charsWritten += wordChars;
            }
            else
            {
                return false;
            }

            return true;
        }

        // TODO: handle multi-word tokens
        // https://universaldependencies.org/u/overview/tokenization.html
        // https://universaldependencies.org/format.html

        // Multiword tokens are indexed with integer ranges like 1-2 or 3-5. Lines representing such
        // tokens are inserted before the first word in the range. These ranges must be nonempty and
        // must not overlap. They have a FORM value – the string that occurs in the sentence – but have
        // an underscore in all the remaining fields except MISC (because the token represents multiple
        // words, each with its own lemma, part-of-speech tag, syntactic head, and so on).

        if (Index.TryFormat(destination, out var indexChars, default, default))
        {
            charsWritten += indexChars;
        }
        else
        {
            return false;
        }

        if (destination.Length > charsWritten)
        {
            destination[charsWritten++] = '\t';
        }
        else
        {
            return false;
        }

        if (Text.TryFormat(destination[charsWritten..], out var textChars, format, provider))
        {
            charsWritten += textChars;
        }
        else
        {
            return false;
        }

        const string EmptyFields = "\t_\t_\t_\t_\t_\t_\t_\t";

        if (EmptyFields.TryCopyTo(destination[charsWritten..]))
        {
            charsWritten += EmptyFields.Length;
        }
        else
        {
            return false;
        }

        // TODO: MISC

        if (destination.Length > charsWritten)
        {
            destination[charsWritten++] = '_';
        }
        else
        {
            return false;
        }

        if (destination.Length > charsWritten)
        {
            destination[charsWritten++] = '\n';
        }
        else
        {
            return false;
        }

        for (var i = 0; i < Words.Count; i++)
        {
            var word = Words[i];

            if (word.TryFormat(destination[charsWritten..], out var wordChars, format, provider))
            {
                charsWritten += wordChars;

                if (i < Words.Count - 1)
                {
                    if (destination.Length > charsWritten)
                    {
                        destination[charsWritten++] = '\n';
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            else
            {
                return false;
            }
        }

        return true;
    }
}
