// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Globalization;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using CommunityToolkit.HighPerformance;
using CommunityToolkit.HighPerformance.Buffers;
using DSE.Open.Values;
using DSE.Open.Values.Text.Json.Serialization;

namespace DSE.Open.Globalization;

// TODO

/// <summary>
/// A language tag as defined by <see href="https://www.rfc-editor.org/rfc/rfc5646.html">RFC5646</see>.
/// </summary>
[ComparableValue]
[JsonConverter(typeof(JsonSpanSerializableValueConverter<LanguageTag, AsciiCharSequence>))]
[StructLayout(LayoutKind.Auto)]
public readonly partial struct LanguageTag
    : IComparableValue<LanguageTag, AsciiCharSequence>
{
    /// <summary>
    /// Gets the maximum practical length to expect for a language code that is suitable for
    /// allocating limited size buffers.
    /// </summary>
    /// <remarks>
    /// "Protocols or specifications that specify limited buffer sizes for language tags MUST
    /// allow for language tags of at least 35 characters."
    /// (<see href="https://www.rfc-editor.org/rfc/rfc5646.html#section-4.4.1"/>)
    /// </remarks>
    public const int MaxLength = 35;

    static int ISpanSerializable<LanguageTag>.MaxSerializedCharLength { get; } = MaxLength;

    private static readonly Regex s_regex = GetValidationRegex();

    public static bool IsValidValue(AsciiCharSequence value) => IsValidValue(value.AsSpan());

    public static LanguageTag FromCultureInfo(CultureInfo cultureInfo)
    {
        Guard.IsNotNull(cultureInfo);
        return FromString(cultureInfo.Name);
    }

    public static LanguageTag FromString(string languageTag)
    {
        Guard.IsNotNull(languageTag);
        return new(AsciiCharSequence.Parse(languageTag));
    }

    public static LanguageTag FromByteSpan(ReadOnlySpan<byte> languageTag) => new(new AsciiCharSequence(languageTag));

    public static LanguageTag FromCharSpan(ReadOnlySpan<char> languageTag) => new(AsciiCharSequence.Parse(languageTag));

    public static bool IsValidValue(ReadOnlySpan<byte> value)
    {
        if (value.Length is < 2 or > MaxLength)
        {
            return false;
        }

        // Most common case
        if (value.Length == 5 && value[2] == '-')
        {
            return AsciiChar.IsLetter(value[0])
                && AsciiChar.IsLetter(value[1])
                && AsciiChar.IsLetter(value[3])
                && AsciiChar.IsLetter(value[4]);
        }

        // Two-character primary language subtag only
        if (value.Length == 2)
        {
            return AsciiChar.IsLetter(value[0])
                && AsciiChar.IsLetter(value[1]);
        }

        // Three-character primary language subtag only
        if (value.Length == 3)
        {
            return AsciiChar.IsLetter(value[0])
                && AsciiChar.IsLetter(value[1])
                && AsciiChar.IsLetter(value[2]);
        }

        // Fall back to regex

        Span<char> buffer = stackalloc char[value.Length];
        for (var i = 0; i < value.Length; i++)
        {
            buffer[i] = (char)value[i];
        }

        var chars = buffer;

        return s_regex.IsMatch(chars);
    }

    public bool Equals(LanguageTag other) => _value.EqualsCaseInsensitive(other._value);

    public int CompareTo(LanguageTag other)=>_value.CompareToCaseInsensitive(other._value);


    public override int GetHashCode()
    {
        return AsciiCharSequenceComparer.CaseInsensitive.GetHashCode(_value);
    }

    private static string GetString(string s)
        => string.IsInterned(s) ?? LanguageTagStringPool.Shared.GetOrAdd(s);

    public bool LanguagePartEquals(LanguageTag otherLangPart)
        => LanguagePartEquals(otherLangPart._value.AsSpan());

    public char[] ToCharArray() => _value.ToCharArray();

    public bool LanguagePartEquals(ReadOnlySpan<byte> otherLangPart)
    {
        if (_value.IsEmpty)
        {
            return otherLangPart.IsEmpty;
        }

        var span = _value.AsSpan();
        var index = span.IndexOf((byte)'-');

        return otherLangPart.Length == index - 1
            && AsciiChar.SequenceEqualsCaseInsenstive(span[..index], otherLangPart);
    }

    public ReadOnlySpan<byte> GetLanguagePartSpan()
    {
        if (_value.IsEmpty)
        {
            return default;
        }

        var span = _value.AsSpan();
        var index = span.IndexOf((byte)'-');
        return index < 0 ? span : span[..index];
    }

public LanguageTag GetLanguagePart() {
    // ensure initialized
    return new(new AsciiCharSequence(GetLanguagePartSpan()));
}

    [GeneratedRegex("^((?:(en-GB-oed|i-ami|i-bnn|i-default|i-enochian|i-hak|i-klingon|i-lux|i-mingo|i-navajo|i-pwn|i-tao|i-tay|i-tsu|sgn-BE-FR|sgn-BE-NL|sgn-CH-DE)|(art-lojban|cel-gaulish|no-bok|no-nyn|zh-guoyu|zh-hakka|zh-min|zh-min-nan|zh-xiang))|((?:([A-Za-z]{2,3}(-(?:[A-Za-z]{3}(-[A-Za-z]{3}){0,2}))?)|[A-Za-z]{4}|[A-Za-z]{5,8})(-(?:[A-Za-z]{4}))?(-(?:[A-Za-z]{2}|[0-9]{3}))?(-(?:[A-Za-z0-9]{5,8}|[0-9][A-Za-z0-9]{3}))*(-(?:[0-9A-WY-Za-wy-z](-[A-Za-z0-9]{2,8})+))*(-(?:x(-[A-Za-z0-9]{1,8})+))?)|(?:x(-[A-Za-z0-9]{1,8})+))$", RegexOptions.Compiled)]
    private static partial Regex GetValidationRegex();

    /// <summary>
    /// A shared pool for caching strings created by <see cref="LanguageTag"/>
    /// </summary>
    private static class LanguageTagStringPool
    {
        public static readonly StringPool Shared = new(128);
    }

    public static readonly LanguageTag English = FromValue((AsciiCharSequence)"en");

    public static readonly LanguageTag EnglishUk = FromValue((AsciiCharSequence)"en-GB");

    public static readonly LanguageTag EnglishUs = FromValue((AsciiCharSequence)"en-US");

    public static readonly LanguageTag EnglishAustralia = FromValue((AsciiCharSequence)"en-AU");

    public static readonly LanguageTag EnglishCanada = FromValue((AsciiCharSequence)"en-CA");

    public static readonly LanguageTag EnglishIndia = FromValue((AsciiCharSequence)"en-IN");

    public static readonly LanguageTag EnglishIreland = FromValue((AsciiCharSequence)"en-IE");

    public static readonly LanguageTag EnglishNewZealand = FromValue((AsciiCharSequence)"en-NZ");

    public static readonly LanguageTag EnglishSouthAfrica = FromValue((AsciiCharSequence)"en-ZA");

}
