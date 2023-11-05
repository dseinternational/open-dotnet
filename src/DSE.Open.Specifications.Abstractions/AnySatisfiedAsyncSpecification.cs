// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Specifications;

internal sealed class AnySatisfiedAsyncSpecification<T> : AggregateAsyncSpecification<T>
{
    public AnySatisfiedAsyncSpecification(IEnumerable<IAsyncSpecification<T>> specifications, bool asParallel = false) : base(specifications)
    {
    }

    public override async Task<bool> IsSatisfiedByAsync(T candidate, CancellationToken cancellationToken = default)
    {
        using var cancellationSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

        var coll = Specifications.Select(s => s.IsSatisfiedByAsync(candidate, cancellationSource.Token)).ToList();

        while (coll.Count != 0)
        {
            var next = await Task.WhenAny(coll).ConfigureAwait(false);

#pragma warning disable CA1849 // Call async methods when in an async method
            if (next.Result)
#pragma warning restore CA1849 // Call async methods when in an async method
            {
                await cancellationSource.CancelAsync().ConfigureAwait(false);
                return true;
            }

            _ = coll.Remove(next);
        }

        return false;
    }
}
