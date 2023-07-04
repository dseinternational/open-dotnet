// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Tests.Storage.ValueConversion;

public class TimeSpanToInt32HoursConverterTests
{
    [Theory]
    [InlineData(10)]
    [InlineData(100)]
    [InlineData(-100)]
    public void ConvertFrom(int hours)
    {
        var converter = new TimeSpanToInt32HoursConverter();
        var result = (TimeSpan)(converter.ConvertFromProvider(hours) ?? throw new InvalidOperationException());
        Assert.Equal(hours, result.TotalHours);
    }

    [Theory]
    [InlineData(10)]
    [InlineData(100)]
    [InlineData(-100)]
    public void ConvertTo(int hours)
    {
        var converter = new TimeSpanToInt32HoursConverter();
        var result = (int)(converter.ConvertToProvider(TimeSpan.FromHours(hours)) ?? throw new InvalidOperationException());
        Assert.Equal(hours, result);
    }
}
