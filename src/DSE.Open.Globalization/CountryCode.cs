// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using DSE.Open.Globalization.Text.Json;
using DSE.Open.Values;

namespace DSE.Open.Globalization;

/// <summary>
/// The ISO 3166-1 alpha-2 code for a country. Only officially assigned and
/// user-assigned codes are valid values.
/// </summary>
[ComparableValue]
[JsonConverter(typeof(JsonStringCountryCodeConverter))]
[StructLayout(LayoutKind.Auto)]
[SuppressMessage("Design", "CA1036:Override methods on comparable types", Justification = "Not necessary")]
public readonly partial struct CountryCode : IComparableValue<CountryCode, AsciiChar2>
{
    public const int Length = 2;

    public static int MaxSerializedCharLength => Length;

    public bool Equals(CountryCode other) => _value.EqualsCaseInsensitive(other._value);

    public int CompareTo(CountryCode other) => _value.CompareToCaseInsensitive(other._value);

    public override int GetHashCode() => AsciiChar2Comparer.CaseInsensitive.GetHashCode(this._value);

    public bool Equals(string other) => _value.Equals(other);

    public bool Equals(ReadOnlyMemory<char> other) => _value.Equals(other);

    public bool Equals(ReadOnlySpan<char> other) => _value.Equals(other);

    public char[] ToCharArray() => _value.ToCharArray();

    public static bool IsValidValue(AsciiChar2 value) => IsValidValue(value, true);

    private static bool IsValidValue(AsciiChar2 value, bool normalize)
    {
        if (normalize)
        {
            value = value.ToUpper();
        }

        return IsoCountryCodes.AssignedAlpha2Ascii.Contains(value);
    }

    public AsciiChar2 ToAsciiChar2() => _value;

    public static CountryCode FromAsciiChar2(AsciiChar2 value) => value.CastToValue<CountryCode, AsciiChar2>();

#pragma warning disable CA2225 // Operator overloads have named alternates

    public static explicit operator CountryCode(string code) => Parse(code);

    public static explicit operator string(CountryCode code) => code.ToString();

#pragma warning restore CA2225 // Operator overloads have named alternates

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

    public static readonly CountryCode Andorra = new(new AsciiChar2('A', 'D'), true); // new(new AsciiChar2('A','D'), true);
    public static readonly CountryCode UnitedArabEmirates = new(new AsciiChar2('A', 'E'), true);
    public static readonly CountryCode Austria = new(new AsciiChar2('A', 'T'), true);
    public static readonly CountryCode Australia = new(new AsciiChar2('A', 'U'), true);
    public static readonly CountryCode Belgium = new(new AsciiChar2('B', 'E'), true);
    public static readonly CountryCode Brazil = new(new AsciiChar2('B', 'R'), true);
    public static readonly CountryCode Bulgaria = new(new AsciiChar2('B', 'G'), true);
    public static readonly CountryCode Canada = new(new AsciiChar2('C', 'A'), true);
    public static readonly CountryCode Switzerland = new(new AsciiChar2('C', 'H'), true);
    public static readonly CountryCode China = new(new AsciiChar2('C', 'N'), true);
    public static readonly CountryCode Cyprus = new(new AsciiChar2('C', 'Y'), true);
    public static readonly CountryCode CzechRepublic = new(new AsciiChar2('C', 'Z'), true);
    public static readonly CountryCode Germany = new(new AsciiChar2('D', 'E'), true);
    public static readonly CountryCode Denmark = new(new AsciiChar2('D', 'K'), true);
    public static readonly CountryCode Estonia = new(new AsciiChar2('E', 'E'), true);
    public static readonly CountryCode Spain = new(new AsciiChar2('E', 'S'), true);
    public static readonly CountryCode Finland = new(new AsciiChar2('F', 'I'), true);
    public static readonly CountryCode France = new(new AsciiChar2('F', 'R'), true);
    public static readonly CountryCode UnitedKingdom = new(new AsciiChar2('G', 'B'), true);
    public static readonly CountryCode Gibraltar = new(new AsciiChar2('G', 'I'), true);
    public static readonly CountryCode Greece = new(new AsciiChar2('G', 'R'), true);
    public static readonly CountryCode HongKongSAR = new(new AsciiChar2('H', 'K'), true);
    public static readonly CountryCode Croatia = new(new AsciiChar2('H', 'R'), true);
    public static readonly CountryCode Hungary = new(new AsciiChar2('H', 'U'), true);
    public static readonly CountryCode Ireland = new(new AsciiChar2('I', 'E'), true);
    public static readonly CountryCode Israel = new(new AsciiChar2('I', 'L'), true);
    public static readonly CountryCode IsleOfMan = new(new AsciiChar2('I', 'M'), true);
    public static readonly CountryCode India = new(new AsciiChar2('I', 'N'), true);
    public static readonly CountryCode Italy = new(new AsciiChar2('I', 'T'), true);
    public static readonly CountryCode Jersey = new(new AsciiChar2('J', 'E'), true);
    public static readonly CountryCode Japan = new(new AsciiChar2('J', 'P'), true);
    public static readonly CountryCode Liechtenstein = new(new AsciiChar2('L', 'I'), true);
    public static readonly CountryCode Lithuania = new(new AsciiChar2('L', 'T'), true);
    public static readonly CountryCode Luxembourg = new(new AsciiChar2('L', 'U'), true);
    public static readonly CountryCode Latvia = new(new AsciiChar2('L', 'V'), true);
    public static readonly CountryCode Monaco = new(new AsciiChar2('M', 'C'), true);
    public static readonly CountryCode Montenegro = new(new AsciiChar2('M', 'E'), true);
    public static readonly CountryCode Malta = new(new AsciiChar2('M', 'T'), true);
    public static readonly CountryCode Mexico = new(new AsciiChar2('M', 'X'), true);
    public static readonly CountryCode Netherlands = new(new AsciiChar2('N', 'L'), true);
    public static readonly CountryCode Norway = new(new AsciiChar2('N', 'O'), true);
    public static readonly CountryCode NewZealand = new(new AsciiChar2('N', 'Z'), true);
    public static readonly CountryCode Poland = new(new AsciiChar2('P', 'L'), true);
    public static readonly CountryCode Portugal = new(new AsciiChar2('P', 'T'), true);
    public static readonly CountryCode Romania = new(new AsciiChar2('R', 'O'), true);
    public static readonly CountryCode Serbia = new(new AsciiChar2('R', 'S'), true);
    public static readonly CountryCode Russia = new(new AsciiChar2('R', 'U'), true);
    public static readonly CountryCode SaudiArabia = new(new AsciiChar2('S', 'A'), true);
    public static readonly CountryCode Sweden = new(new AsciiChar2('S', 'E'), true);
    public static readonly CountryCode Singapore = new(new AsciiChar2('S', 'G'), true);
    public static readonly CountryCode Slovenia = new(new AsciiChar2('S', 'I'), true);
    public static readonly CountryCode Slovakia = new(new AsciiChar2('S', 'K'), true);
    public static readonly CountryCode SanMarino = new(new AsciiChar2('S', 'M'), true);
    public static readonly CountryCode Turkey = new(new AsciiChar2('T', 'R'), true);
    public static readonly CountryCode Ukraine = new(new AsciiChar2('U', 'A'), true);
    public static readonly CountryCode UnitedStates = new(new AsciiChar2('U', 'S'), true);
    public static readonly CountryCode Kosovo = new(new AsciiChar2('X', 'K'), true);
    public static readonly CountryCode SouthAfrica = new(new AsciiChar2('Z', 'A'), true);

    public static CountryCode FromRegionInfo(RegionInfo regionInfo)
        => regionInfo is null
        ? throw new ArgumentNullException(nameof(regionInfo))
        : Parse(regionInfo.TwoLetterISORegionName);

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
        new CountryCode((AsciiChar2)"AX"), // Åland Islands (Finland)
        Belgium,
        Bulgaria,
        Cyprus,
        CzechRepublic,
        Germany,
        Denmark,
        Estonia,
        new CountryCode((AsciiChar2)"ES"), // (includes Canary Islands, Ceuta and Melilla)
        Finland,
        France,
        new CountryCode((AsciiChar2)"GF"), // French Guiana
        new CountryCode((AsciiChar2)"GP"), // Guadeloupe (France)
        Greece,
        Croatia,
        Hungary,
        Ireland,
        Italy,
        Lithuania,
        Luxembourg,
        Latvia,
        new CountryCode((AsciiChar2)"MF"), // Saint Martin (France)
        new CountryCode((AsciiChar2)"MQ"), // Martinique (France)
        Malta,
        Netherlands,
        Poland,
        new CountryCode((AsciiChar2)"PT"), // (Includes Azores and Madeira)
        new CountryCode((AsciiChar2)"RE"), // Réunion (France)
        Romania,
        Sweden,
        Slovenia,
        Slovakia,
        new CountryCode((AsciiChar2)"YT"), // Mayotte (France)
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

}
