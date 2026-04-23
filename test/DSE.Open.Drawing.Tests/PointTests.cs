// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Drawing.Tests;

public class PointTests
{
    // ------------------------------------------------------------------
    //  TryParse
    // ------------------------------------------------------------------

    [Theory]
    [InlineData("1,1", 1, 1)]
    [InlineData("1.1,100.0001", 1.1, 100.0001)]
    [InlineData("-5.5,3.3", -5.5, 3.3)]
    [InlineData("0,0", 0, 0)]
    public void TryParse_valid(string value, double expectedX, double expectedY)
    {
        Assert.True(Point.TryParse(value, out var point));
        Assert.Equal(expectedX, point.X);
        Assert.Equal(expectedY, point.Y);
    }

    [Fact]
    public void TryParse_null_returns_false()
    {
        Assert.False(Point.TryParse(null, out _));
    }

    [Fact]
    public void TryParse_empty_returns_false()
    {
        Assert.False(Point.TryParse("", out _));
    }

    [Fact]
    public void TryParse_no_comma_returns_false()
    {
        Assert.False(Point.TryParse("123", out _));
    }

    [Fact]
    public void TryParse_comma_at_start_returns_false()
    {
        Assert.False(Point.TryParse(",5", out _));
    }

    [Fact]
    public void TryParse_non_numeric_returns_false()
    {
        Assert.False(Point.TryParse("abc,def", out _));
    }

    // ------------------------------------------------------------------
    //  Parse
    // ------------------------------------------------------------------

    [Fact]
    public void Parse_valid()
    {
        var p = Point.Parse("10.5,20.5");
        Assert.Equal(10.5, p.X);
        Assert.Equal(20.5, p.Y);
    }

    [Fact]
    public void Parse_null_throws()
    {
        Assert.Throws<ArgumentNullException>(() => Point.Parse(null!));
    }

    [Fact]
    public void Parse_invalid_throws_FormatException()
    {
        Assert.Throws<FormatException>(() => Point.Parse("invalid"));
    }

    // ------------------------------------------------------------------
    //  TryParseCollection
    // ------------------------------------------------------------------

    [Fact]
    public void TryParseCollection_space_separated()
    {
        Assert.True(Point.TryParseCollection("1,2 3,4 5,6", out var points));
        Assert.Equal(3, points.Count);
        Assert.Equal(new Point(1, 2), points[0]);
        Assert.Equal(new Point(3, 4), points[1]);
        Assert.Equal(new Point(5, 6), points[2]);
    }

    [Fact]
    public void TryParseCollection_null_returns_empty()
    {
        Assert.True(Point.TryParseCollection((string?)null, out var points));
        Assert.Empty(points);
    }

    [Fact]
    public void TryParseCollection_empty_returns_empty()
    {
        Assert.True(Point.TryParseCollection("", out var points));
        Assert.Empty(points);
    }

    [Fact]
    public void TryParseCollection_whitespace_returns_empty()
    {
        Assert.True(Point.TryParseCollection("   ", out var points));
        Assert.Empty(points);
    }

    [Fact]
    public void TryParseCollection_invalid_entry_returns_false()
    {
        Assert.False(Point.TryParseCollection("1,2 bad 3,4", out var points));
        Assert.Empty(points);
    }

    [Fact]
    public void TryParseCollection_enumerable()
    {
        IEnumerable<string> values = new[] { "1,2", "3,4" };
        Assert.True(Point.TryParseCollection(values, out var points));
        Assert.Equal(2, points.Count);
    }

    [Fact]
    public void TryParseCollection_enumerable_null_throws()
    {
        Assert.Throws<ArgumentNullException>(() => Point.TryParseCollection((IEnumerable<string>)null!, out _));
    }

    // ------------------------------------------------------------------
    //  Equality (record struct)
    // ------------------------------------------------------------------

    [Fact]
    public void Same_points_are_equal()
    {
        var a = new Point(1.5, 2.5);
        var b = new Point(1.5, 2.5);
        Assert.Equal(a, b);
        Assert.True(a == b);
    }

    [Fact]
    public void Different_points_are_not_equal()
    {
        var a = new Point(1, 2);
        var b = new Point(2, 1);
        Assert.NotEqual(a, b);
    }
}
