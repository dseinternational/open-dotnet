// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;

namespace DSE.Open.Globalization.Tests;

public class CountryCodeTests
{
    [Theory]
    [MemberData(nameof(OfficiallyAssignedAlpha2Codes))]
    public static void TryFromValue_succeeds_for_officially_assigned(AsciiChar2 code)
    {
        Assert.True(CountryCode.TryFromValue(code, out _));
    }

    [Theory]
    [MemberData(nameof(UserAssignedAlpha2Codes))]
    public static void TryFromValue_fails_for_user_assigned(AsciiChar2 code)
    {
        Assert.False(CountryCode.TryFromValue(code, out _));
    }

    [Fact]
    public void Serialize_deserialize()
    {
        var v1 = CountryCode.Canada;
        var json = JsonSerializer.Serialize(v1);
        Assert.Equal("\"CA\"", json);
        var v2 = JsonSerializer.Deserialize<CountryCode>(json);
        Assert.Equal(v1, v2);
    }

    public static TheoryData<AsciiChar2> OfficiallyAssignedAlpha2Codes
    {
        get
        {
            var data = new TheoryData<AsciiChar2>();
            foreach (var code in IsoCountryCodes.OfficiallyAssignedAlpha2Ascii)
            {
                data.Add(code);
            }
            return data;
        }
    }

    public static TheoryData<AsciiChar2> UserAssignedAlpha2Codes
    {
        get
        {
            var data = new TheoryData<AsciiChar2>();
            foreach (var code in IsoCountryCodes.UserAssignedAlpha2Ascii)
            {
                data.Add(code);
            }
            return data;
        }
    }


    [Fact]
    public void Valid_CountryCode_Parse()
    {
        foreach (var code in s_validCountryCodes)
        {
            var parsed = CountryCode.Parse(code);
            Assert.Equal(code, parsed.ToString());
        }
    }

    [Fact]
    public void EU_Country_Code_IsEuMemberCountry()
    {
        foreach (var code in s_euMemberCountryCodes)
        {
            Assert.True(CountryCode.IsEuMemberCountry(code));
        }
    }

    [Fact]
    public void EU_Country_Code_IsEuMemberCountry_Parsed()
    {
        foreach (var code in s_euMemberCountryCodes)
        {
            var parsed = CountryCode.Parse(code);
            Assert.True(CountryCode.IsEuMemberCountry(parsed));
        }
    }

    [Fact]
    public void EU_Country_Subdivision_Code_IsEuMemberCountry_Parsed()
    {
        foreach (var code in s_euMemberCountryAndSubdivisionCodes)
        {
            var parsed = CountryCode.Parse(code);
            Assert.True(CountryCode.IsEuMemberCountryOrSubdivision(parsed), code);
        }
    }

    [Fact]
    public void TryFormat_WithValidCode_ShouldReturnTrue()
    {
        // Arrange
        var countryCode = CountryCode.India;
        Span<char> destination = stackalloc char[2];

        // Act
        var result = countryCode.TryFormat(destination, out var charsWritten, null, null);

        // Assert
        Assert.True(result);
        Assert.Equal(2, charsWritten);
        Assert.Equal(countryCode.ToString(), new string(destination));
    }
    /*
    [Fact]
    public void Parse_WithEmptySpan_ShouldReturnDefault()
    {
        var a = CountryCode.Parse(Span<char>.Empty, null);
        Assert.Equal(CountryCode.Empty, a);
    }

    [Fact]
    public void Parse_WithEmptyString_ShouldReturnDefault()
    {
        var a = CountryCode.Parse(string.Empty);
        Assert.Equal(CountryCode.Empty, a);
    }
    */
    [Fact]
    public void Parse_WithNullString_ShouldThrowArgumentNull() => Assert.Throws<ArgumentNullException>(() => CountryCode.Parse(null!));

    /*
    [Fact]
    public void TryParse_WithEmptySpan_ShouldReturnTrueAndDefaultResult()
    {
        var success = CountryCode.TryParse(Span<char>.Empty, null, out var result);
        Assert.True(success);
        Assert.Equal(CountryCode.Empty, result);
    }

    [Fact]
    public void TryParse_WithNullString_ShouldReturnFalseAndDefaultResult()
    {
        var success = CountryCode.TryParse(null, out var result);
        Assert.False(success);
        Assert.Equal(CountryCode.Empty, result);
    }

    [Fact]
    public void TryParse_WithEmptyString_ShouldReturnTrueAndDefaultResult()
    {
        var success = CountryCode.TryParse(string.Empty, out var result);
        Assert.True(success);
        Assert.Equal(CountryCode.Empty, result);
    }
    */
    [Fact]
    public void TryFormat_WithInvalidBuffer_ShouldReturnFalse()
    {
        // Arrange
        var countryCode = CountryCode.Australia;
        Span<char> destination = stackalloc char[1];

        // Act
        var result = countryCode.TryFormat(destination, out var charsWritten, null, null);

        // Assert
        Assert.False(result);
        Assert.Equal(0, charsWritten);
    }

    [Fact]
    public void EqualValuesAreEqual()
    {
        var v1 = CountryCode.Parse("GB");
        var v2 = CountryCode.Parse(v1.ToString());
        Assert.Equal(v1, v2);
    }

    [Fact]
    public void EqualValuesAsObjectsAreEqual()
    {
        var v1 = (object)CountryCode.Parse("GB");
        var v2 = (object)CountryCode.Parse(v1.ToString()!);
        Assert.Equal(v1, v2);
    }

    internal static readonly string[] s_validCountryCodes = {
            "AE",
            "AF",
            "AG",
            "AI",
            "AL",
            "AM",
            "AO",
            "AQ",
            "AR",
            "AS",
            "AT",
            "AU",
            "AW",
            "AX",
            "AZ",
            "BA",
            "BB",
            "BD",
            "BE",
            "BF",
            "BG",
            "BH",
            "BI",
            "BJ",
            "BL",
            "BM",
            "BN",
            "BO",
            "BQ",
            "BR",
            "BS",
            "BT",
            "BV",
            "BW",
            "BY",
            "BZ",
            "CA",
            "CC",
            "CD",
            "CF",
            "CG",
            "CH",
            "CI",
            "CK",
            "CL",
            "CM",
            "CN",
            "CO",
            "CR",
            "CU",
            "CV",
            "CW",
            "CX",
            "CY",
            "CZ",
            "DE",
            "DJ",
            "DK",
            "DM",
            "DO",
            "DZ",
            "EC",
            "EE",
            "EG",
            "EH",
            "ER",
            "ES",
            "ET",
            "FI",
            "FJ",
            "FK",
            "FM",
            "FO",
            "FR",
            "GA",
            "GB",
            "GD",
            "GE",
            "GF",
            "GG",
            "GH",
            "GI",
            "GL",
            "GM",
            "GN",
            "GP",
            "GQ",
            "GR",
            "GS",
            "GT",
            "GU",
            "GW",
            "GY",
            "HK",
            "HM",
            "HN",
            "HR",
            "HT",
            "HU",
            "ID",
            "IE",
            "IL",
            "IM",
            "IN",
            "IO",
            "IQ",
            "IR",
            "IS",
            "IT",
            "JE",
            "JM",
            "JO",
            "JP",
            "KE",
            "KG",
            "KH",
            "KI",
            "KM",
            "KN",
            "KP",
            "KR",
            "KW",
            "KY",
            "KZ",
            "LA",
            "LB",
            "LC",
            "LI",
            "LK",
            "LR",
            "LS",
            "LT",
            "LU",
            "LV",
            "LY",
            "MA",
            "MC",
            "MD",
            "ME",
            "MF",
            "MG",
            "MH",
            "MK",
            "ML",
            "MM",
            "MN",
            "MO",
            "MP",
            "MQ",
            "MR",
            "MS",
            "MT",
            "MU",
            "MV",
            "MW",
            "MX",
            "MY",
            "MZ",
            "NA",
            "NC",
            "NE",
            "NF",
            "NG",
            "NI",
            "NL",
            "NO",
            "NP",
            "NR",
            "NU",
            "NZ",
            "OM",
            "PA",
            "PE",
            "PF",
            "PG",
            "PH",
            "PK",
            "PL",
            "PM",
            "PN",
            "PR",
            "PS",
            "PT",
            "PW",
            "PY",
            "QA",
            "RE",
            "RO",
            "RS",
            "RU",
            "RW",
            "SA",
            "SB",
            "SC",
            "SD",
            "SE",
            "SG",
            "SH",
            "SI",
            "SJ",
            "SK",
            "SL",
            "SM",
            "SN",
            "SO",
            "SR",
            "SS",
            "ST",
            "SV",
            "SX",
            "SY",
            "SZ",
            "TC",
            "TD",
            "TF",
            "TG",
            "TH",
            "TJ",
            "TK",
            "TL",
            "TM",
            "TN",
            "TO",
            "TR",
            "TT",
            "TV",
            "TW",
            "TZ",
            "UA",
            "UG",
            "UM",
            "US",
            "UY",
            "UZ",
            "VA",
            "VC",
            "VE",
            "VG",
            "VI",
            "VN",
            "VU",
            "WF",
            "WS",
            "YE",
            "YT",
            "ZA",
            "ZM",
            "ZW"
    };

    internal static readonly string[] s_euMemberCountryCodes = {
            "AT",
            "BE",
            "BG",
            "CY",
            "CZ",
            "DE",
            "DK",
            "EE",
            "ES",
            "FI",
            "FR",
            "GR",
            "HR",
            "HU",
            "IE",
            "IT",
            "LT",
            "LU",
            "LV",
            "MT",
            "NL",
            "PL",
            "PT",
            "RO",
            "SE",
            "SI",
            "SK"
    };

    // https://en.wikipedia.org/wiki/Member_state_of_the_European_Union
    //
    private static readonly string[] s_euMemberCountryAndSubdivisionCodes = {
            "AT",
            "AX", // Åland Islands (Finland)
            "BE",
            "BG",
            "CY",
            "CZ",
            "DE",
            "DK",
            "EE",
            "ES", // (includes Canary Islands, Ceuta and Melilla)
            "FI",
            "FR",
            "GF", // French Guiana
            "GP", // Guadeloupe (France)
            "GR",
            "HR",
            "HU",
            "IE",
            "IT",
            "LT",
            "LU",
            "LV",
            "MF", // Saint Martin (France)
            "MQ", // Martinique (France)
            "MT",
            "NL",
            "PL",
            "PT", // (Includes Azores and Madeira)
            "RE", // Réunion (France)
            "RO",
            "SE",
            "SI",
            "SK",
            "YT" // Mayotte (France)
    };
}
