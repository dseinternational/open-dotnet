// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Results;

/// <summary>
/// A <see cref="ValueResult{T}"/> that provides a reference to an <see cref="IAsyncEnumerable{T}"/>.
/// </summary>
/// <typeparam name="T"></typeparam>
public record CollectionValueAsyncResult<T> : ValueResult<IAsyncEnumerable<T>>
{
}
