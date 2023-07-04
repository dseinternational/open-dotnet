// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Tests.Storage.ValueConversion;

public class TimeSpanToDoubleMinutesConverterTests
{
    [Theory]
    [InlineData(102684.254589)]
    [InlineData(100.0003)]
    [InlineData(-9.186100)]
    public void ConvertFrom(double minutes)
    {
        var converter = new TimeSpanToDoubleMinutesConverter();
        var result = (TimeSpan)(converter.ConvertFromProvider(minutes) ?? throw new InvalidOperationException());
        Assert.Equal(minutes, result.TotalMinutes);
    }

    [Theory]
    [InlineData(102684.254589)]
    [InlineData(100.0003)]
    [InlineData(-9.186100)]
    public void ConvertTo(double minutes)
    {
        var converter = new TimeSpanToDoubleMinutesConverter();
        var result = (double)(converter.ConvertToProvider(TimeSpan.FromMinutes(minutes)) ?? throw new InvalidOperationException());
        Assert.Equal(minutes, result);
    }
}
