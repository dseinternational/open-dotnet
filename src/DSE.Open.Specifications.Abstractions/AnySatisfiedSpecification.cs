// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Specifications;

internal sealed class AnySatisfiedSpecification<T> : AggregateSpecification<T>, IParallelizableSpecification<T>
{
    public AnySatisfiedSpecification(IEnumerable<ISpecification<T>> specifications)
        : base(specifications)
    {
    }

    public override bool IsSatisfiedBy(T candidate)
    {
        return Specifications.Any(s => s.IsSatisfiedBy(candidate));
    }

    public bool IsSatisfiedBy(T candidate, CancellationToken cancellationToken)
    {
        return Specifications.Any(s =>
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (s is ICancellableSpecification<T> cancellable)
            {
                return cancellable.IsSatisfiedBy(candidate, cancellationToken);
            }

            return s.IsSatisfiedBy(candidate);
        });
    }

    public bool IsSatisfiedBy(
        T candidate,
        int maxDegreeOfParallelism,
        ParallelExecutionMode executionMode = ParallelExecutionMode.Default,
        ParallelMergeOptions mergeOptions = ParallelMergeOptions.Default,
        CancellationToken cancellationToken = default)
    {
        if (maxDegreeOfParallelism < 2)
        {
            return IsSatisfiedBy(candidate, cancellationToken);
        }

        var enumerable = maxDegreeOfParallelism > 1
            ? cancellationToken != CancellationToken.None
                ? Specifications.AsParallel().WithDegreeOfParallelism(maxDegreeOfParallelism).WithCancellation(cancellationToken)
                : (IEnumerable<ISpecification<T>>)Specifications.AsParallel().WithDegreeOfParallelism(maxDegreeOfParallelism)
            : Specifications.AsEnumerable();

        return enumerable.Any(s =>
        {
            if (s is ICancellableSpecification<T> cancellable)
            {
                return cancellable.IsSatisfiedBy(candidate, cancellationToken);
            }

            return s.IsSatisfiedBy(candidate);
        });
    }
}
