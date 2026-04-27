// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.EntityFrameworkCore.Models.Records.Storage.ValueConversion;
using DSE.Open.Records;

namespace DSE.Open.EntityFrameworkCore.Models.Records.Tests.Storage.ValueConversion;

public class GenderToInt32ConverterTests
{
    [Theory]
    [InlineData(GenderToByteConverter.Female)]
    [InlineData(GenderToByteConverter.Male)]
    [InlineData(GenderToByteConverter.Other)]
    public void RoundTrip(int value)
    {
        var converter = GenderToInt32Converter.Default;
        var gender = (Gender)(converter.ConvertFromProvider(value) ?? throw new InvalidOperationException());
        var stored = (int)(converter.ConvertToProvider(gender) ?? throw new InvalidOperationException());
        Assert.Equal(value, stored);
    }
}
