// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Tests.Storage.ValueConversion;

public class TimeSpanToInt16MinutesConverterTests
{
    [Theory]
    [InlineData(10)]
    [InlineData(100)]
    [InlineData(-100)]
    public void ConvertFrom(short minutes)
    {
        var converter = new TimeSpanToInt16MinutesConverter();
        var result = (TimeSpan)(converter.ConvertFromProvider(minutes) ?? throw new InvalidOperationException());
        Assert.Equal(minutes, result.TotalMinutes);
    }

    [Theory]
    [InlineData(10)]
    [InlineData(100)]
    [InlineData(-100)]
    public void ConvertTo(short minutes)
    {
        var converter = new TimeSpanToInt16MinutesConverter();
        var result = (short)(converter.ConvertToProvider(TimeSpan.FromMinutes(minutes)) ?? throw new InvalidOperationException());
        Assert.Equal(minutes, result);
    }
}
