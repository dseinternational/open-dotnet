// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Numerics;
using TorchSharp;

namespace DSE.Open.Interop.Torch;

internal sealed class TorchTensorSingle : TorchTensor<float>
{
    internal TorchTensorSingle(torch.Tensor tensor) : base(tensor)
    {
    }

    protected override TorchTensor<float> CreateNew(torch.Tensor tensor)
    {
        return new TorchTensorSingle(tensor);
    }

    protected override torch.Tensor CreateNewNative(TensorSpan<float> tensor)
    {
        return NativeTorchTensor.Create(tensor);
    }

    public override TorchTensor<float> Add(float scalar)
    {
        return new TorchTensorSingle(Tensor.add(scalar));
    }
}
