// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using DSE.Open.Diagnostics;
using DSE.Open.Globalization;
using DSE.Open.Language.Text.Json.Serialization;

namespace DSE.Open.Language;

/// <summary>
/// Associates a linguistic sign with a meaning.
/// </summary>
[JsonConverter(typeof(JsonStringWordMeaningConverter))]
public sealed class WordMeaning : IEquatable<WordMeaning>, ISpanFormattable, ISpanParsable<WordMeaning>, ISpanSerializable<WordMeaning>
{
    private string? _serialized;

    public static int MaxSerializedCharLength => 128;

    /// <summary>
    /// The sign with the meaning.
    /// </summary>
    public required Sign Sign { get; init; }

    /// <summary>
    /// The language in which the word is presented.
    /// </summary>
    public required LanguageTag Language { get; init; }

    /// <summary>
    /// A Universal POS tag for the word used in a context with the intended meaning.
    /// </summary>
    public required UniversalPosTag PosTag { get; init; }

    /// <summary>
    /// A Treebank POS tag for the word used in a context with the intended meaning.
    /// </summary>
    public required TreebankPosTag PosDetailedTag { get; init; }

    /// <summary>
    /// Gets a value that identifies the word meaning.
    /// </summary>
    [JsonIgnore]
    public string Key => ToString();

    public bool Equals(WordMeaning? other)
    {
        if (other is null)
        {
            return false;
        }

        if (Object.ReferenceEquals(this, other))
        {
            return true;
        }

        return Sign == other.Sign
            && Language == other.Language
            && PosTag == other.PosTag
            && PosDetailedTag == other.PosDetailedTag;
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as WordMeaning);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Sign, Language, PosTag, PosDetailedTag);
    }

    public override string ToString()
    {
        return ToString(default, default);
    }

    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        if (_serialized is not null)
        {
            return _serialized;
        }

        Span<char> k = stackalloc char[MaxSerializedCharLength];

        if (TryFormat(k, out var charsWritten))
        {
            _serialized = k[..charsWritten].ToString();

            return _serialized;
        }

        Expect.Unreachable();
        return null!;
    }

    public bool TryFormat(Span<char> destination, out int charsWritten)
    {
        return TryFormat(destination, out charsWritten, default, default);
    }

    public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
    {
        if (Sign.TryFormat(destination, out charsWritten))
        {
            destination[charsWritten++] = '|';

            if (Language.TryFormat(destination[charsWritten..], out var langChars))
            {
                charsWritten += langChars;
                destination[charsWritten++] = '|';

                if (PosTag.TryFormat(destination[charsWritten..], out var posChars))
                {
                    charsWritten += posChars;
                    destination[charsWritten++] = '|';
                }

                if (PosDetailedTag.TryFormat(destination[charsWritten..], out var posDetailedChars))
                {
                    charsWritten += posDetailedChars;
                    return true;
                }
            }
        }

        return false;
    }

    public static WordMeaning Parse(ReadOnlySpan<char> s)
    {
        return Parse(s, default);
    }

    public static WordMeaning Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
    {
        if (TryParse(s, provider, out var wordMeaning))
        {
            return wordMeaning;
        }

        ThrowHelper.ThrowFormatException($"Failed to parse {nameof(wordMeaning)} value: '{s}'");
        return null!; // unreachable
    }

    public static WordMeaning Parse(string s)
    {
        return Parse(s, null);
    }

    public static WordMeaning Parse(string s, IFormatProvider? provider)
    {
        Guard.IsNotNull(s);
        return Parse(s.AsSpan(), provider);
    }

    public static bool TryParse(ReadOnlySpan<char> s, [MaybeNullWhen(false)] out WordMeaning result)
    {
        return TryParse(s, default, out result);
    }

    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, [MaybeNullWhen(false)] out WordMeaning result)
    {
        Span<Range> ranges = stackalloc Range[4];

        var count = s.Split(ranges, '|');

        if (count < 4)
        {
            result = default;
            return false;
        }

        var sign = Sign.Parse(s[ranges[0]], provider);
        var language = LanguageTag.Parse(s[ranges[1]], provider);
        var posTag = UniversalPosTag.Parse(s[ranges[2]], provider);
        var posDetailedTag = TreebankPosTag.Parse(s[ranges[3]], provider);

        result = new WordMeaning
        {
            Sign = sign,
            Language = language,
            PosTag = posTag,
            PosDetailedTag = posDetailedTag,
        };

        return true;
    }

    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out WordMeaning result)
    {
        throw new NotImplementedException();
    }

    public static bool operator ==(WordMeaning? wm1, WordMeaning? wm2)
    {
        if (wm1 is null)
        {
            if (wm2 is null)
            {
                return true;
            }

            return false;
        }

        return wm1.Equals(wm2);
    }

    public static bool operator !=(WordMeaning? wm1, WordMeaning? wm2)
    {
        return !(wm1 == wm2);
    }
}
