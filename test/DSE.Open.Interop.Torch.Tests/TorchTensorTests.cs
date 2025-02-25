// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics.Tensors;

namespace DSE.Open.Interop.Torch;

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional
#pragma warning disable SYSLIB5001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.

public class TorchTensorTests
{
    [Fact(Skip = "TODO")]
    public void Add_2D_Array()
    {
        var t1 = Tensor.Create([1, 2, 3, 4, 5, 6], [2, 3]);

        var t2 = Tensor.Create([1, 2, 3, 4, 5, 6], [2, 3]);

        using var tt1 = TorchTensor.Create(t1);
        using var tt2 = TorchTensor.Create(t2);

        using var tt3 = tt1.Add(tt2);

        var t3 = Tensor.Create(tt3.ToArray(), t1.Lengths, t1.Strides);

        Assert.Equal(2, t3.Rank);
        Assert.Equal(6, t3.FlattenedLength);
        Assert.Equal(2, t3[[0, 0]]);
        Assert.Equal(4, t3[[0, 1]]);
        Assert.Equal(6, t3[[1, 0]]);
        Assert.Equal(8, t3[[1, 1]]);
    }
}
