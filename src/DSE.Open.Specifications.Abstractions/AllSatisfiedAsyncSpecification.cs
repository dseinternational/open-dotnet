// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Specifications;

internal sealed class AllSatisfiedAsyncSpecification<T> : AggregateAsyncSpecification<T>
{
    public AllSatisfiedAsyncSpecification(IEnumerable<IAsyncSpecification<T>> specifications) : base(specifications)
    {
    }

    public override async Task<bool> IsSatisfiedByAsync(T candidate, CancellationToken cancellationToken = default)
    {
        return (await Task.WhenAll(Specifications
            .Select(s => s.IsSatisfiedByAsync(candidate, cancellationToken)))
            .ConfigureAwait(false))
            .All(r => r);
    }
}
