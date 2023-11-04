// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Specifications;

internal sealed class AllSatisfiedSpecification<T> : AggregateSpecification<T>
{
    public AllSatisfiedSpecification(IEnumerable<ISpecification<T>> specifications) : base(specifications)
    {
    }

    public override async ValueTask<bool> IsSatisfiedByAsync(T candidate, CancellationToken cancellationToken = default)
    {
        return (await Task.WhenAll(Specifications
            .Select(s => s.IsSatisfiedByAsync(candidate, cancellationToken).AsTask()))
            .ConfigureAwait(false))
            .All(r => r);
    }
}
