// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.EntityFrameworkCore.Storage.ValueConversion;
using DSE.Open.Values.Units;

namespace DSE.Open.EntityFrameworkCore.Tests.Storage.ValueConversion;

public class MassToDoubleConverterTests
{
    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(456.789)]
    [InlineData(-25)]
    public void ConvertTo(double grams)
    {
        var converter = MassToDoubleConverter.Default;
        var mass = Mass.FromGrams(grams);
        var result = (double)(converter.ConvertToProvider(mass) ?? throw new InvalidOperationException());
        Assert.Equal(grams, result);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(456.789)]
    [InlineData(-25)]
    public void ConvertFrom(double grams)
    {
        var converter = MassToDoubleConverter.Default;
        var result = (Mass)(converter.ConvertFromProvider(grams) ?? throw new InvalidOperationException());
        Assert.Equal(grams, result.Grams);
    }
}
