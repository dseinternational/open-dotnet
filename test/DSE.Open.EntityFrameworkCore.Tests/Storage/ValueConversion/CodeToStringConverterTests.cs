// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.EntityFrameworkCore.Storage.ValueConversion;
using DSE.Open.Values;

namespace DSE.Open.EntityFrameworkCore.Tests.Storage.ValueConversion;

public class CodeToStringConverterTests
{
    [Fact]
    public void ConvertFrom()
    {
        var converter = CodeToStringConverter.Default;
        var result = (Code)(converter.ConvertFromProvider("abc-123") ?? throw new InvalidOperationException());
        Assert.Equal(Code.Parse("abc-123", null), result);
    }

    [Fact]
    public void ConvertTo()
    {
        var converter = CodeToStringConverter.Default;
        var code = Code.Parse("xyz_42", null);
        var result = converter.ConvertToProvider(code)?.ToString();
        Assert.Equal("xyz_42", result);
    }

    [Fact]
    public void ConvertFromInvalidStringThrows()
    {
        var converter = CodeToStringConverter.Default;
        _ = Assert.Throws<ValueConversionException>(() => converter.ConvertFromProvider(new string('x', 1024)));
    }
}
