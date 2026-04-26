// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Runtime.CompilerServices;
using DSE.Open.Collections.Generic;

namespace DSE.Open.Numerics;

/// <summary>Static factories for <see cref="CategorySet{T}"/>.</summary>
public static class CategorySet
{
    /// <summary>Creates a category set wrapping <paramref name="set"/>.</summary>
    public static CategorySet<T> Create<T>(ISet<T> set)
        where T : IEquatable<T>
    {
        return new CategorySet<T>(set);
    }

    /// <summary>Collection-initializer-friendly factory; copies <paramref name="span"/> into a fresh array.</summary>
    public static CategorySet<T> Create<T>(ReadOnlySpan<T> span)
        where T : IEquatable<T>
    {
        return new CategorySet<T>(span.ToArray());
    }
}

/// <summary>
/// The set of allowed values for a categorical <see cref="Series{T}"/>.
/// </summary>
/// <remarks>
/// A <see cref="CategorySet{T}"/> is a fully mutable <see cref="ISet{T}"/>. When a
/// set is attached to a <see cref="Series{T}"/>, the series validates its elements
/// against the set at construction and on indexer assignment, but it does
/// <b>not</b> subscribe to changes. Mutating the set after it has been attached can
/// therefore leave a series holding values that are no longer members of the set.
/// Treat a set attached to any <see cref="Series{T}"/> as effectively read-only, or
/// pass <c>copy: true</c> to <see cref="Series{T}.Slice(int, int, bool)"/> to take
/// an isolated copy.
/// </remarks>
[CollectionBuilder(typeof(CategorySet), nameof(CategorySet.Create))]
public sealed class CategorySet<T> : Set<T>, ICategorySet<T>
    where T : IEquatable<T>
{
    /// <summary>Creates an empty category set.</summary>
    public CategorySet()
    {
    }

    /// <summary>Creates a category set seeded with the elements of <paramref name="set"/> (deduplicated).</summary>
    public CategorySet(IEnumerable<T> set) : base(set)
    {
    }

    /// <summary>Creates a category set wrapping the elements of <paramref name="set"/>.</summary>
    public CategorySet(ISet<T> set) : base(set)
    {
    }

    /// <summary>Returns a read-only snapshot of this category set.</summary>
    public new ReadOnlyCategorySet<T> AsReadOnly()
    {
        return new ReadOnlyCategorySet<T>(this);
    }
}
