// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;

namespace DSE.Open.Globalization.Tests;

public class CountryCode3Tests
{
    [Theory]
    [MemberData(nameof(OfficiallyAssignedAlpha3Codes))]
    public static void TryFromValue_succeeds_for_officially_assigned(AsciiChar3 code)
    {
        Assert.True(CountryCode3.TryFromValue(code, out _));
    }

    [Fact]
    public void Serialize_deserialize()
    {
        var v1 = CountryCode3.Canada;
        var json = JsonSerializer.Serialize(v1);
        Assert.Equal("\"CAN\"", json);
        var v3 = JsonSerializer.Deserialize<CountryCode3>(json);
        Assert.Equal(v1, v3);
    }

    public static TheoryData<AsciiChar3> OfficiallyAssignedAlpha3Codes
    {
        get
        {
            var data = new TheoryData<AsciiChar3>();
            foreach (var code in IsoCountryCodes.OfficiallyAssignedAlpha3Ascii)
            {
                data.Add(code);
            }
            return data;
        }
    }
}
