// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json.Serialization;
using DSE.Open.Diagnostics;
using DSE.Open.Globalization;

namespace DSE.Open.Language;

/// <summary>
/// Associates a linguistic sign with a meaning.
/// </summary>
public sealed class WordMeaning : IEquatable<WordMeaning>
{
    private string? _key;

    /// <summary>
    /// The sign with the meaning.
    /// </summary>
    [JsonPropertyName("sign")]
    [JsonPropertyOrder(-10000)]
    public required Sign Sign { get; init; }

    /// <summary>
    /// The language in which the word is presented.
    /// </summary>
    [JsonPropertyName("language")]
    [JsonPropertyOrder(-8900)]
    public required LanguageTag Language { get; init; }

    /// <summary>
    /// A Universal POS tag for the word used in a context with the intended meaning.
    /// </summary>
    [JsonPropertyName("universal_pos_tag")]
    [JsonPropertyOrder(-8850)]
    public required UniversalPosTag PosTag { get; init; }

    /// <summary>
    /// A Treebank POS tag for the word used in a context with the intended meaning.
    /// </summary>
    [JsonPropertyName("treebank_pos_tag")]
    [JsonPropertyOrder(-8840)]
    public required TreebankPosTag PosDetailedTag { get; init; }

    /// <summary>
    /// Gets a value that identifies the word meaning.
    /// </summary>
    [JsonIgnore]
    public string Key
    {
        get
        {
            if (_key is not null)
            {
                return _key;
            }

            Span<char> k = stackalloc char[128];

            if (Sign.TryFormat(k, out var charsWritten))
            {
                k[charsWritten++] = '|';

                if (Language.TryFormat(k[charsWritten..], out var langChars))
                {
                    charsWritten += langChars;
                    k[charsWritten++] = '|';

                    if (PosTag.TryFormat(k[charsWritten..], out var posChars))
                    {
                        charsWritten += posChars;
                        k[charsWritten++] = '|';
                    }

                    if (PosDetailedTag.TryFormat(k[charsWritten..], out var posDetailedChars))
                    {
                        charsWritten += posDetailedChars;

                        _key = k[..charsWritten].ToString();

                        return _key;
                    }
                }
            }

            Expect.Unreachable();
            return null!;
        }
    }

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
        return Key;
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
