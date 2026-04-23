// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Specifications;

internal sealed class AllSatisfiedSpecification<T> : AggregateSpecification<T>, IParallelizableSpecification<T>
{
    public AllSatisfiedSpecification(IEnumerable<ISpecification<T>> specifications)
        : base(specifications)
    {
    }

    public override bool IsSatisfiedBy(T candidate)
    {
        return Specifications.All(s => s.IsSatisfiedBy(candidate));
    }

    public bool IsSatisfiedBy(T candidate, CancellationToken cancellationToken)
    {
        return Specifications.All(s =>
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (s is ICancellableSpecification<T> cancellable)
            {
                return cancellable.IsSatisfiedBy(candidate, cancellationToken);
            }

            return s.IsSatisfiedBy(candidate);
        });
    }

    /// <summary>
    /// Determines whether the specified candidate is satisfied by all of the specifications in this aggregate.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="maxDegreeOfParallelism"/> is less than 1.</exception>
    public bool IsSatisfiedBy(
        T candidate,
        int maxDegreeOfParallelism,
        ParallelExecutionMode executionMode = ParallelExecutionMode.Default,
        ParallelMergeOptions mergeOptions = ParallelMergeOptions.Default,
        CancellationToken cancellationToken = default)
    {
        ArgumentOutOfRangeException.ThrowIfEqual(maxDegreeOfParallelism, 0);
        ArgumentOutOfRangeException.ThrowIfNegative(maxDegreeOfParallelism);

        if (maxDegreeOfParallelism < 2)
        {
            return IsSatisfiedBy(candidate, cancellationToken);
        }

        var query = Specifications.AsParallel()
            .WithDegreeOfParallelism(maxDegreeOfParallelism)
            .WithExecutionMode(executionMode)
            .WithMergeOptions(mergeOptions);

        if (cancellationToken != CancellationToken.None)
        {
            query = query.WithCancellation(cancellationToken);
        }

        return query.All(s =>
        {
            if (s is ICancellableSpecification<T> cancellable)
            {
                return cancellable.IsSatisfiedBy(candidate, cancellationToken);
            }

            return s.IsSatisfiedBy(candidate);
        });
    }
}
