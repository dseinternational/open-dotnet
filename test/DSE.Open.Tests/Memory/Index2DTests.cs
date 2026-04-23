// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Memory;

public class Index2DTests
{
    [Fact]
    public void ConstructorAssignsHeightAndWidth()
    {
        var index = new Index2D(3, 5);

        Assert.Equal(3, index.Height);
        Assert.Equal(5, index.Width);
    }

    [Fact]
    public void DeconstructReturnsHeightAndWidth()
    {
        var index = new Index2D(7, 11);

        var (height, width) = index;

        Assert.Equal(7, height);
        Assert.Equal(11, width);
    }

    [Fact]
    public void ImplicitConversionFromValueTuplePreservesBothValues()
    {
        Index2D index = (4, 9);

        Assert.Equal(4, index.Height);
        Assert.Equal(9, index.Width);
    }

    [Fact]
    public void ImplicitConversionToValueTuplePreservesBothValues()
    {
        var index = new Index2D(6, 13);

        (int h, int w) tuple = index;

        Assert.Equal(6, tuple.h);
        Assert.Equal(13, tuple.w);
    }
}
