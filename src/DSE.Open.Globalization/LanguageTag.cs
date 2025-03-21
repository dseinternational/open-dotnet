// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections.Concurrent;
using System.Collections.Frozen;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using CommunityToolkit.HighPerformance.Buffers;
using DSE.Open.Hashing;
using DSE.Open.Runtime.InteropServices;
using DSE.Open.Values;
using DSE.Open.Values.Text.Json.Serialization;

namespace DSE.Open.Globalization;

/// <summary>
/// A language tag as defined by <see href="https://www.rfc-editor.org/rfc/rfc5646.html">RFC5646</see>.
/// </summary>
[ComparableValue]
[JsonConverter(typeof(JsonUtf8SpanSerializableValueConverter<LanguageTag, AsciiString>))]
[StructLayout(LayoutKind.Sequential)]
public readonly partial struct LanguageTag
    : IComparableValue<LanguageTag, AsciiString>,
      IUtf8SpanSerializable<LanguageTag>,
      IRepeatableHash64
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

    public static int MaxSerializedCharLength => MaxLength;

    public static int MaxSerializedByteLength => MaxLength;

    private static readonly Regex s_regex = GetValidationRegex();

    public static bool IsValidValue(AsciiString value)
    {
        return IsValidValue(value.AsSpan());
    }

    public int Length => _value.Length;

    public static LanguageTag FromCultureInfo(CultureInfo cultureInfo)
    {
        ArgumentNullException.ThrowIfNull(cultureInfo);
        return FromString(cultureInfo.Name);
    }

    public static LanguageTag FromString(string languageTag)
    {
        ArgumentNullException.ThrowIfNull(languageTag);
        return new(AsciiString.Parse(languageTag, CultureInfo.InvariantCulture));
    }

    public static LanguageTag FromByteSpan(ReadOnlySpan<byte> languageTag)
    {
        return new(AsciiString.Parse(languageTag, null));
    }

    public static LanguageTag FromCharSpan(ReadOnlySpan<char> languageTag)
    {
        return new(AsciiString.Parse(languageTag, CultureInfo.InvariantCulture));
    }

    public static bool IsValidValue(ReadOnlySpan<AsciiChar> value)
    {
        return IsValidValue(ValuesMarshal.AsBytes(value));
    }

    [SkipLocalsInit]
    public static bool IsValidValue(ReadOnlySpan<byte> value)
    {
        switch (value.Length)
        {
            case < 2 or > MaxLength:
                return false;
            case 5 when value[2] == '-': // Most common case
                return AsciiChar.IsLetter(value[0])
                    && AsciiChar.IsLetter(value[1])
                    && AsciiChar.IsLetter(value[3])
                    && AsciiChar.IsLetter(value[4]);
            case 2: // Two-character primary language subtag only
                return AsciiChar.IsLetter(value[0])
                    && AsciiChar.IsLetter(value[1]);
            case 3: // Three-character primary language subtag only
                return AsciiChar.IsLetter(value[0])
                    && AsciiChar.IsLetter(value[1])
                    && AsciiChar.IsLetter(value[2]);
            default:
                break;
        }

        // Fall back to regex
        Span<char> buffer = stackalloc char[MaxSerializedCharLength];

        for (var i = 0; i < value.Length; i++)
        {
            buffer[i] = (char)value[i];
        }

        return s_regex.IsMatch(buffer[..value.Length]);
    }

    /// <summary>
    /// Determines if the specified language tag is equal to this value. Language tag comparisons are
    /// <see href="https://datatracker.ietf.org/doc/html/rfc5646#section-2.1.1">case insensitive</see>.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Equals(LanguageTag other)
    {
        return _value.EqualsIgnoreCase(other._value);
    }

    /// <summary>
    /// Determines if the specified value is equal to this value. Language tag comparisons are
    /// <see href="https://datatracker.ietf.org/doc/html/rfc5646#section-2.1.1">case insensitive</see>.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Equals(AsciiString other)
    {
        return _value.EqualsIgnoreCase(other);
    }

    /// <summary>
    /// Determines if the specified value is equal to this value. Language tag comparisons are
    /// <see href="https://datatracker.ietf.org/doc/html/rfc5646#section-2.1.1">case insensitive</see>.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Equals(ReadOnlySpan<char> other)
    {
        return _value.EqualsIgnoreCase(other);
    }

    /// <summary>
    /// Determines if the specified value is equal to this value. Language tag comparisons are
    /// <see href="https://datatracker.ietf.org/doc/html/rfc5646#section-2.1.1">case insensitive</see>.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Equals(string other)
    {
        return _value.EqualsIgnoreCase(other);
    }

    public int CompareTo(LanguageTag other)
    {
        return _value.CompareToIgnoreCase(other._value);
    }

    public override int GetHashCode()
    {
        return AsciiStringComparer.IgnoreCase.GetHashCode(_value);
    }

    // we know they will be interned - see IsoCountryCodes
    private static string GetString(ReadOnlySpan<char> s)
    {
        return LanguageTagStringPool.Shared.GetOrAdd(s);
    }

    public bool LanguagePartEquals(LanguageTag otherLangPart)
    {
        return LanguagePartEquals(otherLangPart._value.AsSpan());
    }

    [Obsolete($"Renamed to {nameof(AsAsciiString)}")]
    public AsciiString ToAsciiString()
    {
        return _value;
    }

    /// <summary>
    /// Gets the <see cref="AsciiString"/> representation of this <see cref="LanguageTag"/>.
    /// </summary>
    public AsciiString AsAsciiString()
    {
        return _value;
    }

    public char[] ToCharArray()
    {
        return _value.ToCharArray();
    }

    /// <summary>
    /// Returns the language tag as a string, without any formatting applied.
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return ToString(default, CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Returns the language tag as a string, formatted as recommended in RFC5646.
    /// </summary>
    /// <returns>The language tag as a string, formatted as recommended in RFC5646.</returns>
    public string ToStringFormatted()
    {
        return ToString("N", CultureInfo.InvariantCulture);
    }

    public string ToStringLower()
    {
        return _value.ToStringLower();
    }

    public string ToStringUpper()
    {
        return _value.ToStringUpper();
    }

    public bool TryFormat(
        Span<char> destination,
        out int charsWritten,
        ReadOnlySpan<char> format,
        IFormatProvider? provider)
    {
        if (destination.Length < _value.Length)
        {
            charsWritten = 0;
            return false;
        }

        // No formatting
        if (format.IsEmpty)
        {
            return _value.TryFormat(destination, out charsWritten, format, provider);
        }

        if (format.Length != 1)
        {
            ThrowHelper.ThrowFormatException($"Invalid format string '{format.ToString()}'");
        }

        switch (format[0] | 0x20)
        {
            case 'n':
                FormatNormalized(_value, destination);
                break;
            case 'l' or 'u':
                return _value.TryFormat(destination, out charsWritten, format, provider);
            default:
                ThrowHelper.ThrowFormatException($"Invalid format string '{format.ToString()}'");
                break;
        }

        charsWritten = _value.Length;
        return true;

        static void FormatNormalized(AsciiString value, Span<char> destination)
        {
            /*
               An implementation can reproduce this format without accessing the
               registry as follows.  All subtags, including extension and private
               use subtags, use lowercase letters with two exceptions: two-letter
               and four-letter subtags that neither appear at the start of the tag
               nor occur after singletons.  Such two-letter subtags are all
               uppercase (as in the tags "en-CA-x-ca" or "sgn-BE-FR") and four-
               letter subtags are titlecase (as in the tag "az-Latn-x-latn").
             */

            var valueSpan = value.AsSpan();

            var ti0 = valueSpan.IndexOf((AsciiChar)'-');

            // only language tag - all lowercase and return...

            if (ti0 < 0)
            {
                for (var i = 0; i < value.Length; i++)
                {
                    destination[i] = (char)value[i].ToLower();
                }

                return;
            }

            // language part to lower

            for (var i = 0; i < ti0; i++)
            {
                destination[i] = (char)value[i].ToLower();
            }

            // from now on all subtags to lower except for 2 and 4 letter subtags,
            // unless occuring after singletons

            var remaining = valueSpan[(ti0 + 1)..];
            var previousSingleton = false;
            var destIndex = ti0;

            destination[ti0] = '-';
            destIndex++;

            var lastTag = false;

            while (!remaining.IsEmpty)
            {
                var length = remaining.IndexOf((AsciiChar)'-');

                if (length < 0)
                {
                    length = remaining.Length;
                    lastTag = true;
                }

                if (!previousSingleton && length is 2)
                {
                    for (var j = 0; j < length; j++)
                    {
                        destination[destIndex + j] = (char)remaining[j].ToUpper();
                    }
                }
                else if (!previousSingleton && length is 4)
                {
                    destination[destIndex] = (char)remaining[0].ToUpper();

                    for (var j = 1; j < length; j++)
                    {
                        destination[destIndex + j] = (char)remaining[j].ToLower();
                    }
                }
                else
                {
                    for (var j = 0; j < length; j++)
                    {
                        destination[destIndex + j] = (char)remaining[j].ToLower();
                    }
                }

                destIndex += length;

                if (!lastTag)
                {
                    destination[destIndex] = '-';
                    destIndex++;
                    remaining = remaining[(length + 1)..];
                    previousSingleton = length == 1;
                }
                else
                {
                    Debug.Assert(length == remaining.Length);
                    remaining = default;
                }
            }
        }
    }

    private static readonly ConcurrentDictionary<AsciiString, CultureInfo> s_cultureInfoCache = new();

    /// <summary>
    /// Gets the <see cref="CultureInfo"/> represented by the current value.
    /// </summary>
    /// <returns></returns>
    public CultureInfo GetCultureInfo()
    {
        if (!s_cultureInfoCache.TryGetValue(_value, out var ci))
        {
            ci = CultureInfo.GetCultureInfo(_value.ToString());
            _ = s_cultureInfoCache.TryAdd(_value, ci);
        }

        return ci;
    }

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
            && span[..index].SequenceEqualsIgnoreCase(otherLangPart);
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

    public LanguageTag GetLanguagePart()
    {
        return new(new(GetLanguagePartSpan().ToArray()));
    }

    [GeneratedRegex(
        "^((?:(en-GB-oed|i-ami|i-bnn|i-default|i-enochian|i-hak|i-klingon|i-lux|i-mingo|i-navajo|i-pwn|i-tao|i-tay|i-tsu|sgn-BE-FR|sgn-BE-NL|sgn-CH-DE)|(art-lojban|cel-gaulish|no-bok|no-nyn|zh-guoyu|zh-hakka|zh-min|zh-min-nan|zh-xiang))|((?:([A-Za-z]{2,3}(-(?:[A-Za-z]{3}(-[A-Za-z]{3}){0,2}))?)|[A-Za-z]{4}|[A-Za-z]{5,8})(-(?:[A-Za-z]{4}))?(-(?:[A-Za-z]{2}|[0-9]{3}))?(-(?:[A-Za-z0-9]{5,8}|[0-9][A-Za-z0-9]{3}))*(-(?:[0-9A-WY-Za-wy-z](-[A-Za-z0-9]{2,8})+))*(-(?:x(-[A-Za-z0-9]{1,8})+))?)|(?:x(-[A-Za-z0-9]{1,8})+))$",
        RegexOptions.ExplicitCapture | RegexOptions.IgnoreCase,
        250
    )]
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

    public ulong GetRepeatableHashCode()
    {
        return RepeatableHash64Provider.Default.GetRepeatableHashCode(_value);
    }

    private static readonly FrozenDictionary<CountryCode, LanguageTag> s_languageLookup = new Dictionary<CountryCode, LanguageTag>
    {
        { CountryCode.Australia, EnglishAustralia },
        { CountryCode.Brazil, Parse("pt-BR", CultureInfo.InvariantCulture) },
        { CountryCode.Canada, EnglishCanada },
        { CountryCode.China, Parse("zh-CN", CultureInfo.InvariantCulture) },
        { CountryCode.France, Parse("fr-FR", CultureInfo.InvariantCulture) },
        { CountryCode.Germany, Parse("de-DE", CultureInfo.InvariantCulture) },
        { CountryCode.India, EnglishIndia },
        { CountryCode.Ireland, EnglishIreland },
        { CountryCode.Italy, Parse("it-IT", CultureInfo.InvariantCulture) },
        { CountryCode.Mexico, Parse("es-MX", CultureInfo.InvariantCulture) },
        { CountryCode.NewZealand, EnglishNewZealand },
        { CountryCode.Poland, Parse("pl-PL", CultureInfo.InvariantCulture) },
        { CountryCode.SouthAfrica, EnglishSouthAfrica },
        { CountryCode.Spain, Parse("es-ES", CultureInfo.InvariantCulture) },
        { CountryCode.UnitedKingdom, EnglishUk },
        { CountryCode.UnitedStates, EnglishUs },

    }.ToFrozenDictionary();
}
