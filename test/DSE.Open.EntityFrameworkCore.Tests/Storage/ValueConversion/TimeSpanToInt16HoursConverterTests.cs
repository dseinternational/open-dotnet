// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Tests.Storage.ValueConversion;

public class TimeSpanToInt16HoursConverterTests
{
    [Theory]
    [InlineData(10)]
    [InlineData(100)]
    [InlineData(-100)]
    public void ConvertFrom(short hours)
    {
        var converter = new TimeSpanToInt16HoursConverter();
        var result = (TimeSpan)(converter.ConvertFromProvider(hours) ?? throw new InvalidOperationException());
        Assert.Equal(hours, result.TotalHours);
    }

    [Theory]
    [InlineData(10)]
    [InlineData(100)]
    [InlineData(-100)]
    public void ConvertTo(short hours)
    {
        var converter = new TimeSpanToInt16HoursConverter();
        var result = (short)(converter.ConvertToProvider(TimeSpan.FromHours(hours)) ?? throw new InvalidOperationException());
        Assert.Equal(hours, result);
    }
}
