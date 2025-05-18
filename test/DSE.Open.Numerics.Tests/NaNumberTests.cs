// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics.Tensors;

namespace DSE.Open.Numerics;

public class NaNumberTests
{
    [Fact]
    public void Sum()
    {
        NaNumber<int>[] sequence = [1, 2, 3, 4, 5];
        var sum = TensorPrimitives.Sum(sequence);
        Assert.Equal(15, sum);
    }

    [Fact]
    public void Sum_Na()
    {
        NaNumber<int>[] sequence = [1, 2, 3, NaNumber<int>.Na, 4, 5, NaNumber<int>.Na, 6, 7, 8];
        var sum = TensorPrimitives.Sum(sequence);
        Assert.Equal("NA", sum.ToString());
    }
}
