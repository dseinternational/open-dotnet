// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Tests.Storage.ValueConversion;

public class StringToUtf8BinaryValueConverterTests
{
    private const string StringValue = "Les signes inférieur et supérieur qui entourent le nom de rubrique indiquent " +
        "qu'il s'agit d'une rubrique de fusion et que les données (dans le cas présent, le prénom de l'enregistrement) " +
        "vont apparaître sur les étiquettes en lieu et place de «Prénom».";

    private static readonly byte[] ByteValue = ("Les signes inférieur et supérieur qui entourent le nom de rubrique indiquent "u8 +
        "qu'il s'agit d'une rubrique de fusion et que les données (dans le cas présent, le prénom de l'enregistrement) "u8 +
        "vont apparaître sur les étiquettes en lieu et place de «Prénom»."u8).ToArray();

    [Fact]
    public void ConvertsToByteArray()
    {
        var converter = new StringToUtf8BinaryValueConverter();
        var converted = (byte[]?)converter.ConvertToProvider.Invoke(StringValue);
        Assert.NotNull(converted);
        Assert.True(converted.AsSpan().SequenceEqual(ByteValue));
    }

    [Fact]
    public void ConvertsFromByteArray()
    {
        var converter = new StringToUtf8BinaryValueConverter();
        var converted = (string?)converter.ConvertFromProvider.Invoke(ByteValue.ToArray());
        Assert.NotNull(converted);
        Assert.Equal(StringValue, converted);
    }
}
