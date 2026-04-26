// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Numerics;

/// <summary>
/// Type-erased read-only contract over a category set. The non-generic interface
/// is implemented by both <see cref="CategorySet{T}"/> and
/// <see cref="ReadOnlyCategorySet{T}"/> so the metadata can be queried without
/// knowing the element type.
/// </summary>
public interface IReadOnlyCategorySet
{
    /// <summary>Gets <see langword="true"/> when the set contains no elements.</summary>
    bool IsEmpty { get; }
}

/// <summary>
/// A read-only set of distinct category values of type <typeparamref name="T"/>.
/// </summary>
/// <typeparam name="T">The category element type.</typeparam>
public interface IReadOnlyCategorySet<T> : IReadOnlyCategorySet, IReadOnlySet<T>
    where T : IEquatable<T>
{

}
