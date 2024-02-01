// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;

namespace DSE.Open.Numerics;

public static partial class Vector
{
    public static Vector<T> Create<T>(Memory<T> sequence)
        where T : struct, INumber<T>
    {
        return new Vector<T>(sequence);
    }

    public static Vector<T> Create<T>(ReadOnlySpan<T> sequence)
        where T : struct, INumber<T>
    {
        return new Vector<T>(sequence.ToArray());
    }

    public static Vector<T> Create<T>(int length)
        where T : struct, INumber<T>
    {
        return new Vector<T>(new T[length]);
    }

    public static TVector Add<T, TVector>(TVector left, TVector right)
        where T : struct, INumber<T>
        where TVector : IReadOnlyVector<T, TVector>
    {
        var destination = new T[left.Length];

        Sequence.Add(left.Sequence, right.Sequence, destination);

        return TVector.Create(destination);
    }

    public static void AddInPace<T, TVector>(TVector left, TVector right)
        where T : struct, INumber<T>
        where TVector : IVector<T, TVector>
    {
        Sequence.AddInPlace(left.Sequence, right.Sequence);
    }

}
