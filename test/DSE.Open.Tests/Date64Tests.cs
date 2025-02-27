// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;

namespace DSE.Open;

public class Date64Tests
{
    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(1619827200000)]
    [InlineData(-1)]
    [InlineData(-62135596800000)]
    [InlineData(253402300799999)]
    public void Create_Milliseconds(long milliseconds)
    {
        var date = new DateTime64(milliseconds);
        Assert.Equal(milliseconds, date.TotalMilliseconds);
        Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(milliseconds), date);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(1619827200000)]
    [InlineData(-1)]
    [InlineData(-62135596800000)]
    [InlineData(253402300799999)]
    public void Create_DateTimeOffset(long milliseconds)
    {
        var dateTimeOffset = DateTimeOffset.FromUnixTimeMilliseconds(milliseconds);
        var date = new DateTime64(milliseconds);
        Assert.Equal(milliseconds, date.TotalMilliseconds);
        Assert.Equal(dateTimeOffset, date);
    }

    [Fact]
    public void Serialize()
    {
        var date = new DateTime64(1619827200000);
        var json = JsonSerializer.Serialize(date);
        Assert.Equal("1619827200000", json);
    }

    [Fact]
    public void Deserialize()
    {
        var date = JsonSerializer.Deserialize<DateTime64>("1619827200000");
        Assert.Equal(1619827200000, date.TotalMilliseconds);
    }

}
