// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.EntityFrameworkCore.Models.Records.Storage.ValueConversion;
using DSE.Open.EntityFrameworkCore.Storage.ValueConversion;
using DSE.Open.Records;

namespace DSE.Open.EntityFrameworkCore.Models.Records.Tests.Storage.ValueConversion;

public class BiologicalSexToByteConverterTests
{
    [Theory]
    [InlineData(BiologicalSexToByteConverter.Female)]
    [InlineData(BiologicalSexToByteConverter.Male)]
    public void RoundTrip(byte value)
    {
        var converter = BiologicalSexToByteConverter.Default;
        var sex = (BiologicalSex)(converter.ConvertFromProvider(value) ?? throw new InvalidOperationException());
        var stored = (byte)(converter.ConvertToProvider(sex) ?? throw new InvalidOperationException());
        Assert.Equal(value, stored);
    }

    [Fact]
    public void UnsupportedByteThrows()
    {
        var converter = BiologicalSexToByteConverter.Default;
        _ = Assert.Throws<ValueConversionException>(() => converter.ConvertFromProvider((byte)42));
    }
}
