// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Numerics;

public partial class VectorTests
{
    [Fact]
    public void Max_Array_Int32()
    {
        var sequence = Enumerable.Range(1, 500).Union(Enumerable.Range(0, 100)).ToArray();
        var max = Vector.Maximum(sequence);
        Assert.Equal(500, max);
    }

    [Fact]
    public void Max_Enumerable_Int32()
    {
        var sequence = Enumerable.Range(1, 500).Union(Enumerable.Range(0, 100));
        var max = Vector.Maximum(sequence);
        Assert.Equal(500, max);
    }

    [Fact]
    public void Max_Array_Double()
    {
        var sequence = Enumerable.Range(1, 500).Union(Enumerable.Range(0, 100))
            .Select(i => i / 3.33)
            .ToArray();
        var max = Vector.MaximumFloatingPoint(sequence);
        Assert.Equal(150.15015015015015, max);
    }

    [Fact]
    public void Max_Array_Decimal()
    {
        var sequence = Enumerable.Range(1, 500).Union(Enumerable.Range(0, 100))
            .Select(i => i / 3.33m)
            .ToArray();
        var max = Vector.Maximum(sequence);
        Assert.Equal(150.15015015015015015015015015m, max);
    }
}
