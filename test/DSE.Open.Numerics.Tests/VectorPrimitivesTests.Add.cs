// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Numerics;

public partial class VectorPrimitivesTests
{
    [Fact]
    public void Add_Int32_Array_Int32()
    {
        var v1 = Series.Create([1, 2, 3, 4, 5]);
        var v2 = Series.Create([1, 2, 3, 4, 5]);
        var sum = new int[5];

        VectorPrimitives.Add(v1, v2, sum);

        Assert.Equal(2, sum[0]);
        Assert.Equal(4, sum[1]);
        Assert.Equal(6, sum[2]);
        Assert.Equal(8, sum[3]);
        Assert.Equal(10, sum[4]);
    }
}
