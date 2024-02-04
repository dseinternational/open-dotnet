// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;

namespace DSE.Open.Numerics;

public interface IVector<T, TSelf> : IReadOnlyVector<T, TSelf>
    where T : struct, INumber<T>
    where TSelf : IVector<T, TSelf>
{
    new T this[int index] { get; set; }

    new Span<T> Memory { get; }

#pragma warning disable CA1000 // Do not declare static members on generic types
    static abstract TSelf Create(Span<T> sequence);
#pragma warning restore CA1000 // Do not declare static members on generic types
}
