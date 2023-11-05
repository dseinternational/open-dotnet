// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Specifications;

internal abstract class AggregateSpecification<T> : ISpecification<T>
{
    protected AggregateSpecification(IEnumerable<ISpecification<T>> specifications)
    {
        Specifications = specifications.ToArray();
    }

    protected IReadOnlyList<ISpecification<T>> Specifications { get; }

    public abstract bool IsSatisfiedBy(T candidate);
}
