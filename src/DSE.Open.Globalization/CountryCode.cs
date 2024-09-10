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
    public const int Length = 2;

    public static int MaxSerializedCharLength => Length;

    public static int MaxSerializedByteLength => Length;

    public int CompareTo(CountryCode other)
    {
        return _value.CompareToIgnoreCase(other._value);
    }

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

    public char[] ToCharArray()
    {
        return _value.ToCharArray();
    }

    public string ToStringLower()
    {
        return _value.ToStringLower();
    }

    public string ToStringUpper()
    {
        return _value.ToStringUpper();
    }

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

    public AsciiChar2 ToAsciiChar2()
    {
        return _value;
    }

    public static CountryCode FromAsciiChar2(AsciiChar2 value)
    {
        return value.CastToValue<CountryCode, AsciiChar2>();
    }

#pragma warning disable CA2225 // Operator overloads have named alternates

    public static explicit operator CountryCode(string code)
    {
        return Parse(code, CultureInfo.InvariantCulture);
    }

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

    public static bool IsEuroZoneCountry(string code)
    {
        return code is null
            ? throw new ArgumentNullException(nameof(code))
            : IsEuroZoneCountry(Parse(code.ToUpperInvariant(), CultureInfo.InvariantCulture));
    }

    public static bool IsEuroZoneCountry(CountryCode code)
    {
        return s_euroZoneCodes.Contains(code);
    }

    public static readonly CountryCode Andorra = new(new('A', 'D'), true);
    public static readonly CountryCode UnitedArabEmirates = new(new('A', 'E'), true);
    public static readonly CountryCode Austria = new(new('A', 'T'), true);
    public static readonly CountryCode Australia = new(new('A', 'U'), true);
    public static readonly CountryCode Belgium = new(new('B', 'E'), true);
    public static readonly CountryCode Brazil = new(new('B', 'R'), true);
    public static readonly CountryCode Bulgaria = new(new('B', 'G'), true);
    public static readonly CountryCode Canada = new(new('C', 'A'), true);
    public static readonly CountryCode Switzerland = new(new('C', 'H'), true);
    public static readonly CountryCode China = new(new('C', 'N'), true);
    public static readonly CountryCode Cyprus = new(new('C', 'Y'), true);
    public static readonly CountryCode CzechRepublic = new(new('C', 'Z'), true);
    public static readonly CountryCode Germany = new(new('D', 'E'), true);
    public static readonly CountryCode Denmark = new(new('D', 'K'), true);
    public static readonly CountryCode Estonia = new(new('E', 'E'), true);
    public static readonly CountryCode Spain = new(new('E', 'S'), true);
    public static readonly CountryCode Finland = new(new('F', 'I'), true);
    public static readonly CountryCode France = new(new('F', 'R'), true);
    public static readonly CountryCode UnitedKingdom = new(new('G', 'B'), true);
    public static readonly CountryCode Gibraltar = new(new('G', 'I'), true);
    public static readonly CountryCode Greece = new(new('G', 'R'), true);
    public static readonly CountryCode HongKongSAR = new(new('H', 'K'), true);
    public static readonly CountryCode Croatia = new(new('H', 'R'), true);
    public static readonly CountryCode Hungary = new(new('H', 'U'), true);
    public static readonly CountryCode Ireland = new(new('I', 'E'), true);
    public static readonly CountryCode Israel = new(new('I', 'L'), true);
    public static readonly CountryCode IsleOfMan = new(new('I', 'M'), true);
    public static readonly CountryCode India = new(new('I', 'N'), true);
    public static readonly CountryCode Italy = new(new('I', 'T'), true);
    public static readonly CountryCode Jersey = new(new('J', 'E'), true);
    public static readonly CountryCode Japan = new(new('J', 'P'), true);
    public static readonly CountryCode Liechtenstein = new(new('L', 'I'), true);
    public static readonly CountryCode Lithuania = new(new('L', 'T'), true);
    public static readonly CountryCode Luxembourg = new(new('L', 'U'), true);
    public static readonly CountryCode Latvia = new(new('L', 'V'), true);
    public static readonly CountryCode Monaco = new(new('M', 'C'), true);
    public static readonly CountryCode Montenegro = new(new('M', 'E'), true);
    public static readonly CountryCode Malta = new(new('M', 'T'), true);
    public static readonly CountryCode Mexico = new(new('M', 'X'), true);
    public static readonly CountryCode Netherlands = new(new('N', 'L'), true);
    public static readonly CountryCode Norway = new(new('N', 'O'), true);
    public static readonly CountryCode NewZealand = new(new('N', 'Z'), true);
    public static readonly CountryCode Poland = new(new('P', 'L'), true);
    public static readonly CountryCode Portugal = new(new('P', 'T'), true);
    public static readonly CountryCode Romania = new(new('R', 'O'), true);
    public static readonly CountryCode Serbia = new(new('R', 'S'), true);
    public static readonly CountryCode Russia = new(new('R', 'U'), true);
    public static readonly CountryCode SaudiArabia = new(new('S', 'A'), true);
    public static readonly CountryCode Sweden = new(new('S', 'E'), true);
    public static readonly CountryCode Singapore = new(new('S', 'G'), true);
    public static readonly CountryCode Slovenia = new(new('S', 'I'), true);
    public static readonly CountryCode Slovakia = new(new('S', 'K'), true);
    public static readonly CountryCode SanMarino = new(new('S', 'M'), true);
    public static readonly CountryCode Turkey = new(new('T', 'R'), true);
    public static readonly CountryCode Ukraine = new(new('U', 'A'), true);
    public static readonly CountryCode UnitedStates = new(new('U', 'S'), true);
    public static readonly CountryCode Kosovo = new(new('X', 'K'), true);
    public static readonly CountryCode SouthAfrica = new(new('Z', 'A'), true);

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
