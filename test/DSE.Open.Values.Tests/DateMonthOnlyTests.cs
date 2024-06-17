// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Values.Tests;

public class DateMonthOnlyTests
{
    [Fact]
    public void Parse_empty_span_fails()
    {
        Assert.Throws<FormatException>(() => DateMonthOnly.Parse([], null));
    }

    [Fact]
    public void Parse_empty_string_fails()
    {
        Assert.Throws<FormatException>(() => DateMonthOnly.Parse("", null));
    }

    [Fact]
    public void Parse_null_string_fails()
    {
        Assert.Throws<ArgumentNullException>(() => DateMonthOnly.Parse(null!, null));
    }

    [Theory]
    [InlineData("2023-01")]
    [InlineData("2023-05-06")]
    [InlineData("9090-11")]
    public void Parse_valid_values_succeeds(string value)
    {
        _ = DateMonthOnly.Parse(value, CultureInfo.InvariantCulture);
    }

    [Theory]
    [InlineData("2023-01", "2023-01")]
    [InlineData("2023-05-06", "2023-05")]
    [InlineData("2001-1", "2001-01")]
    [InlineData("2001-1-1", "2001-01")]
    [InlineData("9090-11", "9090-11")]
    public void ToString_outputs_expected_format(string value, string expected)
    {
        var month = DateMonthOnly.Parse(value, CultureInfo.InvariantCulture);
        var str = month.ToString();
        Assert.Equal(expected, str);
    }

    [Theory]
    [InlineData("2023-01", "2022-01", 12)]
    [InlineData("2023-01", "2023-01", 0)]
    [InlineData("2022-01", "2023-01", -12)]
    [InlineData("2023-01", "2023-07", -6)]
    public void Subtraction_operator_returns_difference_in_months(string a, string b, int expectedDif)
    {
        var value = DateMonthOnly.Parse(a, CultureInfo.InvariantCulture) - DateMonthOnly.Parse(b, CultureInfo.InvariantCulture);
        Assert.Equal(expectedDif, value);
    }
}
