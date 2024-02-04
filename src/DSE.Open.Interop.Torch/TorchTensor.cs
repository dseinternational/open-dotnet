// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Numerics;

namespace DSE.Open.Interop.Torch;

public static class TorchTensor
{
    public static TorchTensor<int> Create(Tensor<int> tensor)
    {
        return Create(tensor.TensorSpan);
    }

    public static TorchTensor<int> Create(TensorSpan<int> tensor)
    {
        return new TorchTensorInt32(NativeTorchTensor.Create(tensor));
    }

    public static TorchTensor<float> Create(Tensor<float> tensor)
    {
        return Create(tensor.TensorSpan);
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
