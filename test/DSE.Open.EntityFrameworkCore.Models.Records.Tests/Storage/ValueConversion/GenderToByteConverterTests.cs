// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.EntityFrameworkCore.Models.Records.Storage.ValueConversion;
using DSE.Open.EntityFrameworkCore.Storage.ValueConversion;
using DSE.Open.Records;

namespace DSE.Open.EntityFrameworkCore.Models.Records.Tests.Storage.ValueConversion;

public class GenderToByteConverterTests
{
    [Theory]
    [InlineData(GenderToByteConverter.Female)]
    [InlineData(GenderToByteConverter.Male)]
    [InlineData(GenderToByteConverter.Other)]
    public void RoundTripFromByte(byte value)
    {
        var converter = GenderToByteConverter.Default;
        var gender = (Gender)(converter.ConvertFromProvider(value) ?? throw new InvalidOperationException());
        var stored = (byte)(converter.ConvertToProvider(gender) ?? throw new InvalidOperationException());
        Assert.Equal(value, stored);
    }

    [Fact]
    public void UnknownByteMapsToOther()
    {
        var converter = GenderToByteConverter.Default;
        var gender = (Gender)(converter.ConvertFromProvider(GenderToByteConverter.Unknown) ?? throw new InvalidOperationException());
        Assert.Equal(Gender.Other, gender);
    }

    [Fact]
    public void UnsupportedByteThrows()
    {
        var converter = GenderToByteConverter.Default;
        _ = Assert.Throws<ValueConversionException>(() => converter.ConvertFromProvider((byte)42));
    }
}
