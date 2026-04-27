// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.EntityFrameworkCore.Models.Records.Storage.ValueConversion;
using DSE.Open.EntityFrameworkCore.Storage.ValueConversion;
using DSE.Open.Records;

namespace DSE.Open.EntityFrameworkCore.Models.Records.Tests.Storage.ValueConversion;

public class GenderToStringConverterTests
{
    [Fact]
    public void ConvertToFemale()
    {
        var converter = GenderToStringConverter.Default;
        Assert.Equal("female", converter.ConvertToProvider(Gender.Female)?.ToString());
    }

    [Fact]
    public void ConvertFromMale()
    {
        var converter = GenderToStringConverter.Default;
        var gender = (Gender)(converter.ConvertFromProvider("male") ?? throw new InvalidOperationException());
        Assert.Equal(Gender.Male, gender);
    }

    [Fact]
    public void ConvertFromInvalidThrows()
    {
        var converter = GenderToStringConverter.Default;
        _ = Assert.Throws<ValueConversionException>(() => converter.ConvertFromProvider("not-a-gender"));
    }
}
