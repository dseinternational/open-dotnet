// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.EntityFrameworkCore.Storage.ValueConversion;
using DSE.Open.Globalization;

namespace DSE.Open.EntityFrameworkCore.Tests.Storage.ValueConversion;

public class LanguageTagToStringConverterTests
{
    [Fact]
    public void ConvertFrom()
    {
        var converter = LanguageTagToStringConverter.Default;
        var result = (LanguageTag)(converter.ConvertFromProvider("en-GB") ?? throw new InvalidOperationException());
        Assert.Equal(LanguageTag.EnglishUk, result);
    }

    [Fact]
    public void ConvertTo()
    {
        var converter = LanguageTagToStringConverter.Default;
        var result = converter.ConvertToProvider(LanguageTag.EnglishUs)?.ToString();
        Assert.Equal("en-US", result);
    }
}
