// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Tests.Storage.ValueConversion;

public class TimeSpanToDoubleHoursConverterTests
{
    [Theory]
    [InlineData(102684.254589)]
    [InlineData(100.0003)]
    [InlineData(-9.186100)]
    public void ConvertFrom(double hours)
    {
        var converter = new TimeSpanToDoubleHoursConverter();
        var result = (TimeSpan)(converter.ConvertFromProvider(hours) ?? throw new InvalidOperationException());
        Assert.Equal(hours, result.TotalHours);
    }

    [Theory]
    [InlineData(102684.254589)]
    [InlineData(100.0003)]
    [InlineData(-9.186100)]
    public void ConvertTo(double hours)
    {
        var converter = new TimeSpanToDoubleHoursConverter();
        var result = (double)(converter.ConvertToProvider(TimeSpan.FromHours(hours)) ?? throw new InvalidOperationException());
        Assert.Equal(hours, result);
    }
}
