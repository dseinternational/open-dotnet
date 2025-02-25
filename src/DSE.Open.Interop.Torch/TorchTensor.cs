// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics.Tensors;

namespace DSE.Open.Interop.Torch;

#pragma warning disable SYSLIB5001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.

public static class TorchTensor
{
    public static TorchTensor<int> Create(Tensor<int> tensor)
    {
        ArgumentNullException.ThrowIfNull(tensor);
        return Create(tensor.AsTensorSpan());
    }

    public static TorchTensor<int> Create(TensorSpan<int> tensor)
    {
        return new TorchTensorInt32(NativeTorchTensor.Create(tensor));
    }

    public static TorchTensor<float> Create(Tensor<float> tensor)
    {
        ArgumentNullException.ThrowIfNull(tensor);
        return Create(tensor.AsTensorSpan());
    }

    public static TorchTensor<float> Create(TensorSpan<float> tensor)
    {
        return new TorchTensorSingle(NativeTorchTensor.Create(tensor));
    }

    public static bool IsValidElementType(Type type)
    {
        return type == typeof(int)
            || type == typeof(short)
            || type == typeof(float)
            || type == typeof(double);
    }
}
