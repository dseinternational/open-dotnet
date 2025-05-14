// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Runtime.CompilerServices;
using DSE.Open.Hashing;

namespace DSE.Open.Numerics;

public abstract class DataLabelCollectionBase<TData>
    where TData : IEquatable<TData>
{

    protected static ulong GetHash(TData data)
    {
        if (data is IRepeatableHash64 hash64)
        {
            return hash64.GetRepeatableHashCode();
        }

        return HashU64_SplitMix(data.GetHashCode());
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static ulong HashU64_SplitMix(int value)
    {
        unchecked
        {
            var z = (uint)value + 0x9E3779B97F4A7C15UL;
            z = (z ^ (z >> 30)) * 0xBF58476D1CE4E5B9UL;
            z = (z ^ (z >> 27)) * 0x94D049BB133111EBUL;
            z ^= z >> 31;
            return z;
        }
    }
}
