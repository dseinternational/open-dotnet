// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;
using DSE.Open.Text.Json;

namespace DSE.Open.Tests;

public class RangeTests
{
    [Fact]
    public void Deconstruct()
    {
        var r = new Range<int>(2, 20);
        var (start, end) = r;
        Assert.Equal(2, start);
        Assert.Equal(20, end);
    }

    [Fact]
    public void Construct()
    {
        Range<int> r = new (2, 20);
        Assert.Equal(2, r.Start);
        Assert.Equal(20, r.End);
    }

    [Fact]
    public void Initialize_sets_start_end_int32()
    {
        var r = new Range<int>(2, 20);
        Assert.Equal(2, r.Start);
        Assert.Equal(20, r.End);
    }

    [Fact]
    public void Initialize_sets_start_end_int64()
    {
        var r = new Range<long>(-586L, 46984635120);
        Assert.Equal(-586L, r.Start);
        Assert.Equal(46984635120, r.End);
    }

    [Fact]
    public void Includes_int32()
    {
        var r = new Range<int>(501, 9468);
        Assert.True(r.Includes(501));
        Assert.True(r.Includes(9468));
        Assert.True(r.Includes(502));
        Assert.True(r.Includes(9000));
        Assert.False(r.Includes(0));
        Assert.False(r.Includes(500));
        Assert.False(r.Includes(9469));
    }

    [Fact]
    public void Includes_int64()
    {
        var r = new Range<long>(-6486983541325, 46984635120);
        Assert.True(r.Includes(-6486983541325));
        Assert.True(r.Includes(46984635120));
        Assert.True(r.Includes(0));
        Assert.False(r.Includes(-6486983541326));
        Assert.False(r.Includes(46984635121));
    }

    [Fact]
    public void Serialize_deserialize()
    {
        var r = new Range<long>(-6486983541325, 46984635120);
        var json = JsonSerializer.Serialize(r);
        var r2 = JsonSerializer.Deserialize<Range<long>>(json);
        Assert.Equal(r, r2);
    }

    [Fact]
    public void Serialize_deserialize_with_converter()
    {
        var r = new Range<long>(-6486983541325, 46984635120);
        var json = JsonSerializer.Serialize(r, JsonSharedOptions.UnicodeRangesAll);
        Assert.Equal("\"-6486983541325:46984635120\"", json);
        var r2 = JsonSerializer.Deserialize<Range<long>>(json, JsonSharedOptions.UnicodeRangesAll);
        Assert.Equal(r, r2);
    }

    [Theory]
    [InlineData(1, 2, "1:2")]
    [InlineData(-100, 200, "-100:200")]
    [InlineData(int.MinValue, int.MaxValue, "-2147483648:2147483647")]
    public void ToStringOutputInt32(int start, int end, string expected)
    {
        var range = new Range<int>(start, end);
        var output = range.ToString(null, CultureInfo.InvariantCulture);
        Assert.Equal(expected, output);
    }

    [Theory]
    [InlineData(1, 2, "1:2")]
    [InlineData(-100, 200, "-100:200")]
    [InlineData(long.MinValue, long.MaxValue, "-9223372036854775808:9223372036854775807")]
    public void ToStringOutputInt64(long start, long end, string expected)
    {
        var range = new Range<long>(start, end);
        var output = range.ToString(null, CultureInfo.InvariantCulture);
        Assert.Equal(expected, output);
    }

    [Theory]
    [InlineData("", 0, 0)]
    [InlineData("1:1", 1, 1)]
    [InlineData("-1:1", -1, 1)]
    [InlineData("489512:6489512", 489512, 6489512)]
    public void TryParseInt32(string value, int start, int end)
    {
        Assert.True(Range<long>.TryParse(value, CultureInfo.InvariantCulture, out var range));
        Assert.Equal(start, range.Start);
        Assert.Equal(end, range.End);
    }

    [Theory]
    [InlineData("", 0, 0)]
    [InlineData("1:1", 1, 1)]
    [InlineData("-1:1", -1, 1)]
    [InlineData("106489512:2306489512", 106489512, 2306489512)]
    public void TryParseInt64(string value, long start, long end)
    {
        Assert.True(Range<long>.TryParse(value, CultureInfo.InvariantCulture, out var range));
        Assert.Equal(start, range.Start);
        Assert.Equal(end, range.End);
    }
}
