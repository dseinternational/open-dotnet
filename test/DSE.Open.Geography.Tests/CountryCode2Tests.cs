// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;
using DSE.Open.Geography;

namespace DSE.Open.Values.Tests.Geography;

public class CountryCode2Tests
{
    [Theory]
    [MemberData(nameof(OfficiallyAssignedAlpha2Codes))]
    public static void TryFromValue_succeeds_for_officially_assigned(AsciiChar2 code)
    {
        Assert.True(CountryCode2.TryFromValue(code, out _));
    }

    [Theory]
    [MemberData(nameof(UserAssignedAlpha2Codes))]
    public static void TryFromValue_fails_for_user_assigned(AsciiChar2 code)
    {
        Assert.False(CountryCode2.TryFromValue(code, out _));
    }

    [Fact]
    public void Serialize_deserialize()
    {
        var v1 = CountryCode2.Canada;
        var json = JsonSerializer.Serialize(v1);
        Assert.Equal("\"CA\"", json);
        var v2 = JsonSerializer.Deserialize<CountryCode2>(json);
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
}