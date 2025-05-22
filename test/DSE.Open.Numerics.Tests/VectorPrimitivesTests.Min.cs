// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Numerics;

public partial class VectorPrimitivesTests
{
    [Fact]
    public void Min_ReturnsMin_Int32()
    {
        var v = Vector.Create([2, 200, 4000, 30000]);
        var s = v.Min();
        Assert.Equal(2, s);
    }
}
