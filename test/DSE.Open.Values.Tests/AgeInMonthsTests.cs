// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;

namespace DSE.Open.Values;

public class AgeInMonthsTests
{
    [Fact]
    public void Constructor_sets_total_months()
    {
        var age = new AgeInMonths(1);
        Assert.Equal(1, age.TotalMonths);
    }

    [Fact]
    public void Constructor_sets_total_months_from_year()
    {
        var age = new AgeInMonths(1, 0);
        Assert.Equal(12, age.TotalMonths);
    }

    [Theory]
    [InlineData(1, 0, "1:0")]
    [InlineData(0, 1, "0:1")]
    [InlineData(0, 13, "1:1")]
    [InlineData(10, 24, "12:0")]
    [InlineData(178956970, 0, "178956970:0")]
    [InlineData(-178956970, 0, "-178956970:0")]
    [InlineData(0, int.MaxValue, "178956970:7")]
    public void ToString_outputs_correct_format(int years, int months, string expected)
    {
        var age = new AgeInMonths(years, months);
        Assert.Equal(expected, age.ToString());
    }

    [Theory]
    [InlineData("0:1", 1)]
    [InlineData("1:0", 12)]
    [InlineData("10:1", 121)]
    [InlineData("-500:1", -5999)]
    public void Parse_parses_correctly(string value, int expectedTotalMonths)
    {
        var age = AgeInMonths.Parse(value, null);
        Assert.Equal(expectedTotalMonths, age.TotalMonths);
    }

    [Fact]
    public void Serialize_deserialize()
    {
        var age = new AgeInMonths(1, 2);
        var json = JsonSerializer.Serialize(age);
        var age2 = JsonSerializer.Deserialize<AgeInMonths>(json);
        Assert.Equal(age, age2);

    }
}
