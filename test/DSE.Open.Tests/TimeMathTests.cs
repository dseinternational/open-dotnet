// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open;

public class TimeMathTests
{
    [Fact]
    public void Max_DateTime_ReturnsLater()
    {
        var earlier = new DateTime(2020, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        var later = new DateTime(2024, 6, 15, 0, 0, 0, DateTimeKind.Utc);

        Assert.Equal(later, TimeMath.Max(earlier, later));
        Assert.Equal(later, TimeMath.Max(later, earlier));
    }

    [Fact]
    public void Max_DateTime_EqualValues_ReturnsFirstArg()
    {
        var a = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        var b = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        Assert.Equal(a, TimeMath.Max(a, b));
    }

    [Fact]
    public void Min_DateTime_ReturnsEarlier()
    {
        var earlier = new DateTime(2020, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        var later = new DateTime(2024, 6, 15, 0, 0, 0, DateTimeKind.Utc);

        Assert.Equal(earlier, TimeMath.Min(earlier, later));
        Assert.Equal(earlier, TimeMath.Min(later, earlier));
    }

    [Fact]
    public void Max_DateTimeOffset_ReturnsLater()
    {
        var earlier = new DateTimeOffset(2020, 1, 1, 0, 0, 0, TimeSpan.Zero);
        var later = new DateTimeOffset(2024, 6, 15, 0, 0, 0, TimeSpan.Zero);

        Assert.Equal(later, TimeMath.Max(earlier, later));
    }

    [Fact]
    public void Min_DateTimeOffset_ReturnsEarlier()
    {
        var earlier = new DateTimeOffset(2020, 1, 1, 0, 0, 0, TimeSpan.Zero);
        var later = new DateTimeOffset(2024, 6, 15, 0, 0, 0, TimeSpan.Zero);

        Assert.Equal(earlier, TimeMath.Min(earlier, later));
    }

    [Fact]
    public void Max_TimeSpan_ReturnsLonger()
    {
        var shorter = TimeSpan.FromMinutes(5);
        var longer = TimeSpan.FromHours(1);

        Assert.Equal(longer, TimeMath.Max(shorter, longer));
    }

    [Fact]
    public void Min_TimeSpan_ReturnsShorter()
    {
        var shorter = TimeSpan.FromMinutes(5);
        var longer = TimeSpan.FromHours(1);

        Assert.Equal(shorter, TimeMath.Min(shorter, longer));
    }

    [Fact]
    public void TimeSpan_HandlesNegativeDurations()
    {
        var neg = TimeSpan.FromSeconds(-10);
        var pos = TimeSpan.FromSeconds(10);

        Assert.Equal(pos, TimeMath.Max(neg, pos));
        Assert.Equal(neg, TimeMath.Min(neg, pos));
    }
}
