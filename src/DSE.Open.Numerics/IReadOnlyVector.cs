// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;

namespace DSE.Open.Numerics;

public interface IReadOnlyVector
{
    int Length { get; }
}

public interface IReadOnlyVector<T, TSelf> : IReadOnlyVector
    where T : struct, INumber<T>
    where TSelf : IReadOnlyVector<T, TSelf>
{
    T this[int index] { get; }

    ReadOnlySpan<T> Memory { get; }

#pragma warning disable CA1000 // Do not declare static members on generic types
    static abstract TSelf Create(ReadOnlySpan<T> sequence);
#pragma warning restore CA1000 // Do not declare static members on generic types

    TSelf Add(TSelf other)
    {
        NumericsException.ThrowIfNotEqualLength(this, other);

        var destination = new T[Length];

        VectorPrimitives.Add(Memory, other.Memory, destination);

        return TSelf.Create(destination);
    }
}
