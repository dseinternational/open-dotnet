// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Specifications;

internal abstract class AggregateAsyncSpecification<T> : IAsyncSpecification<T>
{
    protected AggregateAsyncSpecification(IEnumerable<IAsyncSpecification<T>> specifications)
    {
        Specifications = specifications.ToArray();
    }

    protected IReadOnlyList<IAsyncSpecification<T>> Specifications { get; }

    public abstract Task<bool> IsSatisfiedByAsync(T candidate, CancellationToken cancellationToken = default);
}
