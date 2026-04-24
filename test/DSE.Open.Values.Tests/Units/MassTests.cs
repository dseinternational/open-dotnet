// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Values.Units;

public class MassTests
{
    [Fact]
    public void Initialize()
    {
        var mass = Mass.FromKilograms(2.548);
        Assert.Equal(2548, mass.Amount);
    }

    [Fact]
    public void Equal()
    {
        var mass1 = Mass.FromKilograms(2.548);
        var mass2 = Mass.FromKilograms(2.548);
        Assert.True(mass1.Equals(mass2));
    }

    [Fact]
    public void Equal_2()
    {
        var mass1 = Mass.FromKilograms(2.548);
        var mass2 = Mass.FromGrams(2548);
        Assert.True(mass1.Equals(mass2));
    }

    [Fact]
    public void Equal_obj()
    {
        var mass1 = Mass.FromKilograms(2.548);
        object mass2 = Mass.FromGrams(2548);
        Assert.True(mass1.Equals(mass2));
    }

    [Theory]
    [MemberData(nameof(ParseData))]
    public void TryParse(string value, double massInGrams)
    {
        var expected = Mass.FromGrams(massInGrams);
        Assert.True(Mass.TryParse(value, out var parsed));
        Assert.Equal(expected, parsed);
    }

    [Theory]
    [InlineData("16oz", 453.59237)]
    [InlineData("1lb", 453.59237)]
    [InlineData("1000mg", 1)]
    public void TryParse_WithSupportedNonDefaultUnits_ShouldConvertToGrams(string value, double massInGrams)
    {
        Assert.True(Mass.TryParse(value, NumberStyles.Number, CultureInfo.InvariantCulture, out var parsed));

        Assert.Equal(massInGrams, parsed.Grams, 5);
    }

    [Fact]
    public void TryParse_WithCustomFormatProvider_ShouldRespectDecimalSeparator()
    {
        var culture = CultureInfo.GetCultureInfo("fr-FR");

        Assert.True(Mass.TryParse("1,5 kg", NumberStyles.Number, culture, out var parsed));

        Assert.Equal(1500, parsed.Grams);
    }

    [Fact]
    public void Parse_WithInvalidValue_ShouldThrowFormatException()
    {
        _ = Assert.Throws<FormatException>(() => Mass.Parse("not-a-mass"));
    }

    [Fact]
    public void ToString_WithTargetUnit_ShouldFormatConvertedValue()
    {
        var mass = Mass.FromGrams(1000);

        Assert.Equal("1kg", mass.ToString(null, CultureInfo.InvariantCulture, UnitOfMass.Kilogram));
    }

    [Fact]
    public void ToString_WithNullUnit_ShouldThrowArgumentNullException()
    {
        var mass = Mass.FromGrams(1000);

        _ = Assert.Throws<ArgumentNullException>(() => mass.ToString(null, CultureInfo.InvariantCulture, null!));
    }

    public static TheoryData<string, double> ParseData => new()
    {
        { "48615.256 g", 48615.256 },
        { "48615.256 kg", 48615256 },
        { "-10000 g", -10000 },
        { "-1 kg", -1000 },
        { "48615.256g", 48615.256 },
        { "48615.256kg", 48615256 },
        { "-10000g", -10000 },
        { "-1kg", -1000 }
    };
}
