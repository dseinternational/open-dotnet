// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Globalization;

namespace DSE.Open.Values;

public class YearDateTests
{
    // ------------------------------------------------------------------
    //  Construction
    // ------------------------------------------------------------------

    [Fact]
    public void Default_is_empty()
    {
        YearDate value = default;
        Assert.Equal(YearDate.Empty, value);
    }

    [Fact]
    public void Construct_with_year_only()
    {
        var yd = new YearDate(2024);
        Assert.Equal(2024, yd.Year);
        Assert.True(yd.HasYearOnly);
        Assert.False(yd.HasDayAndMonth);
        Assert.Null(yd.Month);
        Assert.Null(yd.Day);
        Assert.Null(yd.DayNumber);
        Assert.Null(yd.DayOfWeek);
        Assert.Null(yd.DayOfYear);
    }

    [Fact]
    public void Construct_with_year_month_day()
    {
        var yd = new YearDate(2024, 3, 15);
        Assert.Equal(2024, yd.Year);
        Assert.Equal(3, yd.Month);
        Assert.Equal(15, yd.Day);
        Assert.True(yd.HasDayAndMonth);
        Assert.False(yd.HasYearOnly);
        Assert.NotNull(yd.DayNumber);
        Assert.NotNull(yd.DayOfWeek);
        Assert.NotNull(yd.DayOfYear);
    }

    [Fact]
    public void Construct_with_DateOnly()
    {
        var date = new DateOnly(2024, 6, 1);
        var yd = new YearDate(date);
        Assert.Equal(2024, yd.Year);
        Assert.Equal(6, yd.Month);
        Assert.Equal(1, yd.Day);
        Assert.True(yd.HasDayAndMonth);
    }

    [Fact]
    public void Construct_with_DateTime()
    {
        var dt = new DateTime(2024, 12, 25, 14, 30, 0);
        var yd = new YearDate(dt);
        Assert.Equal(2024, yd.Year);
        Assert.Equal(12, yd.Month);
        Assert.Equal(25, yd.Day);
        Assert.True(yd.HasDayAndMonth);
    }

    [Fact]
    public void Construct_with_invalid_year_throws()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new YearDate(0));
    }

    [Fact]
    public void Construct_with_invalid_month_throws()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new YearDate(2024, 13, 1));
    }

    [Fact]
    public void Construct_with_invalid_day_throws()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new YearDate(2024, 2, 30));
    }

    // ------------------------------------------------------------------
    //  Conversions
    // ------------------------------------------------------------------

    [Fact]
    public void ToDateOnly_returns_underlying_date()
    {
        var yd = new YearDate(2024, 7, 4);
        var date = yd.ToDateOnly();
        Assert.Equal(new DateOnly(2024, 7, 4), date);
    }

    [Fact]
    public void ToDateTime_returns_date_at_midnight()
    {
        var yd = new YearDate(2024, 7, 4);
        var dt = yd.ToDateTime();
        Assert.Equal(new DateTime(2024, 7, 4, 0, 0, 0), dt);
    }

    [Fact]
    public void ToDateOnly_year_only_returns_jan_1()
    {
        var yd = new YearDate(2024);
        var date = yd.ToDateOnly();
        Assert.Equal(new DateOnly(2024, 1, 1), date);
    }

    // ------------------------------------------------------------------
    //  Formatting
    // ------------------------------------------------------------------

    [Fact]
    public void ToString_full_date_formats_as_iso()
    {
        var yd = new YearDate(2024, 3, 15);
        Assert.Equal("2024-03-15", yd.ToString());
    }

    [Fact]
    public void ToString_year_only_formats_as_four_digit_year()
    {
        var yd = new YearDate(2024);
        Assert.Equal("2024", yd.ToString());
    }

    [Fact]
    public void ToString_year_only_pads_to_four_digits()
    {
        var yd = new YearDate(42);
        Assert.Equal("0042", yd.ToString());
    }

    [Fact]
    public void TryFormat_full_date_to_span()
    {
        var yd = new YearDate(2024, 1, 9);
        Span<char> buffer = stackalloc char[20];
        Assert.True(yd.TryFormat(buffer, out var written, [], CultureInfo.InvariantCulture));
        Assert.Equal("2024-01-09", buffer[..written].ToString());
    }

    [Fact]
    public void TryFormat_year_only_to_span()
    {
        var yd = new YearDate(1999);
        Span<char> buffer = stackalloc char[10];
        Assert.True(yd.TryFormat(buffer, out var written, [], CultureInfo.InvariantCulture));
        Assert.Equal("1999", buffer[..written].ToString());
    }

    // ------------------------------------------------------------------
    //  Parsing
    // ------------------------------------------------------------------

    [Fact]
    public void Parse_iso_date_string()
    {
        var yd = YearDate.Parse("2024-03-15", CultureInfo.InvariantCulture);
        Assert.Equal(2024, yd.Year);
        Assert.Equal(3, yd.Month);
        Assert.Equal(15, yd.Day);
        Assert.True(yd.HasDayAndMonth);
    }

    [Fact]
    public void Parse_year_only_string()
    {
        var yd = YearDate.Parse("2024", CultureInfo.InvariantCulture);
        Assert.Equal(2024, yd.Year);
        Assert.True(yd.HasYearOnly);
    }

    [Fact]
    public void Parse_invalid_string_throws()
    {
        Assert.Throws<FormatException>(() => YearDate.Parse("not-a-date", CultureInfo.InvariantCulture));
    }

    [Fact]
    public void Parse_null_throws()
    {
        Assert.Throws<ArgumentNullException>(() => YearDate.Parse(null!, null));
    }

    [Fact]
    public void TryParse_empty_span_returns_empty()
    {
        Assert.True(YearDate.TryParse([], null, out var result));
        Assert.Equal(YearDate.Empty, result);
    }

    [Fact]
    public void TryParse_null_string_returns_false()
    {
        Assert.False(YearDate.TryParse(null, out var result));
        Assert.Equal(YearDate.Empty, result);
    }

    [Fact]
    public void TryParse_four_digit_non_numeric_returns_false()
    {
        Assert.False(YearDate.TryParse("abcd".AsSpan(), null, out _));
    }

    [Fact]
    public void TryParse_valid_iso_date()
    {
        Assert.True(YearDate.TryParse("2000-12-31", out var result));
        Assert.Equal(2000, result.Year);
        Assert.Equal(12, result.Month);
        Assert.Equal(31, result.Day);
    }

    [Fact]
    public void TryParse_valid_year_only()
    {
        Assert.True(YearDate.TryParse("1990", out var result));
        Assert.Equal(1990, result.Year);
        Assert.True(result.HasYearOnly);
    }

    [Fact]
    public void TryParse_invalid_returns_false()
    {
        Assert.False(YearDate.TryParse("xyz", out _));
    }

    // ------------------------------------------------------------------
    //  Comparison
    // ------------------------------------------------------------------

    [Fact]
    public void CompareTo_earlier_date_is_less()
    {
        var earlier = new YearDate(2020, 1, 1);
        var later = new YearDate(2024, 1, 1);
        Assert.True(earlier.CompareTo(later) < 0);
    }

    [Fact]
    public void CompareTo_same_date_is_zero()
    {
        var a = new YearDate(2024, 6, 15);
        var b = new YearDate(2024, 6, 15);
        Assert.Equal(0, a.CompareTo(b));
    }

    [Fact]
    public void Operator_less_than()
    {
        var a = new YearDate(2020);
        var b = new YearDate(2024);
        Assert.True(a < b);
        Assert.False(b < a);
    }

    [Fact]
    public void Operator_greater_than()
    {
        var a = new YearDate(2024);
        var b = new YearDate(2020);
        Assert.True(a > b);
        Assert.False(b > a);
    }

    [Fact]
    public void Operator_less_than_or_equal()
    {
        var a = new YearDate(2020);
        var b = new YearDate(2024);
        var c = new YearDate(2020);
        Assert.True(a <= b);
        Assert.True(a <= c);
    }

    [Fact]
    public void Operator_greater_than_or_equal()
    {
        var a = new YearDate(2024);
        var b = new YearDate(2020);
        var c = new YearDate(2024);
        Assert.True(a >= b);
        Assert.True(a >= c);
    }

    // ------------------------------------------------------------------
    //  Equality (record struct)
    // ------------------------------------------------------------------

    [Fact]
    public void Equal_full_dates_are_equal()
    {
        var a = new YearDate(2024, 3, 15);
        var b = new YearDate(2024, 3, 15);
        Assert.Equal(a, b);
        Assert.True(a == b);
        Assert.False(a != b);
    }

    [Fact]
    public void Different_dates_are_not_equal()
    {
        var a = new YearDate(2024, 3, 15);
        var b = new YearDate(2024, 3, 16);
        Assert.NotEqual(a, b);
    }

    [Fact]
    public void Year_only_and_full_date_differ_when_same_year()
    {
        var yearOnly = new YearDate(2024);
        var fullDate = new YearDate(2024, 1, 1);
        // They differ because _hasDayAndMonth is part of the record
        Assert.NotEqual(yearOnly, fullDate);
    }

    // ------------------------------------------------------------------
    //  DayOfWeek
    // ------------------------------------------------------------------

    [Fact]
    public void DayOfWeek_returns_correct_value()
    {
        // 2024-03-15 is a Friday
        var yd = new YearDate(2024, 3, 15);
        Assert.Equal(DayOfWeek.Friday, yd.DayOfWeek);
    }
}
