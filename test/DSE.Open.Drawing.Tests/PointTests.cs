// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Drawing.Tests;

public class PointTests
{
    [Theory]
    [InlineData("1,1", 1, 1)]
    [InlineData("1.1,100.0001", 1.1, 100.0001)]
    public void Parse(string value, double expectedX, double expectedY)
    {
        Assert.True(Point.TryParse(value, out var point));
        Assert.Equal(expectedX, point.X);
        Assert.Equal(expectedY, point.Y);
    }
}
