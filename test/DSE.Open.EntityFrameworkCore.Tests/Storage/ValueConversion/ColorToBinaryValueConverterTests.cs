// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Drawing;
using DSE.Open.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Tests.Storage.ValueConversion;

public class ColorToBinaryValueConverterTests
{
    [Theory]
    [MemberData(nameof(ColorValues))]
    public void ConvertsTo(Color c)
    {
        var converter = new ColorToBinaryValueConverter();
        var converted = (byte[]?)converter.ConvertToProvider.Invoke(c);
        Assert.NotNull(converted);
        Assert.True(converted.AsSpan().SequenceEqual(c.AsRgbaSpan()));
    }

    [Theory]
    [MemberData(nameof(ColorValues))]
    public void ConvertsFrom(Color c)
    {
        var converter = new ColorToBinaryValueConverter();
        var converted = (Color?)converter.ConvertFromProvider.Invoke(c.AsRrgbaBytes());
        Assert.NotNull(converted);
        Assert.Equal(c, converted);
    }

    public static TheoryData<Color> ColorValues
    {
        get
        {
            var data = new TheoryData<Color>
            {
                Colors.Aqua,
                Colors.Black,
                Colors.Cyan,
                Colors.White
            };
            return data;
        }
    }
}
