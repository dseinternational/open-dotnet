// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Numerics;
using TorchSharp;

namespace DSE.Open.Interop.Torch;

internal sealed class TorchTensorInt32 : TorchTensor<int>
{
    internal TorchTensorInt32(torch.Tensor tensor) : base(tensor)
    {
    }

    protected override TorchTensor<int> CreateNew(torch.Tensor tensor)
    {
        return new TorchTensorInt32(tensor);
    }

    protected override torch.Tensor CreateNewNative(TensorSpan<int> tensor)
    {
        return NativeTorchTensor.Create(tensor);
    }

    public override TorchTensor<int> Add(int scalar)
    {
        return new TorchTensorInt32(Tensor.add(scalar));
    }
}
