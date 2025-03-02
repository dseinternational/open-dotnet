// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Numerics;

public class DataPointCollectionExtensionsTests
{
    [Fact]
    public void ToTensorSpan()
    {
        var x = new[] { 1, 2, 3 };
        var y = new[] { 4, 5, 6 };

        var points = DataPoint.CreateRange(x, y).ToArray();

        Assert.Equal(3, points.Length);

        var ts = points.ToTensorSpan();

        Assert.Equal(3, ts.Lengths[0]);
        Assert.Equal(2, ts.Lengths[1]);
    }

    [Fact]
    public void ToTensor()
    {
        var x = new[] { 1, 2, 3 };
        var y = new[] { 4, 5, 6 };

        var points = DataPoint.CreateRange(x, y).ToArray();

        Assert.Equal(3, points.Length);

        var tensor = points.ToTensor();

        Assert.Equal(3, tensor.Lengths[0]);
        Assert.Equal(2, tensor.Lengths[1]);
    }
}
