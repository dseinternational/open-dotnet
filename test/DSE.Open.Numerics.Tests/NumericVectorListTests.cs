// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Numerics;

public class NumericVectorListTests
{
    [Fact]
    public void ToTensor()
    {
        int[] x = [.. Enumerable.Range(0, 5)];
        int[] y = [.. Enumerable.Range(0, 5)];

        NumericVectorList<int> vector = [x, y];

        var tensor = vector.ToTensor();

        Assert.NotNull(tensor);
        Assert.Equal(2, tensor.Rank);
        Assert.Equal(2, tensor.Lengths[0]);
        Assert.Equal(5, tensor.Lengths[1]);
    }
}
