// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Values;

public class DateMonthOnlyTests
{
    [Fact]
    public void Constructor_normalizes_date_to_start_of_month()
    {
        var month = new DateMonthOnly(new DateOnly(2024, 2, 29));

        Assert.Equal(new DateOnly(2024, 2, 1), month.StartOfMonth);
        Assert.Equal(2024, month.Year);
        Assert.Equal(2, month.Month);
    }

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
        Assert.Equal(expectedDif, value.TotalMonths);
    }

    [Fact]
    public void Deconstruct_returns_year_and_month()
    {
        var month = DateMonthOnly.Parse("2024-02", CultureInfo.InvariantCulture);

        var (year, monthNumber) = month;

        Assert.Equal(2024, year);
        Assert.Equal(2, monthNumber);
    }

    [Fact]
    public void FromDateTimeOffset_uses_offset_date_year_and_month()
    {
        var date = new DateTimeOffset(2024, 12, 31, 23, 30, 0, TimeSpan.FromHours(-8));

        var month = DateMonthOnly.FromDateTimeOffset(date);

        Assert.Equal(new DateOnly(2024, 12, 1), month.StartOfMonth);
    }

    [Fact]
    public void AddMonths_and_AddYears_return_normalized_months()
    {
        var month = DateMonthOnly.Parse("2024-01", CultureInfo.InvariantCulture);

        Assert.Equal("2024-03", month.AddMonths(2).ToString());
        Assert.Equal("2025-01", month.AddYears(1).ToString());
    }

    [Fact]
    public void Comparison_operators_compare_start_of_month()
    {
        var earlier = DateMonthOnly.Parse("2024-01", CultureInfo.InvariantCulture);
        var later = DateMonthOnly.Parse("2024-02", CultureInfo.InvariantCulture);

        Assert.True(earlier < later);
        Assert.True(earlier <= later);
        Assert.True(later > earlier);
        Assert.True(later >= earlier);
        Assert.True(earlier <= DateMonthOnly.Parse("2024-01", CultureInfo.InvariantCulture));
        Assert.True(later >= DateMonthOnly.Parse("2024-02", CultureInfo.InvariantCulture));
    }

    [Fact]
    public void TryFormat_WithShortBuffer_ShouldReturnFalse()
    {
        var month = DateMonthOnly.Parse("2024-02", CultureInfo.InvariantCulture);
        Span<char> buffer = stackalloc char[6];

        var success = month.TryFormat(buffer, out var charsWritten);

        Assert.False(success);
        Assert.Equal(0, charsWritten);
    }
}
