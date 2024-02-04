// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Numerics;
using TorchSharp;

namespace DSE.Open.Interop.Torch;

internal sealed class TorchTensorDouble : TorchTensor<double>
{
    internal TorchTensorDouble(torch.Tensor tensor) : base(tensor)
    {
    }

    protected override TorchTensor<double> CreateNew(torch.Tensor tensor)
    {
        return new TorchTensorDouble(tensor);
    }

    protected override torch.Tensor CreateNewNative(TensorSpan<double> tensor)
    {
        return NativeTorchTensor.Create(tensor);
    }

    public override TorchTensor<double> Add(double scalar)
    {
        return new TorchTensorDouble(Tensor.add(scalar));
    }
}
