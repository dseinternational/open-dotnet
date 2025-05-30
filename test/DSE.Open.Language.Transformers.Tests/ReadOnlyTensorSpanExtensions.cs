// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics.Tensors;

namespace DSE.Open.Language.Transformers;

#pragma warning disable SYSLIB5001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.

internal static class ReadOnlyTensorSpanExtensions
{
    public static string GetDescription<T>(this in ReadOnlyTensorSpan<T> tensor)
    {
        return $"{typeof(ReadOnlyTensorSpan<>).Name}<{typeof(T).Name}> Rank: {tensor.Rank}, " +
            $"Lengths: [{string.Join(",", tensor.Lengths.ToArray())}], " +
            $"Strides: [{string.Join(",", tensor.Strides.ToArray())}]";
    }
}
