// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Buffers;
using System.Text.Json.Serialization;
using DSE.Open.Collections.Generic;
using DSE.Open.Globalization;
using DSE.Open.Hashing;

namespace DSE.Open.Language.Annotations;

/// <summary>
/// A linguistically-annotated sentence, composed of one or more <see cref="Token"/>s.
/// </summary>
public record Sentence : ISpanFormattable, IRepeatableHash64
{
    private ReadOnlyValueCollection<Word>? _words;

    /// <summary>
    /// The 1-based index of the sentence within its document.
    /// </summary>
    [JsonPropertyName("index")]
    public int Index { get; init; } = 1;

    /// <summary>
    /// The optional identifier of the sentence.
    /// </summary>
    [JsonPropertyName("sent_id")]
    public string? Id { get; init; }

    /// <summary>
    /// The optional identifier of the containing document.
    /// </summary>
    [JsonPropertyName("doc_id")]
    public string? DocumentId { get; init; }

    /// <summary>
    /// The optional language of the sentence.
    /// </summary>
    [JsonPropertyName("language")]
    public LanguageTag? Language { get; init; }

    /// <summary>
    /// The text of the sentence.
    /// </summary>
    [JsonPropertyName("text")]
    public required string Text { get; init; }

    /// <summary>
    /// The tokens that make up the sentence.
    /// </summary>
    [JsonPropertyName("tokens")]
    public required ReadOnlyValueCollection<Token> Tokens { get; init; } = [];

    /// <summary>
    /// The flattened sequence of <see cref="Word"/>s drawn from <see cref="Tokens"/>.
    /// </summary>
    [JsonIgnore]
    public ReadOnlyValueCollection<Word> Words => _words ??= [.. Tokens.SelectMany(t => t.Words)];

    /// <summary>
    /// Comment lines associated with the sentence (CoNLL-U <c>#</c> lines).
    /// </summary>
    [JsonPropertyName("comments")]
    public required ReadOnlyValueCollection<string> Comments { get; init; } = [];

    /// <inheritdoc/>
    public override string ToString()
    {
        return ToString(default, default);
    }

    /// <inheritdoc/>
    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        var buffer = ArrayPool<char>.Shared.Rent(GetCharCount());

        try
        {
            if (TryFormat(buffer, out var charsWritten, default, default))
            {
                return new(buffer, 0, charsWritten);
            }
            else
            {
                ThrowHelper.ThrowFormatException("Failed to format sentence.");
                return string.Empty;
            }
        }
        finally
        {
            ArrayPool<char>.Shared.Return(buffer);
        }
    }

    internal int GetCharCount()
    {
        var count = SentIdPrefix.Length
            + (Id?.Length ?? GetInvariantCharCount(Index))
            + 1
            + TextPrefix.Length
            + Text.Length
            + 1
            + 1;

        if (Language is not null)
        {
            count += LangPrefix.Length
                + Language.Value.Length
                + 1;
        }

        foreach (var token in Tokens)
        {
            count += token.GetCharCount() + 1;
        }

        return count;
    }

    private static int GetInvariantCharCount(int value)
    {
        Span<char> buffer = stackalloc char[11];
        _ = value.TryFormat(buffer, out var charsWritten, default, CultureInfo.InvariantCulture);
        return charsWritten;
    }

    private const string SentIdPrefix = "# sent_id = ";
    private const string LangPrefix = "# lang = ";
    private const string TextPrefix = "# text = ";
    private const char NewLine = '\n';

    /// <inheritdoc/>
    public bool TryFormat(
        Span<char> destination,
        out int charsWritten,
        ReadOnlySpan<char> format,
        IFormatProvider? provider)
    {
        charsWritten = 0;

        if (SentIdPrefix.TryCopyTo(destination))
        {
            charsWritten += SentIdPrefix.Length;
        }
        else
        {
            return false;
        }

        if (Id is not null)
        {
            if (Id.TryCopyTo(destination[charsWritten..]))
            {
                charsWritten += Id.Length;
            }
            else
            {
                return false;
            }
        }
        else
        {
            if (Index.TryFormat(destination[charsWritten..], out var idChars, default, CultureInfo.InvariantCulture))
            {
                charsWritten += idChars;
            }
            else
            {
                return false;
            }
        }

        if (destination.Length > charsWritten)
        {
            destination[charsWritten++] = NewLine;
        }
        else
        {
            return false;
        }

        if (Language is not null)
        {
            if (LangPrefix.TryCopyTo(destination[charsWritten..]))
            {
                charsWritten += LangPrefix.Length;
            }
            else
            {
                return false;
            }

            if (Language.Value.TryFormat(destination[charsWritten..], out var langChars, default, CultureInfo.InvariantCulture))
            {
                charsWritten += langChars;
            }
            else
            {
                return false;
            }

            if (destination.Length > charsWritten)
            {
                destination[charsWritten++] = NewLine;
            }
            else
            {
                return false;
            }
        }

        if (TextPrefix.TryCopyTo(destination[charsWritten..]))
        {
            charsWritten += TextPrefix.Length;
        }
        else
        {
            return false;
        }

        if (Text.TryCopyTo(destination[charsWritten..]))
        {
            charsWritten += Text.Length;
        }
        else
        {
            return false;
        }

        if (destination.Length > charsWritten)
        {
            destination[charsWritten++] = NewLine;
        }
        else
        {
            return false;
        }

        foreach (var t in Tokens)
        {
            if (t.TryFormat(destination[charsWritten..], out var tChars, default, CultureInfo.InvariantCulture))
            {
                charsWritten += tChars;
            }
            else
            {
                return false;
            }

            if (destination.Length > charsWritten)
            {
                destination[charsWritten++] = NewLine;
            }
            else
            {
                return false;
            }
        }

        if (destination.Length > charsWritten)
        {
            destination[charsWritten++] = NewLine;
        }
        else
        {
            return false;
        }

        return true;
    }

    /// <inheritdoc/>
    public ulong GetRepeatableHashCode()
    {
        var hash = RepeatableHash64Provider.Default.CombineHashCodes(
            RepeatableHash64Provider.Default.GetRepeatableHashCode(Index),
            RepeatableHash64Provider.Default.GetRepeatableHashCode(Id.AsSpan()),
            RepeatableHash64Provider.Default.GetRepeatableHashCode(DocumentId.AsSpan()),
            Language?.GetRepeatableHashCode() ?? 0u,
            RepeatableHash64Provider.Default.GetRepeatableHashCode(Text.AsSpan())
            );

        foreach (var t in Tokens)
        {
            hash = RepeatableHash64Provider.Default.CombineHashCodes(hash, t.GetRepeatableHashCode());
        }

        // ignore comments

        return hash;
    }

    /// <summary>
    /// Parses a <see cref="Sentence"/> from a CoNLL-U formatted string.
    /// </summary>
    [Obsolete("Use FromConllu instead.")]
    public static Sentence ReadConllu(string conlluDefintion)
    {
        return FromConllu(conlluDefintion);
    }

    /// <summary>
    /// Parses a <see cref="Sentence"/> from a CoNLL-U formatted string.
    /// </summary>
    public static Sentence FromConllu(string conlluDefintion)
    {
        ArgumentNullException.ThrowIfNull(conlluDefintion);

        var lines = conlluDefintion.Split("\n", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        var tokens = ParseTokens(lines);

        var text = lines.FirstOrDefault(l => l.StartsWith("# text = ", StringComparison.OrdinalIgnoreCase
            ))?.Split('=', 2)[1].Trim()
            ?? string.Join(' ', tokens.Select(t => t.Text));

        return new Sentence
        {
            Text = text,
            Tokens = [.. tokens],
            Comments = []
        };
    }

    private static IEnumerable<Token> ParseTokens(string[] lines)
    {
        foreach (var line in lines)
        {
            if (line.StartsWith('#'))
            {
                continue;
            }

            Token token;

            try
            {
                token = Token.ParseInvariant(line);
            }
            catch (FormatException ex)
            {
                throw new FormatException($"Invalid token format: {line}", ex);
            }

            yield return token;
        }
    }
}
