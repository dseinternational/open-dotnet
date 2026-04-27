// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.EntityFrameworkCore.Storage.ValueConversion;
using DSE.Open.Values.Units;

namespace DSE.Open.EntityFrameworkCore.Tests.Storage.ValueConversion;

public class LengthToDoubleConverterTests
{
    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(123.456)]
    [InlineData(-50)]
    public void ConvertTo(double millimetres)
    {
        var converter = LengthToDoubleConverter.Default;
        var length = Length.FromMillimetres(millimetres);
        var result = (double)(converter.ConvertToProvider(length) ?? throw new InvalidOperationException());
        Assert.Equal(millimetres, result);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(123.456)]
    [InlineData(-50)]
    public void ConvertFrom(double millimetres)
    {
        var converter = LengthToDoubleConverter.Default;
        var result = (Length)(converter.ConvertFromProvider(millimetres) ?? throw new InvalidOperationException());
        Assert.Equal(millimetres, result.Millimetres);
    }
}
