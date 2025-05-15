// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Runtime.CompilerServices;
using DSE.Open.Collections.Generic;

namespace DSE.Open.Numerics;

public static class ReadOnlyCategorySet
{
    public static ReadOnlyCategorySet<T> Create<T>(IReadOnlySet<T> set)
        where T : IEquatable<T>
    {
        return new ReadOnlyCategorySet<T>(set);
    }
    public static ReadOnlyCategorySet<T> Create<T>(ReadOnlySpan<T> span)
        where T : IEquatable<T>
    {
        return new ReadOnlyCategorySet<T>(span.ToArray());
    }
}

[CollectionBuilder(typeof(ReadOnlyCategorySet), nameof(ReadOnlyCategorySet.Create))]
public sealed class ReadOnlyCategorySet<T> : ReadOnlySet<T>, IReadOnlyCategorySet<T>
    where T : IEquatable<T>
{
    public ReadOnlyCategorySet()
    {
    }

    public ReadOnlyCategorySet(IEnumerable<T> set) : base(set)
    {
    }

    public ReadOnlyCategorySet(IReadOnlySet<T> set) : base(set)
    {
    }
}
