// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Globalization.Tests;

public class TelephoneNumberTests
{
    [Theory]
    [MemberData(nameof(ValidNumbers))]
    public void Initialize(uint countryCode, ulong nationalNumber)
    {
        var t = new TelephoneNumber(countryCode, nationalNumber);
        Assert.Equal(countryCode, t.CountryCode);
        Assert.Equal(nationalNumber, t.NationalNumber);
    }

    [Theory]
    [MemberData(nameof(ValidCountryCallingCodes))]
    public void IsValidCountryCallingCodeReturnsTrueForAssignedCodes(uint code)
    {
        Assert.True(TelephoneNumber.IsValidCountryCallingCode(code));
    }

    [Theory]
    [MemberData(nameof(ValidNumberStrings))]
    public void TryParseSpan(string stringCode, uint countryCode, ulong nationalNumber)
    {
        var success = TelephoneNumber.TryParse(stringCode.AsSpan(), null, out var t);
        Assert.True(success);
        Assert.Equal(countryCode, t.CountryCode);
        Assert.Equal(nationalNumber, t.NationalNumber);
        Assert.Equal(t, t);
    }

    [Theory]
    [MemberData(nameof(ValidNumberStrings))]
    public void TryParseString(string stringCode, uint countryCode, ulong nationalNumber)
    {
        var success = TelephoneNumber.TryParse(stringCode, null, out var t);
        Assert.True(success);
        Assert.Equal(countryCode, t.CountryCode);
        Assert.Equal(nationalNumber, t.NationalNumber);
        Assert.Equal(t, t);
    }

    public static readonly TheoryData<uint, ulong> ValidNumbers = new()
    {
        { 44u, 3300430021ul },
        { 1u, 8442828620ul },
    };

    public static readonly TheoryData<string, uint, ulong> ValidNumberStrings = new()
    {
        { "+44 3300430021", 44u, 3300430021ul },
        { "443300430021", 44u, 3300430021ul },
        { "44 3300430021", 44u, 3300430021ul },
    };

    public static TheoryData<uint> ValidCountryCallingCodes()
    {
        var data = new TheoryData<uint>();
        foreach (var code in CountryCallingCodeInfo.AssignedCodes)
        {
            data.Add(code);
        }

        return data;
    }
}
