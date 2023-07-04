// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Tests.Storage.ValueConversion;

public class TimeSpanToInt64MinutesConverterTests
{
    [Theory]
    [InlineData(10)]
    [InlineData(100)]
    [InlineData(-100)]
    public void ConvertFrom(int minutes)
    {
        var convert = new TimeSpanToInt32MinutesConverter();
        var result = (TimeSpan)(convert.ConvertFromProvider(minutes) ?? throw new InvalidOperationException());
        Assert.Equal(minutes, result.TotalMinutes);
    }

    [Theory]
    [InlineData(10)]
    [InlineData(100)]
    [InlineData(-100)]
    public void ConvertTo(int minutes)
    {
        var convert = new TimeSpanToInt32MinutesConverter();
        var result = (int)(convert.ConvertToProvider(TimeSpan.FromMinutes(minutes)) ?? throw new InvalidOperationException());
        Assert.Equal(minutes, result);
    }
}
