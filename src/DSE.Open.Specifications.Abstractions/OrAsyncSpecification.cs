// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Specifications.Abstractions;

/// <summary>
/// A specification that is satisfied if either of two specifications are satisfied.
/// </summary>
/// <typeparam name="T"></typeparam>
internal sealed class OrAsyncSpecification<T> : IAsyncSpecification<T>
{
    private readonly IAsyncSpecification<T> _left;
    private readonly IAsyncSpecification<T> _right;

    public OrAsyncSpecification(IAsyncSpecification<T> left, IAsyncSpecification<T> right)
    {
        Guard.IsNotNull(left);
        Guard.IsNotNull(right);
        _left = left;
        _right = right;
    }

    public async Task<bool> IsSatisfiedByAsync(T value, CancellationToken cancellationToken=default)
    {
        using var cancellationSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

        var t1 = _left.IsSatisfiedByAsync(value, cancellationToken);
        var t2 = _right.IsSatisfiedByAsync(value, cancellationToken);

        var completed = await Task.WhenAny(t1, t2).ConfigureAwait(false);

        if (await completed.ConfigureAwait(false))
        {
            await cancellationSource.CancelAsync().ConfigureAwait(false);
            return true;
        }

        if (completed == t1)
        {
            return await t2.ConfigureAwait(false);
        }

        return await t1.ConfigureAwait(false);
    }
}
