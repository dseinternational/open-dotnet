// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections.Frozen;
using System.Diagnostics.CodeAnalysis;
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
[StructLayout(LayoutKind.Sequential)]
[SuppressMessage("Design", "CA1036:Override methods on comparable types", Justification = "Not necessary")]
public readonly partial struct CountryCode : IComparableValue<CountryCode, AsciiChar2>, IUtf8SpanSerializable<CountryCode>
{
    /// <summary>
    /// The number of characters in an ISO 3166-1 alpha-2 country code.
    /// </summary>
    public const int Length = 2;

    /// <inheritdoc/>
    public static int MaxSerializedCharLength => Length;

    /// <inheritdoc/>
    public static int MaxSerializedByteLength => Length;

    /// <inheritdoc/>
    public int CompareTo(CountryCode other)
    {
        return _value.CompareToIgnoreCase(other._value);
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return AsciiChar2Comparer.IgnoreCase.GetHashCode(_value);
    }

    /// <summary>
    /// Determines if the specified value is equal to this value. Country code comparisons are case-insensitive.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Equals(CountryCode other)
    {
        return _value.EqualsIgnoreCase(other._value);
    }

    /// <summary>
    /// Determines if the specified value is equal to this value. Country code comparisons are case-insensitive.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Equals(string other)
    {
        return _value.EqualsIgnoreCase(other);
    }

    /// <summary>
    /// Determines if the specified value is equal to this value. Country code comparisons are case-insensitive.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Equals(ReadOnlyMemory<char> other)
    {
        return _value.EqualsIgnoreCase(other.Span);
    }

    /// <summary>
    /// Determines if the specified value is equal to this value. Country code comparisons are case-insensitive.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Equals(ReadOnlySpan<char> other)
    {
        return _value.EqualsIgnoreCase(other);
    }

    /// <summary>
    /// Returns the country code as a character array.
    /// </summary>
    public char[] ToCharArray()
    {
        return _value.ToCharArray();
    }

    /// <summary>
    /// Returns the country code as a lowercase string.
    /// </summary>
    public string ToStringLower()
    {
        return _value.ToStringLower();
    }

    /// <summary>
    /// Returns the country code as an uppercase string.
    /// </summary>
    public string ToStringUpper()
    {
        return _value.ToStringUpper();
    }

    /// <summary>
    /// Returns a value indicating whether the specified value is a valid (assigned)
    /// ISO 3166-1 alpha-2 country code.
    /// </summary>
    /// <param name="value">The value to check.</param>
    public static bool IsValidValue(AsciiChar2 value)
    {
        return IsValidValue(value, true);
    }

    private static bool IsValidValue(AsciiChar2 value, bool normalize)
    {
        if (normalize)
        {
            value = value.ToUpper();
        }

        return IsoCountryCodes.AssignedAlpha2Ascii.Contains(value);
    }

    /// <summary>
    /// Returns the underlying <see cref="AsciiChar2"/> value of the country code.
    /// </summary>
    public AsciiChar2 ToAsciiChar2()
    {
        return _value;
    }

    /// <summary>
    /// Creates a <see cref="CountryCode"/> from the specified <see cref="AsciiChar2"/> value
    /// without validation.
    /// </summary>
    /// <param name="value">The two-character ASCII value.</param>
    public static CountryCode FromAsciiChar2(AsciiChar2 value)
    {
        return value.CastToValue<CountryCode, AsciiChar2>();
    }

#pragma warning disable CA2225 // Operator overloads have named alternates

    /// <summary>
    /// Explicitly converts a string to a <see cref="CountryCode"/>.
    /// </summary>
    /// <param name="code">The string to convert.</param>
    public static explicit operator CountryCode(string code)
    {
        return Parse(code, CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Explicitly converts a <see cref="CountryCode"/> to its string representation.
    /// </summary>
    /// <param name="code">The country code to convert.</param>
    public static explicit operator string(CountryCode code)
    {
        return code.ToString();
    }

#pragma warning restore CA2225 // Operator overloads have named alternates

    /// <summary>
    /// Indicates if a given ISO country code references a member country of the EU.
    /// </summary>
    /// <param name="code"></param>
    /// <returns></returns>
    public static bool IsEuMemberCountry(CountryCode code)
    {
        return s_euMemberCountryCodes.Contains(code);
    }

    /// <summary>
    /// Indicates if a given ISO country code references a member country of the EU.
    /// </summary>
    /// <param name="code"></param>
    /// <returns></returns>
    public static bool IsEuMemberCountry(string code)
    {
        return code is null
            ? throw new ArgumentNullException(nameof(code))
            : IsEuMemberCountry(Parse(code.ToUpperInvariant(), CultureInfo.InvariantCulture));
    }

    /// <summary>
    /// Indicates if a given ISO country code references a member country of the EU or an
    /// included subdivision of a member country.
    /// </summary>
    /// <param name="code"></param>
    /// <returns></returns>
    public static bool IsEuMemberCountryOrSubdivision(CountryCode code)
    {
        return s_euMemberCountryAndSubdivisionCodes.Contains(code);
    }

    /// <summary>
    /// Indicates if a given ISO country code references a member country of the EU or an
    /// included subdivision of a member country.
    /// </summary>
    /// <param name="code"></param>
    /// <returns></returns>
    public static bool IsEuMemberCountryOrSubdivision(string code)
    {
        return code is null
            ? throw new ArgumentNullException(nameof(code))
            : IsEuMemberCountryOrSubdivision(Parse(code.ToUpperInvariant(), CultureInfo.InvariantCulture));
    }

    /// <summary>
    /// Indicates if a given ISO country code references a member country of the EU VAT Area or an
    /// included subdivision of a member country.
    /// </summary>
    /// <param name="code"></param>
    /// <returns></returns>
    public static bool IsEuVatAreaCountryOrSubdivision(CountryCode code)
    {
        return s_euVatAreaCountryAndSubdivisionCodes.Contains(code);
    }

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
            : IsEuVatAreaCountryOrSubdivision(Parse(code.ToUpperInvariant(), CultureInfo.InvariantCulture));
    }

    /// <summary>
    /// Indicates if a given ISO country code references a member country of the Eurozone.
    /// </summary>
    /// <param name="code"></param>
    /// <returns></returns>
    public static bool IsEuroZoneCountry(string code)
    {
        return code is null
            ? throw new ArgumentNullException(nameof(code))
            : IsEuroZoneCountry(Parse(code.ToUpperInvariant(), CultureInfo.InvariantCulture));
    }

    /// <summary>
    /// Indicates if a given ISO country code references a member country of the Eurozone.
    /// </summary>
    /// <param name="code"></param>
    /// <returns></returns>
    public static bool IsEuroZoneCountry(CountryCode code)
    {
        return s_euroZoneCodes.Contains(code);
    }

    /// <summary>The country code for Andorra (AD).</summary>
    public static readonly CountryCode Andorra = new(new('A', 'D'), true);
    /// <summary>The country code for the United Arab Emirates (AE).</summary>
    public static readonly CountryCode UnitedArabEmirates = new(new('A', 'E'), true);
    /// <summary>The country code for Austria (AT).</summary>
    public static readonly CountryCode Austria = new(new('A', 'T'), true);
    /// <summary>The country code for Australia (AU).</summary>
    public static readonly CountryCode Australia = new(new('A', 'U'), true);
    /// <summary>The country code for Belgium (BE).</summary>
    public static readonly CountryCode Belgium = new(new('B', 'E'), true);
    /// <summary>The country code for Brazil (BR).</summary>
    public static readonly CountryCode Brazil = new(new('B', 'R'), true);
    /// <summary>The country code for Bulgaria (BG).</summary>
    public static readonly CountryCode Bulgaria = new(new('B', 'G'), true);
    /// <summary>The country code for Canada (CA).</summary>
    public static readonly CountryCode Canada = new(new('C', 'A'), true);
    /// <summary>The country code for Switzerland (CH).</summary>
    public static readonly CountryCode Switzerland = new(new('C', 'H'), true);
    /// <summary>The country code for China (CN).</summary>
    public static readonly CountryCode China = new(new('C', 'N'), true);
    /// <summary>The country code for Cyprus (CY).</summary>
    public static readonly CountryCode Cyprus = new(new('C', 'Y'), true);
    /// <summary>The country code for the Czech Republic (CZ).</summary>
    public static readonly CountryCode CzechRepublic = new(new('C', 'Z'), true);
    /// <summary>The country code for Germany (DE).</summary>
    public static readonly CountryCode Germany = new(new('D', 'E'), true);
    /// <summary>The country code for Denmark (DK).</summary>
    public static readonly CountryCode Denmark = new(new('D', 'K'), true);
    /// <summary>The country code for Estonia (EE).</summary>
    public static readonly CountryCode Estonia = new(new('E', 'E'), true);
    /// <summary>The country code for Spain (ES).</summary>
    public static readonly CountryCode Spain = new(new('E', 'S'), true);
    /// <summary>The country code for Finland (FI).</summary>
    public static readonly CountryCode Finland = new(new('F', 'I'), true);
    /// <summary>The country code for France (FR).</summary>
    public static readonly CountryCode France = new(new('F', 'R'), true);
    /// <summary>The country code for the United Kingdom (GB).</summary>
    public static readonly CountryCode UnitedKingdom = new(new('G', 'B'), true);
    /// <summary>The country code for Gibraltar (GI).</summary>
    public static readonly CountryCode Gibraltar = new(new('G', 'I'), true);
    /// <summary>The country code for Greece (GR).</summary>
    public static readonly CountryCode Greece = new(new('G', 'R'), true);
    /// <summary>The country code for Hong Kong SAR (HK).</summary>
    public static readonly CountryCode HongKongSAR = new(new('H', 'K'), true);
    /// <summary>The country code for Croatia (HR).</summary>
    public static readonly CountryCode Croatia = new(new('H', 'R'), true);
    /// <summary>The country code for Hungary (HU).</summary>
    public static readonly CountryCode Hungary = new(new('H', 'U'), true);
    /// <summary>The country code for Ireland (IE).</summary>
    public static readonly CountryCode Ireland = new(new('I', 'E'), true);
    /// <summary>The country code for Israel (IL).</summary>
    public static readonly CountryCode Israel = new(new('I', 'L'), true);
    /// <summary>The country code for the Isle of Man (IM).</summary>
    public static readonly CountryCode IsleOfMan = new(new('I', 'M'), true);
    /// <summary>The country code for India (IN).</summary>
    public static readonly CountryCode India = new(new('I', 'N'), true);
    /// <summary>The country code for Italy (IT).</summary>
    public static readonly CountryCode Italy = new(new('I', 'T'), true);
    /// <summary>The country code for Jersey (JE).</summary>
    public static readonly CountryCode Jersey = new(new('J', 'E'), true);
    /// <summary>The country code for Japan (JP).</summary>
    public static readonly CountryCode Japan = new(new('J', 'P'), true);
    /// <summary>The country code for Liechtenstein (LI).</summary>
    public static readonly CountryCode Liechtenstein = new(new('L', 'I'), true);
    /// <summary>The country code for Lithuania (LT).</summary>
    public static readonly CountryCode Lithuania = new(new('L', 'T'), true);
    /// <summary>The country code for Luxembourg (LU).</summary>
    public static readonly CountryCode Luxembourg = new(new('L', 'U'), true);
    /// <summary>The country code for Latvia (LV).</summary>
    public static readonly CountryCode Latvia = new(new('L', 'V'), true);
    /// <summary>The country code for Monaco (MC).</summary>
    public static readonly CountryCode Monaco = new(new('M', 'C'), true);
    /// <summary>The country code for Montenegro (ME).</summary>
    public static readonly CountryCode Montenegro = new(new('M', 'E'), true);
    /// <summary>The country code for Malta (MT).</summary>
    public static readonly CountryCode Malta = new(new('M', 'T'), true);
    /// <summary>The country code for Mexico (MX).</summary>
    public static readonly CountryCode Mexico = new(new('M', 'X'), true);
    /// <summary>The country code for the Netherlands (NL).</summary>
    public static readonly CountryCode Netherlands = new(new('N', 'L'), true);
    /// <summary>The country code for Norway (NO).</summary>
    public static readonly CountryCode Norway = new(new('N', 'O'), true);
    /// <summary>The country code for New Zealand (NZ).</summary>
    public static readonly CountryCode NewZealand = new(new('N', 'Z'), true);
    /// <summary>The country code for Poland (PL).</summary>
    public static readonly CountryCode Poland = new(new('P', 'L'), true);
    /// <summary>The country code for Portugal (PT).</summary>
    public static readonly CountryCode Portugal = new(new('P', 'T'), true);
    /// <summary>The country code for Romania (RO).</summary>
    public static readonly CountryCode Romania = new(new('R', 'O'), true);
    /// <summary>The country code for Serbia (RS).</summary>
    public static readonly CountryCode Serbia = new(new('R', 'S'), true);
    /// <summary>The country code for Russia (RU).</summary>
    public static readonly CountryCode Russia = new(new('R', 'U'), true);
    /// <summary>The country code for Saudi Arabia (SA).</summary>
    public static readonly CountryCode SaudiArabia = new(new('S', 'A'), true);
    /// <summary>The country code for Sweden (SE).</summary>
    public static readonly CountryCode Sweden = new(new('S', 'E'), true);
    /// <summary>The country code for Singapore (SG).</summary>
    public static readonly CountryCode Singapore = new(new('S', 'G'), true);
    /// <summary>The country code for Slovenia (SI).</summary>
    public static readonly CountryCode Slovenia = new(new('S', 'I'), true);
    /// <summary>The country code for Slovakia (SK).</summary>
    public static readonly CountryCode Slovakia = new(new('S', 'K'), true);
    /// <summary>The country code for San Marino (SM).</summary>
    public static readonly CountryCode SanMarino = new(new('S', 'M'), true);
    /// <summary>The country code for Turkey (TR).</summary>
    public static readonly CountryCode Turkey = new(new('T', 'R'), true);
    /// <summary>The country code for Ukraine (UA).</summary>
    public static readonly CountryCode Ukraine = new(new('U', 'A'), true);
    /// <summary>The country code for the United States (US).</summary>
    public static readonly CountryCode UnitedStates = new(new('U', 'S'), true);
    /// <summary>The country code for Kosovo (XK).</summary>
    public static readonly CountryCode Kosovo = new(new('X', 'K'), true);
    /// <summary>The country code for South Africa (ZA).</summary>
    public static readonly CountryCode SouthAfrica = new(new('Z', 'A'), true);

    /// <summary>
    /// Returns the <see cref="CountryCode"/> corresponding to the
    /// <see cref="RegionInfo.TwoLetterISORegionName"/> of the specified <see cref="RegionInfo"/>.
    /// </summary>
    /// <param name="regionInfo">The <see cref="RegionInfo"/> to convert.</param>
    public static CountryCode FromRegionInfo(RegionInfo regionInfo)
    {
        return regionInfo is null
            ? throw new ArgumentNullException(nameof(regionInfo))
            : Parse(regionInfo.TwoLetterISORegionName, CultureInfo.InvariantCulture);
    }

    private static readonly FrozenSet<CountryCode> s_euMemberCountryCodes = FrozenSet.ToFrozenSet(
    [
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
    ]);

    // https://en.wikipedia.org/wiki/Member_state_of_the_European_Union
    //
    private static readonly FrozenSet<CountryCode> s_euMemberCountryAndSubdivisionCodes = FrozenSet.ToFrozenSet(
    [
        Austria,
        new((AsciiChar2)"AX"), // Åland Islands (Finland)
        Belgium,
        Bulgaria,
        Cyprus,
        CzechRepublic,
        Germany,
        Denmark,
        Estonia,
        new((AsciiChar2)"ES"), // (includes Canary Islands, Ceuta and Melilla)
        Finland,
        France,
        new((AsciiChar2)"GF"), // French Guiana
        new((AsciiChar2)"GP"), // Guadeloupe (France)
        Greece,
        Croatia,
        Hungary,
        Ireland,
        Italy,
        Lithuania,
        Luxembourg,
        Latvia,
        new((AsciiChar2)"MF"), // Saint Martin (France)
        new((AsciiChar2)"MQ"), // Martinique (France)
        Malta,
        Netherlands,
        Poland,
        new((AsciiChar2)"PT"), // (Includes Azores and Madeira)
        new((AsciiChar2)"RE"), // Réunion (France)
        Romania,
        Sweden,
        Slovenia,
        Slovakia,
        new((AsciiChar2)"YT"), // Mayotte (France)
    ]);

    // https://www.gov.uk/guidance/vat-eu-country-codes-vat-numbers-and-vat-in-other-languages
    // https://en.wikipedia.org/wiki/European_Union_value_added_tax#EU_VAT_area
    // Note: Akrotiri and Dhekelia does not have ISO code.
    private static readonly FrozenSet<CountryCode> s_euVatAreaCountryAndSubdivisionCodes = FrozenSet.ToFrozenSet(
    [
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
    ]);

    private static readonly FrozenSet<CountryCode> s_euroZoneCodes = FrozenSet.ToFrozenSet(
    [
        Austria,
        Belgium,
        Croatia,
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
    ]);
}
