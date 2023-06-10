// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using DSE.Open.Diagnostics;
using DSE.Open.Globalization.Text.Json;

namespace DSE.Open.Globalization;

/// <summary>
/// The ISO 3166-1 alpha-2 code for a country.
/// </summary>
[JsonConverter(typeof(JsonStringCountryCodeConverter))]
[StructLayout(LayoutKind.Auto)]
[SuppressMessage("Design", "CA1036:Override methods on comparable types", Justification = "Not necessary")]
public readonly struct CountryCode : IComparable, IComparable<CountryCode>, IEquatable<CountryCode>,
    ISpanParsable<CountryCode>, ISpanFormattable, IEquatable<string>, IEquatable<ReadOnlyMemory<char>>
{
    private readonly char _c0;
    private readonly char _c1;

    public const int Length = 2;

    public static readonly CountryCode Empty;

    public static readonly CountryCode Andorra = new('A', 'D');
    public static readonly CountryCode UnitedArabEmirates = new('A', 'E');
    public static readonly CountryCode Austria = new('A', 'T');
    public static readonly CountryCode Australia = new('A', 'U');
    public static readonly CountryCode Belgium = new('B', 'E');
    public static readonly CountryCode Brazil = new('B', 'R');
    public static readonly CountryCode Bulgaria = new('B', 'G');
    public static readonly CountryCode Canada = new('C', 'A');
    public static readonly CountryCode Switzerland = new('C', 'H');
    public static readonly CountryCode China = new('C', 'N');
    public static readonly CountryCode Cyprus = new('C', 'Y');
    public static readonly CountryCode CzechRepublic = new('C', 'Z');
    public static readonly CountryCode Germany = new('D', 'E');
    public static readonly CountryCode Denmark = new('D', 'K');
    public static readonly CountryCode Estonia = new('E', 'E');
    public static readonly CountryCode Spain = new('E', 'S');
    public static readonly CountryCode Finland = new('F', 'I');
    public static readonly CountryCode France = new('F', 'R');
    public static readonly CountryCode UnitedKingdom = new('G', 'B');
    public static readonly CountryCode Gibraltar = new('G', 'I');
    public static readonly CountryCode Greece = new('G', 'R');
    public static readonly CountryCode HongKongSAR = new('H', 'K');
    public static readonly CountryCode Croatia = new('H', 'R');
    public static readonly CountryCode Hungary = new('H', 'U');
    public static readonly CountryCode Ireland = new('I', 'E');
    public static readonly CountryCode Israel = new('I', 'L');
    public static readonly CountryCode IsleOfMan = new('I', 'M');
    public static readonly CountryCode India = new('I', 'N');
    public static readonly CountryCode Italy = new('I', 'T');
    public static readonly CountryCode Jersey = new('J', 'E');
    public static readonly CountryCode Japan = new('J', 'P');
    public static readonly CountryCode Liechtenstein = new('L', 'I');
    public static readonly CountryCode Lithuania = new('L', 'T');
    public static readonly CountryCode Luxembourg = new('L', 'U');
    public static readonly CountryCode Latvia = new('L', 'V');
    public static readonly CountryCode Monaco = new('M', 'C');
    public static readonly CountryCode Montenegro = new('M', 'E');
    public static readonly CountryCode Malta = new('M', 'T');
    public static readonly CountryCode Mexico = new('M', 'X');
    public static readonly CountryCode Netherlands = new('N', 'L');
    public static readonly CountryCode Norway = new('N', 'O');
    public static readonly CountryCode NewZealand = new('N', 'Z');
    public static readonly CountryCode Poland = new('P', 'L');
    public static readonly CountryCode Portugal = new('P', 'T');
    public static readonly CountryCode Romania = new('R', 'O');
    public static readonly CountryCode Serbia = new('R', 'S');
    public static readonly CountryCode Russia = new('R', 'U');
    public static readonly CountryCode SaudiArabia = new('S', 'A');
    public static readonly CountryCode Sweden = new('S', 'E');
    public static readonly CountryCode Singapore = new('S', 'G');
    public static readonly CountryCode Slovenia = new('S', 'I');
    public static readonly CountryCode Slovakia = new('S', 'K');
    public static readonly CountryCode SanMarino = new('S', 'M');
    public static readonly CountryCode Turkey = new('T', 'R');
    public static readonly CountryCode Ukraine = new('U', 'A');
    public static readonly CountryCode UnitedStates = new('U', 'S');
    public static readonly CountryCode Kosovo = new('X', 'K');
    public static readonly CountryCode SouthAfrica = new('Z', 'A');

    /// <summary>
    /// Gets the <see cref="CountryCode" /> for an unknown/unidentified country.
    /// </summary>
    public static readonly CountryCode Unknown = new('Z', 'Z');

    private CountryCode(char c0, char c1)
    {
        _c0 = c0;
        _c1 = c1;
    }

    public static CountryCode Parse(string s) => Parse(s, null);

    public static CountryCode Parse(string s, IFormatProvider? provider)
    {
        Guard.IsNotNull(s);
        return Parse(s.AsSpan(), provider);
    }

    public static CountryCode Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
    {
        if (!TryParse(s, provider, out var result))
        {
            ThrowHelper.ThrowFormatException(
                $"'{s.ToString()} is not a valid ISO 3166-1 alpha-2 country code.");
        }

        return result;
    }

    public static bool TryParse(string? s, out CountryCode code) => TryParse(s, null, out code);

    public static bool TryParse(string? s, IFormatProvider? provider, out CountryCode result)
    {
        if (s is null)
        {
            result = default;
            return false;
        }

        return TryParse(s.AsSpan(), provider, out result);
    }

    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out CountryCode result)
    {
        if (s.IsEmpty)
        {
            result = default;
            return true;
        }

        if (s.Length != Length || !IsValidCode(s))
        {
            result = Unknown;
            return false;
        }

        result = new CountryCode(char.ToUpperInvariant(s[0]), char.ToUpperInvariant(s[1]));
        return true;
    }

    public static CountryCode FromRegionInfo(RegionInfo regionInfo)
        => regionInfo is null
        ? throw new ArgumentNullException(nameof(regionInfo))
        : Parse(regionInfo.TwoLetterISORegionName);

    public override bool Equals(object? obj) => obj is CountryCode code && Equals(code);

    public bool Equals(CountryCode other) => _c0 == other._c0 && _c1 == other._c1;

    public bool Equals(string? other) => Equals(other.AsSpan());

    public bool Equals(ReadOnlyMemory<char> other) => Equals(other.Span);

    public bool Equals(ReadOnlySpan<char> other) => other.Length == 2 && _c0 == other[0] && _c1 == other[1];

    public override int GetHashCode() => HashCode.Combine(_c0, _c1);

    int IComparable.CompareTo(object? obj)
    {
        return obj switch
        {
            null => 1,
            CountryCode code => CompareTo(code),
            _ => throw new ArgumentException("Object must be an Alpha2RegionCode")
        };
    }

    public int CompareTo(CountryCode obj) => Compare(this, obj);

    public static int Compare(CountryCode code1, CountryCode code2)
        => code1._c0 == code2._c0
        ? code1._c1.CompareTo(code2._c1)
        : code1._c0.CompareTo(code2._c0);

    public override string ToString() => ToString(null, null);

    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        Span<char> destination = stackalloc char[Length];
        _ = TryFormat(destination, out _, format, formatProvider);
        return CodeStringPool.Shared.GetOrAdd(destination);
    }

    public string ToStringLowerInvariant()
    {
        Span<char> destination = stackalloc char[Length];
        if (!TryFormatLowerInvariant(destination, out var charsWritten))
        {
            UnexpectedConditionException.Throw($"TryFormatLowerInvariant should not fail for {nameof(CountryCode)}");
        }

        return destination[..charsWritten].ToString();
    }

    public bool TryFormat(
        Span<char> destination,
        out int charsWritten,
        ReadOnlySpan<char> format,
        IFormatProvider? provider)
    {
        if (destination.Length < Length)
        {
            charsWritten = 0;
            return false;
        }

        destination[0] = _c0;
        destination[1] = _c1;
        charsWritten = Length;
        return true;
    }

    public bool TryFormatLowerInvariant(
        Span<char> destination,
        out int charsWritten)
    {
        if (destination.Length < Length)
        {
            charsWritten = 0;
            return false;
        }

        destination[0] = char.ToLowerInvariant(_c0);
        destination[1] = char.ToLowerInvariant(_c1);
        charsWritten = Length;
        return true;
    }

    public ReadOnlySpan<char> AsSpan() => new[] { _c0, _c1 };

    public static bool IsValidCode(ReadOnlySpan<char> code)
    {
        if (code.Length != Length)
        {
            return false;
        }

        // case insensitive
        var cc = new CountryCode(char.ToUpperInvariant(code[0]), char.ToUpperInvariant(code[1]));

        var i = s_isoCountryCodes.BinarySearch(cc);

        return i >= 0;
    }

    public static bool IsValidCode(string? code) => code is not null && IsValidCode(code.AsSpan());

    internal static int Encode(CountryCode code) => Encode(code._c0, code._c1);

    internal static int Encode(char c0, char c1) => (c0 << 8) | c1;

    internal static CountryCode Decode(int value)
    {
        var c0 = Convert.ToChar((value >> 8) & 0xFF);
        var c1 = Convert.ToChar(value & 0xFF);
        return new CountryCode(c0, c1);
    }

    public static explicit operator CountryCode(string code) => Parse(code);

    public static explicit operator string(CountryCode code) => code.ToString();

    public static bool Equals(CountryCode value1, CountryCode value2) => Compare(value1, value2) == 0;

    public static bool operator ==(CountryCode value1, CountryCode value2) => Equals(value1, value2);

    public static bool operator !=(CountryCode value1, CountryCode value2) => !Equals(value1, value2);

    /// <summary>
    /// Indicates if a given ISO country code references a member country of the EU.
    /// </summary>
    /// <param name="code"></param>
    /// <returns></returns>
    public static bool IsEuMemberCountry(CountryCode code) => s_euMemberCountryCodes.BinarySearch(code) > -1;

    /// <summary>
    /// Indicates if a given ISO country code references a member country of the EU.
    /// </summary>
    /// <param name="code"></param>
    /// <returns></returns>
    public static bool IsEuMemberCountry(string code)
        => code is null
        ? throw new ArgumentNullException(nameof(code))
        : IsEuMemberCountry(Parse(code.ToUpperInvariant()));

    /// <summary>
    /// Indicates if a given ISO country code references a member country of the EU or an
    /// included subdivision of a member country.
    /// </summary>
    /// <param name="code"></param>
    /// <returns></returns>
    public static bool IsEuMemberCountryOrSubdivision(CountryCode code)
        => s_euMemberCountryAndSubdivisionCodes.BinarySearch(code) > -1;

    /// <summary>
    /// Indicates if a given ISO country code references a member country of the EU or an
    /// included subdivision of a member country.
    /// </summary>
    /// <param name="code"></param>
    /// <returns></returns>
    public static bool IsEuMemberCountryOrSubdivision(string code)
        => code is null
        ? throw new ArgumentNullException(nameof(code))
        : IsEuMemberCountryOrSubdivision(Parse(code.ToUpperInvariant()));

    /// <summary>
    /// Indicates if a given ISO country code references a member country of the EU VAT Area or an
    /// included subdivision of a member country.
    /// </summary>
    /// <param name="code"></param>
    /// <returns></returns>
    public static bool IsEuVatAreaCountryOrSubdivision(CountryCode code)
        => s_euVatAreaCountryAndSubdivisionCodes.BinarySearch(code) > -1;

    /// <summary>
    /// Indicates if a given ISO country code references a member country of the EU VAT Area or an
    /// included subdivision of a member country.
    /// </summary>
    /// <param name="code"></param>
    /// <returns></returns>
    public static bool IsEuVatAreaCountryOrSubdivision(string code)
    {
        return code is null
            ? throw new ArgumentNullException(nameof(code))
            : IsEuVatAreaCountryOrSubdivision(Parse(code.ToUpperInvariant()));
    }

    public static bool IsEuroZoneCountry(string code)
        => code is null
        ? throw new ArgumentNullException(nameof(code))
        : IsEuroZoneCountry(Parse(code.ToUpperInvariant()));

    public static bool IsEuroZoneCountry(CountryCode code) => s_euroZoneCodes.BinarySearch(code) > -1;

    private static readonly List<CountryCode> s_euMemberCountryCodes = new()
    {
        Austria,
        Belgium,
        Bulgaria,
        Cyprus,
        CzechRepublic,
        Germany,
        Denmark,
        Estonia,
        Spain,
        Finland,
        France,
        Greece,
        Croatia,
        Hungary,
        Ireland,
        Italy,
        Lithuania,
        Luxembourg,
        Latvia,
        Malta,
        Netherlands,
        Poland,
        Portugal,
        Romania,
        Sweden,
        Slovenia,
        Slovakia,
    };

    // https://en.wikipedia.org/wiki/Member_state_of_the_European_Union
    //
    private static readonly List<CountryCode> s_euMemberCountryAndSubdivisionCodes = new()
    {
        Austria,
        new CountryCode('A', 'X'), // Åland Islands (Finland)
        Belgium,
        Bulgaria,
        Cyprus,
        CzechRepublic,
        Germany,
        Denmark,
        Estonia,
        new CountryCode('E', 'S'), // (includes Canary Islands, Ceuta and Melilla)
        Finland,
        France,
        new CountryCode('G', 'F'), // French Guiana
        new CountryCode('G', 'P'), // Guadeloupe (France)
        Greece,
        Croatia,
        Hungary,
        Ireland,
        Italy,
        Lithuania,
        Luxembourg,
        Latvia,
        new CountryCode('M', 'F'), // Saint Martin (France)
        new CountryCode('M', 'Q'), // Martinique (France)
        Malta,
        Netherlands,
        Poland,
        new CountryCode('P', 'T'), // (Includes Azores and Madeira)
        new CountryCode('R', 'E'), // Réunion (France)
        Romania,
        Sweden,
        Slovenia,
        Slovakia,
        new CountryCode('Y', 'T'), // Mayotte (France)
    };

    // https://www.gov.uk/guidance/vat-eu-country-codes-vat-numbers-and-vat-in-other-languages
    // https://en.wikipedia.org/wiki/European_Union_value_added_tax#EU_VAT_area
    // Note: Akrotiri and Dhekelia does not have ISO code.
    private static readonly List<CountryCode> s_euVatAreaCountryAndSubdivisionCodes = new()
    {
        Austria,
        Belgium,
        Bulgaria,
        Cyprus,
        CzechRepublic,
        Germany,
        Denmark,
        Estonia,
        Spain,
        Finland,
        France,
        Greece,
        Croatia,
        Hungary,
        Ireland,
        Italy,
        IsleOfMan, // Isle of Mann *,
        Lithuania,
        Luxembourg,
        Latvia,
        Monaco, // Monaco *,
        Malta,
        Netherlands,
        Poland,
        Portugal,
        Romania,
        Sweden,
        Slovenia,
        Slovakia
    };

    private static readonly List<CountryCode> s_isoCountryCodes = new()
    {
        new CountryCode('A', 'D'), // Andorra
        UnitedArabEmirates, // AE - United Arab Emirates
        new CountryCode('A', 'F'), // Afghanistan
        new CountryCode('A', 'G'), // Antigua and Barbuda
        new CountryCode('A', 'I'), // Anguilla
        new CountryCode('A', 'L'), // Albania
        new CountryCode('A', 'M'), // Armenia
        new CountryCode('A', 'O'), // Angola
        new CountryCode('A', 'Q'), // Antarctica
        new CountryCode('A', 'R'), // Argentina
        new CountryCode('A', 'S'), // American Samoa
        new CountryCode('A', 'T'), // Austria
        new CountryCode('A', 'U'), // Australia
        new CountryCode('A', 'W'), // Aruba
        new CountryCode('A', 'X'), // Aland Islands !Åland Islands
        new CountryCode('A', 'Z'), // Azerbaijan
        new CountryCode('B', 'A'), // Bosnia and Herzegovina
        new CountryCode('B', 'B'), // Barbados
        new CountryCode('B', 'D'), // Bangladesh
        new CountryCode('B', 'E'), // Belgium
        new CountryCode('B', 'F'), // Burkina Faso
        new CountryCode('B', 'G'), // Bulgaria
        new CountryCode('B', 'H'), // Bahrain
        new CountryCode('B', 'I'), // Burundi
        new CountryCode('B', 'J'), // Benin
        new CountryCode('B', 'L'), // Saint Barthélemy
        new CountryCode('B', 'M'), // Bermuda
        new CountryCode('B', 'N'), // Brunei Darussalam
        new CountryCode('B', 'O'), // Bolivia, Plurinational State of
        new CountryCode('B', 'Q'), // Bonaire, Sint Eustatius and Saba
        new CountryCode('B', 'R'), // Brazil
        new CountryCode('B', 'S'), // Bahamas
        new CountryCode('B', 'T'), // Bhutan
        new CountryCode('B', 'V'), // Bouvet Island
        new CountryCode('B', 'W'), // Botswana
        new CountryCode('B', 'Y'), // Belarus
        new CountryCode('B', 'Z'), // Belize
        new CountryCode('C', 'A'), // Canada
        new CountryCode('C', 'C'), // Cocos (Keeling) Islands
        new CountryCode('C', 'D'), // Congo, the Democratic Republic of the
        new CountryCode('C', 'F'), // Central African Republic
        new CountryCode('C', 'G'), // Congo
        new CountryCode('C', 'H'), // Switzerland
        new CountryCode('C', 'I'), // Cote d'Ivoire !Côte d'Ivoire
        new CountryCode('C', 'K'), // Cook Islands
        new CountryCode('C', 'L'), // Chile
        new CountryCode('C', 'M'), // Cameroon
        new CountryCode('C', 'N'), // China
        new CountryCode('C', 'O'), // Colombia
        new CountryCode('C', 'R'), // Costa Rica
        new CountryCode('C', 'U'), // Cuba
        new CountryCode('C', 'V'), // Cabo Verde
        new CountryCode('C', 'W'), // Curaçao
        new CountryCode('C', 'X'), // Christmas Island
        new CountryCode('C', 'Y'), // Cyprus
        new CountryCode('C', 'Z'), // Czechia
        new CountryCode('D', 'E'), // Germany
        new CountryCode('D', 'J'), // Djibouti
        new CountryCode('D', 'K'), // Denmark
        new CountryCode('D', 'M'), // Dominica
        new CountryCode('D', 'O'), // Dominican Republic
        new CountryCode('D', 'Z'), // Algeria
        new CountryCode('E', 'C'), // Ecuador
        new CountryCode('E', 'E'), // Estonia
        new CountryCode('E', 'G'), // Egypt
        new CountryCode('E', 'H'), // Western Sahara
        new CountryCode('E', 'R'), // Eritrea
        new CountryCode('E', 'S'), // Spain
        new CountryCode('E', 'T'), // Ethiopia
        new CountryCode('F', 'I'), // Finland
        new CountryCode('F', 'J'), // Fiji
        new CountryCode('F', 'K'), // Falkland Islands (Malvinas)
        new CountryCode('F', 'M'), // Micronesia, Federated States of
        new CountryCode('F', 'O'), // Faroe Islands
        new CountryCode('F', 'R'), // France
        new CountryCode('G', 'A'), // Gabon
        UnitedKingdom, // GB - United Kingdom of Great Britain and Northern Ireland
        new CountryCode('G', 'D'), // Grenada
        new CountryCode('G', 'E'), // Georgia
        new CountryCode('G', 'F'), // French Guiana
        new CountryCode('G', 'G'), // Guernsey
        new CountryCode('G', 'H'), // Ghana
        new CountryCode('G', 'I'), // Gibraltar
        new CountryCode('G', 'L'), // Greenland
        new CountryCode('G', 'M'), // Gambia
        new CountryCode('G', 'N'), // Guinea
        new CountryCode('G', 'P'), // Guadeloupe
        new CountryCode('G', 'Q'), // Equatorial Guinea
        new CountryCode('G', 'R'), // Greece
        new CountryCode('G', 'S'), // South Georgia and the South Sandwich Islands
        new CountryCode('G', 'T'), // Guatemala
        new CountryCode('G', 'U'), // Guam
        new CountryCode('G', 'W'), // Guinea-Bissau
        new CountryCode('G', 'Y'), // Guyana
        new CountryCode('H', 'K'), // Hong Kong
        new CountryCode('H', 'M'), // Heard Island and McDonald Islands
        new CountryCode('H', 'N'), // Honduras
        new CountryCode('H', 'R'), // Croatia
        new CountryCode('H', 'T'), // Haiti
        new CountryCode('H', 'U'), // Hungary
        new CountryCode('I', 'D'), // Indonesia
        new CountryCode('I', 'E'), // Ireland
        new CountryCode('I', 'L'), // Israel
        new CountryCode('I', 'M'), // Isle of Man
        new CountryCode('I', 'N'), // India
        new CountryCode('I', 'O'), // British Indian Ocean Territory
        new CountryCode('I', 'Q'), // Iraq
        new CountryCode('I', 'R'), // Iran, Islamic Republic of
        new CountryCode('I', 'S'), // Iceland
        new CountryCode('I', 'T'), // Italy
        new CountryCode('J', 'E'), // Jersey
        new CountryCode('J', 'M'), // Jamaica
        new CountryCode('J', 'O'), // Jordan
        new CountryCode('J', 'P'), // Japan
        new CountryCode('K', 'E'), // Kenya
        new CountryCode('K', 'G'), // Kyrgyzstan
        new CountryCode('K', 'H'), // Cambodia
        new CountryCode('K', 'I'), // Kiribati
        new CountryCode('K', 'M'), // Comoros
        new CountryCode('K', 'N'), // Saint Kitts and Nevis
        new CountryCode('K', 'P'), // Korea, Democratic People's Republic of
        new CountryCode('K', 'R'), // Korea, Republic of
        new CountryCode('K', 'W'), // Kuwait
        new CountryCode('K', 'Y'), // Cayman Islands
        new CountryCode('K', 'Z'), // Kazakhstan
        new CountryCode('L', 'A'), // Lao People's Democratic Republic
        new CountryCode('L', 'B'), // Lebanon
        new CountryCode('L', 'C'), // Saint Lucia
        new CountryCode('L', 'I'), // Liechtenstein
        new CountryCode('L', 'K'), // Sri Lanka
        new CountryCode('L', 'R'), // Liberia
        new CountryCode('L', 'S'), // Lesotho
        new CountryCode('L', 'T'), // Lithuania
        new CountryCode('L', 'U'), // Luxembourg
        new CountryCode('L', 'V'), // Latvia
        new CountryCode('L', 'Y'), // Libya
        new CountryCode('M', 'A'), // Morocco
        new CountryCode('M', 'C'), // Monaco
        new CountryCode('M', 'D'), // Moldova, Republic of
        new CountryCode('M', 'E'), // Montenegro
        new CountryCode('M', 'F'), // Saint Martin (French part)
        new CountryCode('M', 'G'), // Madagascar
        new CountryCode('M', 'H'), // Marshall Islands
        new CountryCode('M', 'K'), // Macedonia, the former Yugoslav Republic of
        new CountryCode('M', 'L'), // Mali
        new CountryCode('M', 'M'), // Myanmar
        new CountryCode('M', 'N'), // Mongolia
        new CountryCode('M', 'O'), // Macao
        new CountryCode('M', 'P'), // Northern Mariana Islands
        new CountryCode('M', 'Q'), // Martinique
        new CountryCode('M', 'R'), // Mauritania
        new CountryCode('M', 'S'), // Montserrat
        new CountryCode('M', 'T'), // Malta
        new CountryCode('M', 'U'), // Mauritius
        new CountryCode('M', 'V'), // Maldives
        new CountryCode('M', 'W'), // Malawi
        new CountryCode('M', 'X'), // Mexico
        new CountryCode('M', 'Y'), // Malaysia
        new CountryCode('M', 'Z'), // Mozambique
        new CountryCode('N', 'A'), // Namibia
        new CountryCode('N', 'C'), // New Caledonia
        new CountryCode('N', 'E'), // Niger
        new CountryCode('N', 'F'), // Norfolk Island
        new CountryCode('N', 'G'), // Nigeria
        new CountryCode('N', 'I'), // Nicaragua
        new CountryCode('N', 'L'), // Netherlands
        new CountryCode('N', 'O'), // Norway
        new CountryCode('N', 'P'), // Nepal
        new CountryCode('N', 'R'), // Nauru
        new CountryCode('N', 'U'), // Niue
        new CountryCode('N', 'Z'), // New Zealand
        new CountryCode('O', 'M'), // Oman
        new CountryCode('P', 'A'), // Panama
        new CountryCode('P', 'E'), // Peru
        new CountryCode('P', 'F'), // French Polynesia
        new CountryCode('P', 'G'), // Papua New Guinea
        new CountryCode('P', 'H'), // Philippines
        new CountryCode('P', 'K'), // Pakistan
        new CountryCode('P', 'L'), // Poland
        new CountryCode('P', 'M'), // Saint Pierre and Miquelon
        new CountryCode('P', 'N'), // Pitcairn
        new CountryCode('P', 'R'), // Puerto Rico
        new CountryCode('P', 'S'), // Palestine, State of
        new CountryCode('P', 'T'), // Portugal
        new CountryCode('P', 'W'), // Palau
        new CountryCode('P', 'Y'), // Paraguay
        new CountryCode('Q', 'A'), // Qatar
        new CountryCode('R', 'E'), // Reunion !Réunion
        new CountryCode('R', 'O'), // Romania
        new CountryCode('R', 'S'), // Serbia
        new CountryCode('R', 'U'), // Russian Federation
        new CountryCode('R', 'W'), // Rwanda
        SaudiArabia, // SA - Saudi Arabia
        new CountryCode('S', 'B'), // Solomon Islands
        new CountryCode('S', 'C'), // Seychelles
        new CountryCode('S', 'D'), // Sudan
        Sweden, // SE - Sweden
        Singapore, // SG - Singapore
        new CountryCode('S', 'H'), // Saint Helena, Ascension and Tristan da Cunha
        new CountryCode('S', 'I'), // Slovenia
        new CountryCode('S', 'J'), // Svalbard and Jan Mayen
        new CountryCode('S', 'K'), // Slovakia
        new CountryCode('S', 'L'), // Sierra Leone
        new CountryCode('S', 'M'), // San Marino
        new CountryCode('S', 'N'), // Senegal
        new CountryCode('S', 'O'), // Somalia
        new CountryCode('S', 'R'), // Suriname
        new CountryCode('S', 'S'), // South Sudan
        new CountryCode('S', 'T'), // Sao Tome and Principe
        new CountryCode('S', 'V'), // El Salvador
        new CountryCode('S', 'X'), // Sint Maarten (Dutch part)
        new CountryCode('S', 'Y'), // Syrian Arab Republic
        new CountryCode('S', 'Z'), // Swaziland
        new CountryCode('T', 'C'), // Turks and Caicos Islands
        new CountryCode('T', 'D'), // Chad
        new CountryCode('T', 'F'), // French Southern Territories
        new CountryCode('T', 'G'), // Togo
        new CountryCode('T', 'H'), // Thailand
        new CountryCode('T', 'J'), // Tajikistan
        new CountryCode('T', 'K'), // Tokelau
        new CountryCode('T', 'L'), // Timor-Leste
        new CountryCode('T', 'M'), // Turkmenistan
        new CountryCode('T', 'N'), // Tunisia
        new CountryCode('T', 'O'), // Tonga
        Turkey, // Turkey
        new CountryCode('T', 'T'), // Trinidad and Tobago
        new CountryCode('T', 'V'), // Tuvalu
        new CountryCode('T', 'W'), // Taiwan, Province of China
        new CountryCode('T', 'Z'), // Tanzania, United Republic of
        Ukraine, // Ukraine
        new CountryCode('U', 'G'), // Uganda
        new CountryCode('U', 'M'), // United States Minor Outlying Islands
        UnitedStates, // United States of America
        new CountryCode('U', 'Y'), // Uruguay
        new CountryCode('U', 'Z'), // Uzbekistan
        new CountryCode('V', 'A'), // Holy See
        new CountryCode('V', 'C'), // Saint Vincent and the Grenadines
        new CountryCode('V', 'E'), // Venezuela, Bolivarian Republic of
        new CountryCode('V', 'G'), // Virgin Islands, British
        new CountryCode('V', 'I'), // Virgin Islands, U.S.
        new CountryCode('V', 'N'), // Viet Nam
        new CountryCode('V', 'U'), // Vanuatu
        new CountryCode('W', 'F'), // Wallis and Futuna
        new CountryCode('W', 'S'), // Samoa
        new CountryCode('X', 'K'), // Kosovo (temporary use of user-assigned code)
        new CountryCode('Y', 'E'), // Yemen
        new CountryCode('Y', 'T'), // Mayotte
        SouthAfrica, // South Africa
        new CountryCode('Z', 'M'), // Zambia
        new CountryCode('Z', 'W'), // Zimbabwe
        new CountryCode('Z', 'Z'), // Unknown or Invalid Territory (as per Unicode Common Locale Data Repository)
    };

    private static readonly List<CountryCode> s_euroZoneCodes = new()
    {
        Austria,
        Belgium,
        Cyprus,
        Estonia,
        Finland,
        France,
        Germany,
        Greece,
        Ireland,
        Italy,
        Latvia,
        Lithuania,
        Luxembourg,
        Malta,
        Netherlands,
        Portugal,
        Slovakia,
        Slovenia,
        Spain
    };

    public static CountryCode ToCountryCode(CountryCode left, CountryCode right) => throw new NotImplementedException();
}
