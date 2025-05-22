// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Numerics;

public partial class VectorPrimitivesTests
{
    [Fact]
    public void Max_ReturnsMax_Int32()
    {
        var v = Vector.Create([2, 200, 4000, 30000]);
        var s = v.Max();
        Assert.Equal(30000, s);
    }
}
