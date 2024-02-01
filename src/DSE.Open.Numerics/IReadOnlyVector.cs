// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;
using System.Numerics.Tensors;

namespace DSE.Open.Numerics;

public interface IReadOnlyVector<T, TSelf>
    where T : struct, INumber<T>
    where TSelf : IReadOnlyVector<T, TSelf>
{
    int Length { get; }

    T this[int index] { get; }

    ReadOnlySpan<T> Sequence { get; }

#pragma warning disable CA1000 // Do not declare static members on generic types
    static abstract TSelf Create(ReadOnlySpan<T> sequence);
#pragma warning restore CA1000 // Do not declare static members on generic types

    TSelf Add(TSelf other)
    {
        if (Length != other.Length)
        {
            NumericsException.Throw("Vectors must be the same length.");
        }

        var destination = new T[Length];

        TensorPrimitives.Add(Sequence, other.Sequence, destination);

        return TSelf.Create(destination);
    }
}
