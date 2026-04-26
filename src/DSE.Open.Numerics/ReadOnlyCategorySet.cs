// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Runtime.CompilerServices;
using DSE.Open.Collections.Generic;

namespace DSE.Open.Numerics;

/// <summary>Static factories for <see cref="ReadOnlyCategorySet{T}"/>.</summary>
public static class ReadOnlyCategorySet
{
    /// <summary>
    /// Creates a read-only category set wrapping <paramref name="set"/>. Returns
    /// <paramref name="set"/> itself when it is already a <see cref="ReadOnlyCategorySet{T}"/>.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="set"/> is <see langword="null"/>.</exception>
    public static ReadOnlyCategorySet<T> Create<T>(IReadOnlySet<T> set)
        where T : IEquatable<T>
    {
        ArgumentNullException.ThrowIfNull(set);

        if (set is ReadOnlyCategorySet<T> readOnlyCategorySet)
        {
            return readOnlyCategorySet;
        }

        if (set.Count == 0)
        {
            return ReadOnlyCategorySet<T>.Empty;
        }

        return new ReadOnlyCategorySet<T>(set);
    }

    /// <summary>Collection-initializer-friendly factory; copies <paramref name="span"/> into a fresh array.</summary>
    public static ReadOnlyCategorySet<T> Create<T>(ReadOnlySpan<T> span)
        where T : IEquatable<T>
    {
        if (span.Length == 0)
        {
            return ReadOnlyCategorySet<T>.Empty;
        }

        return new ReadOnlyCategorySet<T>(span.ToArray());
    }
}

/// <summary>
/// A read-only set of allowed values for a categorical
/// <see cref="ReadOnlySeries{T}"/>. The underlying storage is shared with the
/// supplied <see cref="IReadOnlySet{T}"/>, so callers must ensure that source
/// is not mutated while the read-only view is in use.
/// </summary>
/// <typeparam name="T">The category value type.</typeparam>
[CollectionBuilder(typeof(ReadOnlyCategorySet), nameof(ReadOnlyCategorySet.Create))]
public sealed class ReadOnlyCategorySet<T> : ReadOnlySet<T>, IReadOnlyCategorySet<T>
    where T : IEquatable<T>
{
    /// <summary>The shared empty read-only category set.</summary>
    public static new readonly ReadOnlyCategorySet<T> Empty = new(new HashSet<T>());

    /// <summary>Creates an empty read-only category set.</summary>
    public ReadOnlyCategorySet()
    {
    }

    /// <summary>Creates a read-only category set seeded from <paramref name="set"/>.</summary>
    public ReadOnlyCategorySet(IEnumerable<T> set) : base(set)
    {
    }

    /// <summary>Creates a read-only view over <paramref name="set"/>.</summary>
    public ReadOnlyCategorySet(IReadOnlySet<T> set) : base(set)
    {
    }

    /// <summary>
    /// Creates a mutable <see cref="CategorySet{T}"/> by copying this read-only instance.
    /// </summary>
    public CategorySet<T> ToCategorySet()
    {
#pragma warning disable IDE0028 // Simplify collection initialization
        return new(this);
#pragma warning restore IDE0028 // Simplify collection initialization
    }
}
