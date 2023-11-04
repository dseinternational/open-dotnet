// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Text.Json.Serialization;
using DSE.Open.Diagnostics;
using DSE.Open.Globalization;
using DSE.Open.Language.Text.Json.Serialization;

namespace DSE.Open.Language;

/// <summary>
/// Associates a linguistic sign with a meaning.
/// </summary>
[JsonConverter(typeof(JsonStringSignMeaningConverter))]
public sealed record SignMeaning
    : IEquatable<SignMeaning>,
      ISpanFormattable,
      ISpanParsable<SignMeaning>,
      ISpanSerializable<SignMeaning>
{
    private string? _string;

    public SignMeaning()
    {
    }

    private SignMeaning(SignMeaning original)
    {
        Sign = original.Sign;
        Language = original.Language;
        PosTag = original.PosTag;
        PosDetailedTag = original.PosDetailedTag;
    }

    public static int MaxSerializedCharLength => 128;

    /// <summary>
    /// The sign with the meaning.
    /// </summary>
    public required Sign Sign { get; init; }

    /// <summary>
    /// The language in which the sign is presented.
    /// </summary>
    public required LanguageTag Language { get; init; }

    /// <summary>
    /// A Universal POS tag for the sign used in a context with the intended meaning.
    /// </summary>
    public required UniversalPosTag PosTag { get; init; }

    /// <summary>
    /// A Treebank POS tag for the sign used in a context with the intended meaning.
    /// </summary>
    public required TreebankPosTag PosDetailedTag { get; init; }

    public bool Equals(SignMeaning? other)
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
        if (_string is not null)
        {
            return _string;
        }

        Span<char> k = stackalloc char[MaxSerializedCharLength];

        if (TryFormat(k, out var charsWritten))
        {
            _string = k[..charsWritten].ToString();

            return _string;
        }

        Expect.Unreachable();
        return null!;
    }

    // not currently used, but stops compiler emitting version including key and token
    private bool PrintMembers(StringBuilder builder)
    {
        _ = builder.Append(CultureInfo.InvariantCulture,
            $"{nameof(Sign)} = {Sign}, {nameof(Language)} = {Language}, " +
            $"{nameof(PosTag)} = {PosTag}, {nameof(PosDetailedTag)} = {PosDetailedTag}");

        return true;
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

    public static SignMeaning Parse(ReadOnlySpan<char> s)
    {
        return Parse(s, default);
    }

    public static SignMeaning ParseInvariant(ReadOnlySpan<char> s)
    {
        return Parse(s, CultureInfo.InvariantCulture);
    }

    public static SignMeaning Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
    {
        if (TryParse(s, provider, out var wordMeaning))
        {
            return wordMeaning;
        }

        ThrowHelper.ThrowFormatException($"Failed to parse {nameof(wordMeaning)} value: '{s}'");
        return null!; // unreachable
    }

    public static SignMeaning Parse(string s)
    {
        return Parse(s, null);
    }

    public static SignMeaning ParseInvariant(string s)
    {
        return Parse(s, CultureInfo.InvariantCulture);
    }

    public static SignMeaning Parse(string s, IFormatProvider? provider)
    {
        Guard.IsNotNull(s);
        return Parse(s.AsSpan(), provider);
    }

    public static bool TryParse(
        ReadOnlySpan<char> s,
        [MaybeNullWhen(false)] out SignMeaning result)
    {
        return TryParse(s, default, out result);
    }

    public static bool TryParseInvariant(
        ReadOnlySpan<char> s,
        [MaybeNullWhen(false)] out SignMeaning result)
    {
        return TryParse(s, CultureInfo.InvariantCulture, out result);
    }

    public static bool TryParse(
        ReadOnlySpan<char> s,
        IFormatProvider? provider,
        [MaybeNullWhen(false)] out SignMeaning result)
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

        result = new SignMeaning
        {
            Sign = sign,
            Language = language,
            PosTag = posTag,
            PosDetailedTag = posDetailedTag,
        };

        return true;
    }

    public static bool TryParseInvariant(
        [NotNullWhen(true)] string? s,
        IFormatProvider? provider,
        [MaybeNullWhen(false)] out SignMeaning result)
    {
        return TryParse(s, CultureInfo.InvariantCulture, out result);
    }

    public static bool TryParse(
        [NotNullWhen(true)] string? s,
        IFormatProvider? provider,
        [MaybeNullWhen(false)] out SignMeaning result)
    {
        if (TryParse(s.AsSpan(), provider, out result))
        {
            return true;
        }

        return false;
    }
}
