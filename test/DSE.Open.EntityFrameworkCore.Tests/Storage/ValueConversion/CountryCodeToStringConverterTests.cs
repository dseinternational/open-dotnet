// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.EntityFrameworkCore.Storage.ValueConversion;
using DSE.Open.Globalization;

namespace DSE.Open.EntityFrameworkCore.Tests.Storage.ValueConversion;

public class CountryCodeToStringConverterTests
{
    [Fact]
    public void ConvertFrom()
    {
        var converter = CountryCodeToStringConverter.Default;
        var result = (CountryCode)(converter.ConvertFromProvider("GB") ?? throw new InvalidOperationException());
        Assert.Equal(CountryCode.UnitedKingdom, result);
    }

    [Fact]
    public void ConvertTo()
    {
        var converter = CountryCodeToStringConverter.Default;
        var result = converter.ConvertToProvider(CountryCode.Norway)?.ToString();
        Assert.Equal("NO", result);
    }
}
