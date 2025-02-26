// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics.Tensors;
using TorchSharp;

namespace DSE.Open.Interop.Torch;

#pragma warning disable SYSLIB5001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.

public static class NativeTorchTensor
{
    public static torch.Tensor Create(ReadOnlyTensorSpan<short> tensor)
    {
        var elements = new short[tensor.FlattenedLength];
        tensor.FlattenTo(elements);
        var dimensions = tensor.Lengths.ToArray().Select(i => (long)i).ToArray();
        return torch.tensor(
            elements,
            dimensions,
            torch.ScalarType.Int16);
    }

    public static torch.Tensor Create(ReadOnlyTensorSpan<int> tensor)
    {
        var elements = new int[tensor.FlattenedLength];
        tensor.FlattenTo(elements);
        var dimensions = tensor.Lengths.ToArray().Select(i => (long)i).ToArray();
        return torch.tensor(
            elements,
            dimensions,
            torch.ScalarType.Int32);
    }

    public static torch.Tensor Create(ReadOnlyTensorSpan<float> tensor)
    {
        var elements = new float[tensor.FlattenedLength];
        tensor.FlattenTo(elements);
        var dimensions = tensor.Lengths.ToArray().Select(i => (long)i).ToArray();
        return torch.tensor(
            elements,
            dimensions,
            torch.ScalarType.Float32);
    }

    public static torch.Tensor Create(ReadOnlyTensorSpan<double> tensor)
    {
        var elements = new double[tensor.FlattenedLength];
        tensor.FlattenTo(elements);
        var dimensions = tensor.Lengths.ToArray().Select(i => (long)i).ToArray();
        return torch.tensor(
            elements,
            dimensions,
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
