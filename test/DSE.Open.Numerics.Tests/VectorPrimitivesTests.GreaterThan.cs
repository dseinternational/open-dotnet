// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Numerics;

public partial class VectorPrimitivesTests
{
    [Fact]
    public void GreaterThan_ReturnsTrue_Int32()
    {
        var v1 = Vector.Create([2, 200, 4000, 30000]);
        var v2 = Vector.Create([1, 100, 2000, 10000]);
        var result = v1.GreaterThan(v2);
        Assert.Equal([true, true, true, true], [.. result]);
    }

    [Fact]
    public void GreaterThan_ReturnsFalse_Int32()
    {
        var v1 = Vector.Create([2, 200, 4000, 30000]);
        var v2 = Vector.Create([3, 300, 5000, 40000]);
        var result = v1.GreaterThan(v2);
        Assert.Equal([false, false, false, false], [.. result]);
    }
}
