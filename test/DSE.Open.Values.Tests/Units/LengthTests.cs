// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Values.Units;

public class LengthTests
{
    [Fact]
    public void Initialize_mm()
    {
        var length = Length.FromMillimetres(100.256);
        Assert.Equal(100.256, length.Amount);
        Assert.Equal(100.256, length.Millimetres);
        Assert.Equal(10.0256, length.Centimetres);
    }

    [Fact]
    public void Initialize_cm()
    {
        var length = Length.FromCentimetres(100.256);
        Assert.Equal(100.256, length.Centimetres);
    }

    [Fact]
    public void Equal()
    {
        var length1 = Length.FromCentimetres(100.256);
        var length2 = Length.FromCentimetres(100.256);
        Assert.True(length1.Equals(length2));
    }

    [Fact]
    public void Equal_2()
    {
        var length1 = Length.FromCentimetres(100.256);
        var length2 = Length.FromMetres(1.00256);
        Assert.True(length1.Equals(length2));
    }

    [Fact]
    public void Equal_obj()
    {
        var length1 = Length.FromCentimetres(100.256);
        object length2 = Length.FromMetres(1.00256);
        Assert.True(length1.Equals(length2));
    }

    [Fact]
    public void ToString_default()
    {
        var length = Length.FromCentimetres(100.256);
        var output = length.ToString();
        Assert.Equal("1002.56 mm", output);
    }

    [Fact]
    public void ToString_invariant()
    {
        var length = Length.FromCentimetres(100.256);
        var output = length.ToStringInvariant();
        Assert.Equal("1002.56 mm", output);
    }

    [Fact]
    public void ToString_convert_m()
    {
        var length = Length.FromCentimetres(100.256);
        var output = length.ToString(UnitOfLength.Metre);
        Assert.Equal("1.00256 m", output);
    }

    [Fact]
    public void ToString_convert_km()
    {
        var length = Length.FromCentimetres(100.256);
        var output = length.ToString(UnitOfLength.Kilometre);
        Assert.Equal("0.00100256 km", output);
    }

    [Theory]
    [MemberData(nameof(ParseData))]
    public void TryParse(string value, double lengthInCm)
    {
        var expected = Length.FromCentimetres(lengthInCm);
        Assert.True(Length.TryParse(value, out var parsed));
        Assert.Equal(expected, parsed);
    }

    public static TheoryData<string, double> ParseData => new()
    {
        { "48615.256 cm", 48615.256 },
        { "48615.256 m", 4861525.6 },
        { "-10000 mm", -1000.0 },
        { "-1 mm", -0.1 }
    };
}
