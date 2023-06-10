// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Drawing.Tests;

public class ColorTests
{
    [Theory]
    [InlineData(0, 0, 0, 0, "#00000000")]
    [InlineData(0, 255, 0, 0, "#FF000000")]
    [InlineData(0, 255, 255, 255, "#FFFFFF00")]
    [InlineData(254, 0, 0, 0, "#000000FE")]
    [InlineData(254, 255, 0, 0, "#FF0000FE")]
    [InlineData(254, 255, 255, 255, "#FFFFFFFE")]
    [InlineData(255, 0, 0, 0, "#000000")]
    [InlineData(255, 255, 0, 0, "#FF0000")]
    [InlineData(255, 255, 255, 255, "#FFFFFF")]
    public void ToStringOutputsHexFormat(byte a, byte r, byte g, byte b, string expected)
    {
        var c = Color.FromArgb(a, r, g, b);
        var s = c.ToString();
        Assert.Equal(expected, s);
    }
}
