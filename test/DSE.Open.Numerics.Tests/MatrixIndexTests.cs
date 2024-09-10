// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Memory;

namespace DSE.Open.Numerics;

public class MatrixIndexTests
{
    [Fact]
    public void Implicit_conversion_tuple()
    {
        var index = new MatrixIndex(1, 2);
        var tuple = (1, 2);
        var index2 = (MatrixIndex)tuple;
        var tuple2 = (ValueTuple<int, int>)index;

        Assert.Equal(index, index2);
        Assert.Equal(tuple, tuple2);
    }

    [Fact]
    public void Implicit_conversion_index2D()
    {
        var index = new MatrixIndex(1, 2);
        var index2D = new Index2D(1, 2);
        var index2 = (MatrixIndex)index2D;
        var index2D2 = (Index2D)index;

        Assert.Equal(index, index2);
        Assert.Equal(index2D, index2D2);
    }
}
