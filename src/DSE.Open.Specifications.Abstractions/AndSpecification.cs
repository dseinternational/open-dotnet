// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Specifications;

/// <summary>
/// A specification that is satisfied if both of two specifications are satisfied.
/// </summary>
/// <typeparam name="T"></typeparam>
internal sealed class AndSpecification<T> : ISpecification<T>
{
    private readonly ISpecification<T> _left;
    private readonly ISpecification<T> _right;

    public AndSpecification(ISpecification<T> left, ISpecification<T> right)
    {
        Guard.IsNotNull(left);
        Guard.IsNotNull(right);

        _left = left;
        _right = right;
    }

    public async ValueTask<bool> IsSatisfiedByAsync(T value, CancellationToken cancellationToken = default)
    {
        var t1 = _left.IsSatisfiedByAsync(value, cancellationToken);
        var t2 = _right.IsSatisfiedByAsync(value, cancellationToken);

        var r1 = await t1.ConfigureAwait(false);
        var r2 = await t2.ConfigureAwait(false);

        return r1 && r2;
    }
}
