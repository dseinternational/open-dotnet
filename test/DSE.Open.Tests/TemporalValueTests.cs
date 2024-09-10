// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;
using Microsoft.Extensions.Time.Testing;

namespace DSE.Open.Tests;

public class TemporalValueTests
{
    [Fact]
    public void Init()
    {
        var now = DateTimeOffset.Now;
        var t = new TemporalValue<int>(now, 42);
        Assert.Equal(42, t.Value);
        Assert.Equal(now, t.Time);
    }

    [Fact]
    public void Init2()
    {
        var now = DateTimeOffset.Now;
        var t = new TemporalValue<int> { Time = now, Value = 42 };
        Assert.Equal(42, t.Value);
        Assert.Equal(now, t.Time);
    }

    [Fact]
    public void CanSerializeAndDeserialize()
    {
        var t = TemporalValue.ForLocalNow(42);
        var json = JsonSerializer.Serialize(t);
        var t2 = JsonSerializer.Deserialize<TemporalValue<int>>(json);
        Assert.Equal(t.Value, t2.Value);
        Assert.Equal(t.Time.ToUnixTimeMilliseconds(), t2.Time.ToUnixTimeMilliseconds());
    }

    [Fact]
    public void ForUtcNow()
    {
        var now = new DateTimeOffset(2024, 7, 20, 10, 02, 05, TimeSpan.Zero);
        var tp = new FakeTimeProvider();
        tp.SetUtcNow(now);
        var t = TemporalValue.ForUtcNow(42, tp);
        Assert.Equal(42, t.Value);
        Assert.Equal(now, t.Time);
    }

    [Fact]
    public void ForLocalNow()
    {
        var now = new DateTimeOffset(2024, 7, 20, 10, 02, 05, TimeSpan.Zero);
        var tp = new FakeTimeProvider();
        tp.SetUtcNow(now);
        tp.SetLocalTimeZone(TimeZoneInfo.Utc);
        var t = TemporalValue.ForLocalNow(42, tp);
        Assert.Equal(42, t.Value);
        Assert.Equal(now, t.Time);
    }
}
