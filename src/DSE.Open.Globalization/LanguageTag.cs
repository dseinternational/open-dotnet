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
[JsonConverter(typeof(JsonSpanSerializableValueConverter<LanguageTag, AsciiString>))]
[StructLayout(LayoutKind.Auto)]
public readonly partial struct LanguageTag
    : IComparableValue<LanguageTag, AsciiString>
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

    public static bool IsValidValue(AsciiString value) => IsValidValue(value.AsSpan());

    public static LanguageTag FromCultureInfo(CultureInfo cultureInfo)
    {
        Guard.IsNotNull(cultureInfo);
        return FromString(cultureInfo.Name);
    }

    public static LanguageTag FromString(string languageTag)
    {
        Guard.IsNotNull(languageTag);
        return new(AsciiString.Parse(languageTag));
    }

    public static LanguageTag FromByteSpan(ReadOnlySpan<byte> languageTag)
        => new(new AsciiString(MemoryMarshal.Cast<byte, AsciiChar>(languageTag)));

    public static LanguageTag FromCharSpan(ReadOnlySpan<char> languageTag)
        => new(AsciiString.Parse(languageTag));

    public static bool IsValidValue(ReadOnlySpan<AsciiChar> value)
        => IsValidValue(MemoryMarshal.Cast<AsciiChar, byte>(value));

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

    public int CompareTo(LanguageTag other) => _value.CompareToCaseInsensitive(other._value);

    public override int GetHashCode() => AsciiStringComparer.CaseInsensitive.GetHashCode(_value);

    private static string GetString(string s)
        => string.IsInterned(s) ?? LanguageTagStringPool.Shared.GetOrAdd(s);

    public bool LanguagePartEquals(LanguageTag otherLangPart)
        => LanguagePartEquals(otherLangPart._value.AsSpan());

    public AsciiString ToAsciiString() => _value;

    public char[] ToCharArray() => _value.ToCharArray();

    /// <summary>
    /// Gets the <see cref="CultureInfo"/> represented by the current value.
    /// </summary>
    /// <returns></returns>
    public CultureInfo GetCultureInfo() => CultureInfo.GetCultureInfo(_value.ToString());

    /// <summary>
    /// Gets the <see cref="LanguageTag"/> for <see cref="CultureInfo.CurrentCulture"/>.
    /// </summary>
    public static LanguageTag CurrentCulture => FromCultureInfo(CultureInfo.CurrentCulture);

    /// <summary>
    /// Gets the <see cref="LanguageTag"/> for <see cref="CultureInfo.CurrentUICulture"/>.
    /// </summary>
    public static LanguageTag CurrentUICulture => FromCultureInfo(CultureInfo.CurrentUICulture);

    public bool LanguagePartEquals(ReadOnlySpan<AsciiChar> otherLangPart)
    {
        if (_value.IsEmpty)
        {
            return otherLangPart.IsEmpty;
        }

        var span = _value.AsSpan();
        var index = span.IndexOf((AsciiChar)'-');

        return otherLangPart.Length == index - 1
            && AsciiString.SequenceEqualsCaseInsenstive(span[..index], otherLangPart);
    }

    public ReadOnlySpan<AsciiChar> GetLanguagePartSpan()
    {
        if (_value.IsEmpty)
        {
            return default;
        }

        var span = _value.AsSpan();
        var index = span.IndexOf((AsciiChar)'-');
        return index < 0 ? span : span[..index];
    }

    public LanguageTag GetLanguagePart() => new(new AsciiString(GetLanguagePartSpan()));

    [GeneratedRegex("^((?:(en-GB-oed|i-ami|i-bnn|i-default|i-enochian|i-hak|i-klingon|i-lux|i-mingo|i-navajo|i-pwn|i-tao|i-tay|i-tsu|sgn-BE-FR|sgn-BE-NL|sgn-CH-DE)|(art-lojban|cel-gaulish|no-bok|no-nyn|zh-guoyu|zh-hakka|zh-min|zh-min-nan|zh-xiang))|((?:([A-Za-z]{2,3}(-(?:[A-Za-z]{3}(-[A-Za-z]{3}){0,2}))?)|[A-Za-z]{4}|[A-Za-z]{5,8})(-(?:[A-Za-z]{4}))?(-(?:[A-Za-z]{2}|[0-9]{3}))?(-(?:[A-Za-z0-9]{5,8}|[0-9][A-Za-z0-9]{3}))*(-(?:[0-9A-WY-Za-wy-z](-[A-Za-z0-9]{2,8})+))*(-(?:x(-[A-Za-z0-9]{1,8})+))?)|(?:x(-[A-Za-z0-9]{1,8})+))$", RegexOptions.Compiled)]
    private static partial Regex GetValidationRegex();

    /// <summary>
    /// A shared pool for caching strings created by <see cref="LanguageTag"/>
    /// </summary>
    private static class LanguageTagStringPool
    {
        public static readonly StringPool Shared = new(128);
    }

    public static readonly LanguageTag English = FromValue((AsciiString)"en");

    public static readonly LanguageTag EnglishUk = FromValue((AsciiString)"en-GB");

    public static readonly LanguageTag EnglishUs = FromValue((AsciiString)"en-US");

    public static readonly LanguageTag EnglishAustralia = FromValue((AsciiString)"en-AU");

    public static readonly LanguageTag EnglishCanada = FromValue((AsciiString)"en-CA");

    public static readonly LanguageTag EnglishIndia = FromValue((AsciiString)"en-IN");

    public static readonly LanguageTag EnglishIreland = FromValue((AsciiString)"en-IE");

    public static readonly LanguageTag EnglishNewZealand = FromValue((AsciiString)"en-NZ");

    public static readonly LanguageTag EnglishSouthAfrica = FromValue((AsciiString)"en-ZA");

    public static LanguageTag GetDefaultForCountry(CountryCode countryCode)
    {
        if (s_languageLookup.TryGetValue(countryCode, out var language))
        {
            return language;
        }

        var match = CultureInfo.GetCultures(CultureTypes.SpecificCultures)
            .FirstOrDefault(ci => ci.Name.EndsWith(countryCode.ToStringInvariant(), StringComparison.OrdinalIgnoreCase));

        return match is not null ? FromCultureInfo(match) : EnglishUs;
    }

    private static readonly Dictionary<CountryCode, LanguageTag> s_languageLookup = new()
    {
        { CountryCode.Australia, EnglishAustralia },
        { CountryCode.Brazil, Parse("pt-BR") },
        { CountryCode.Canada, EnglishCanada },
        { CountryCode.China, Parse("zh-CN") },
        { CountryCode.France, Parse("fr-FR") },
        { CountryCode.Germany, Parse("de-DE") },
        { CountryCode.India, EnglishIndia },
        { CountryCode.Ireland, EnglishIreland },
        { CountryCode.Italy, Parse("it-IT") },
        { CountryCode.Mexico, Parse("es-MX") },
        { CountryCode.NewZealand, EnglishNewZealand },
        { CountryCode.Poland, Parse("pl-PL") },
        { CountryCode.SouthAfrica, EnglishSouthAfrica },
        { CountryCode.Spain, Parse("es-ES") },
        { CountryCode.UnitedKingdom, EnglishUk },
        { CountryCode.UnitedStates, EnglishUs },
    };
}
