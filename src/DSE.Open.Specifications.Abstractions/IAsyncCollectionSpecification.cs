// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Specifications;

/// <summary>
/// Determines if a candidate collection of values satisfies a condition.
/// </summary>
/// <typeparam name="TValue">The type of the value in the collection to be evaluated.</typeparam>
public interface IAsyncCollectionSpecification<in TValue> : IAsyncSpecification<IEnumerable<TValue>>
{
}
