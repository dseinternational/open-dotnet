// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Numerics;

/// <summary>
/// A mutable, untyped set of distinct categories. Use the generic
/// <see cref="ICategorySet{T}"/> when the element type is known at compile time.
/// </summary>
public interface ICategorySet : IReadOnlyCategorySet
{
}

/// <summary>
/// A mutable set of distinct category values of type <typeparamref name="T"/>.
/// Backs categorical data on <see cref="Series{T}"/> and similar structures.
/// </summary>
/// <typeparam name="T">The category element type.</typeparam>
public interface ICategorySet<T>
    : ICategorySet,
      IReadOnlyCategorySet<T>,
      ISet<T>
    where T : IEquatable<T>
{

}
