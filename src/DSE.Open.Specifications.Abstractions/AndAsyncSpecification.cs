// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Specifications.Abstractions;

/// <summary>
/// A specification that is satisfied if both of two specifications are satisfied.
/// </summary>
/// <typeparam name="T"></typeparam>
internal sealed class AndAsyncSpecification<T> : IAsyncSpecification<T>
{
    private readonly IAsyncSpecification<T> _left;
    private readonly IAsyncSpecification<T> _right;

    public AndAsyncSpecification(IAsyncSpecification<T> left, IAsyncSpecification<T> right)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);

        _left = left;
        _right = right;
    }

    public async Task<bool> IsSatisfiedByAsync(T value, CancellationToken cancellationToken = default)
    {
        var t1 = _left.IsSatisfiedByAsync(value, cancellationToken);
        var t2 = _right.IsSatisfiedByAsync(value, cancellationToken);

        var r1 = await t1.ConfigureAwait(false);
        var r2 = await t2.ConfigureAwait(false);

        return r1 && r2;
    }
}
