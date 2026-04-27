// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.EntityFrameworkCore.Storage.ValueConversion;
using DSE.Open.Globalization;

namespace DSE.Open.EntityFrameworkCore.Tests.Storage.ValueConversion;

public class TelephoneNumberToStringConverterTests
{
    [Fact]
    public void ConvertTo()
    {
        var converter = TelephoneNumberToStringConverter.Default;
        var number = TelephoneNumber.Parse("+44 1234 567890", CultureInfo.InvariantCulture);
        var result = converter.ConvertToProvider(number)?.ToString();
        Assert.Equal(number.ToString(), result);
    }

    [Fact]
    public void ConvertFrom()
    {
        var converter = TelephoneNumberToStringConverter.Default;
        var expected = TelephoneNumber.Parse("+44 1234 567890", CultureInfo.InvariantCulture);
        var result = (TelephoneNumber)(converter.ConvertFromProvider(expected.ToString())
            ?? throw new InvalidOperationException());
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ConvertFromInvalidThrows()
    {
        var converter = TelephoneNumberToStringConverter.Default;
        _ = Assert.Throws<ValueConversionException>(() => converter.ConvertFromProvider("not-a-phone-number"));
    }
}
