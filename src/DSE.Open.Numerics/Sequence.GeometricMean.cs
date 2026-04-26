// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.ComponentModel;
using System.Numerics;

namespace DSE.Open.Numerics;

/// <summary>
/// LINQ-friendly numeric reductions over <see cref="IEnumerable{T}"/> sequences.
/// The corresponding span-based primitives live on <see cref="VectorPrimitives"/>.
/// </summary>
public static partial class Sequence
{
    /// <summary>Reserved — geometric mean is not yet implemented.</summary>
    [Obsolete("Not yet implemented", error: true)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static T GeometricMean<T>(IEnumerable<T> sequence)
        where T : struct, INumberBase<T>
    {
        throw new NotImplementedException();
    }

    /// <summary>Reserved — geometric mean is not yet implemented.</summary>
    [Obsolete("Not yet implemented", error: true)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static TResult GeometricMean<T, TResult>(IEnumerable<T> sequence)
        where T : struct, INumberBase<T>
        where TResult : struct, INumberBase<TResult>
    {
        // TODO: consider accumulating method to avoid TResult overflow

        throw new NotImplementedException();
    }
}
