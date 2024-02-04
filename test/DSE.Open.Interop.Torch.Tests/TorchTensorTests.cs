// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Numerics;

namespace DSE.Open.Interop.Torch;

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

public class TorchTensorTests
{
    [Fact]
    public void Add_2D_Array()
    {
        var t1 = new Tensor<int>(new int[,]
        {
            { 1, 2 },
            { 3, 4 },
            { 5, 6 }
        });

        var t2 = new Tensor<int>(new int[,]
        {
            { 1, 2 },
            { 3, 4 },
            { 5, 6 }
        });

        using var tt1 = TorchTensor.Create(t1);
        using var tt2 = TorchTensor.Create(t2);

        using var tt3 = tt1.Add(tt2);

        var t3 = new Tensor<int>(tt3.ToArray(), t1.Shape);

        Assert.Equal(2u, t3.Rank);
        Assert.Equal(6u, t3.Length);
        Assert.Equal(2, t3[[0, 0]]);
        Assert.Equal(4, t3[[0, 1]]);
        Assert.Equal(6, t3[[1, 0]]);
        Assert.Equal(8, t3[[1, 1]]);
    }
}
