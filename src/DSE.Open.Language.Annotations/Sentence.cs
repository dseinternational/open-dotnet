// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Buffers;
using System.Text.Json.Serialization;
using DSE.Open.Collections.Generic;
using DSE.Open.Globalization;

namespace DSE.Open.Language.Annotations;

public record Sentence : ISpanFormattable
{
    private ReadOnlyValueCollection<Word>? _words;

    [JsonPropertyName("index")]
    public int Index { get; init; } = 1;

    [JsonPropertyName("sent_id")]
    public string? Id { get; init; }

    [JsonPropertyName("doc_id")]
    public string? DocumentId { get; init; }

    [JsonPropertyName("language")]
    public LanguageTag? Language { get; init; }

    [JsonPropertyName("text")]
    public required string Text { get; init; }

    [JsonPropertyName("tokens")]
    public required ReadOnlyValueCollection<Token> Tokens { get; init; } = [];

    [JsonIgnore]
    public ReadOnlyValueCollection<Word> Words => _words ??= [.. Tokens.SelectMany(t => t.Words)];

    [JsonPropertyName("comments")]
    public required ReadOnlyValueCollection<string> Comments { get; init; } = [];

    public override string ToString()
    {
        return ToString(default, default);
    }

    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        var buffer = ArrayPool<char>.Shared.Rent(2048);

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

    private const string SentIdPrefix = "# sent_id = ";
    private const string LangPrefix = "# lang = ";
    private const string TextPrefix = "# text = ";
    private const char NewLine = '\n';

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
}
