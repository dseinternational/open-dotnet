// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Numerics;
using TorchSharp;

namespace DSE.Open.Interop.Torch;

public static class NativeTorchTensor
{
    public static torch.Tensor Create(ReadOnlyTensor<short> tensor)
    {
        return Create(tensor.TensorSpan);
    }

    public static torch.Tensor Create(ReadOnlyTensorSpan<short> tensor)
    {
        return torch.tensor(
            tensor.Elements.ToArray(),
            tensor.Shape.ToArray().Select(i => (long)i).ToArray(),
            torch.ScalarType.Int16);
    }

    public static torch.Tensor Create(ReadOnlyTensor<int> tensor)
    {
        return Create(tensor.TensorSpan);
    }

    public static torch.Tensor Create(ReadOnlyTensorSpan<int> tensor)
    {
        return torch.tensor(
            tensor.Elements.ToArray(),
            tensor.Shape.ToArray().Select(i => (long)i).ToArray(),
            torch.ScalarType.Int32);
    }

    public static torch.Tensor Create(ReadOnlyTensor<float> tensor)
    {
        return Create(tensor.TensorSpan);
    }

    public static torch.Tensor Create(ReadOnlyTensorSpan<float> tensor)
    {
        return torch.tensor(
            tensor.Elements.ToArray(),
            tensor.Shape.ToArray().Select(i => (long)i).ToArray(),
            torch.ScalarType.Float32);
    }

    public static torch.Tensor Create(ReadOnlyTensor<double> tensor)
    {
        return Create(tensor.TensorSpan);
    }

    public static torch.Tensor Create(ReadOnlyTensorSpan<double> tensor)
    {
        return torch.tensor(
            tensor.Elements.ToArray(),
            tensor.Shape.ToArray().Select(i => (long)i).ToArray(),
            torch.ScalarType.Float64);
    }

    public static bool IsValidElementType(Type type)
    {
        return type == typeof(int)
            || type == typeof(short)
            || type == typeof(float)
            || type == typeof(double);
    }

}
