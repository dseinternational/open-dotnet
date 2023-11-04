// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Specifications;
/// <summary>
/// A specification that is satisfied if either of two specifications are satisfied. Does not guarantee the order of evaluation.
/// </summary>
/// <typeparam name="T"></typeparam>
internal sealed class OrSpecification<T> : ISpecification<T>
{
    private readonly ISpecification<T> _left;
    private readonly ISpecification<T> _right;

    public OrSpecification(ISpecification<T> left, ISpecification<T> right)
    {
        Guard.IsNotNull(left);
        Guard.IsNotNull(right);

        _left = left;
        _right = right;
    }

    public async ValueTask<bool> IsSatisfiedByAsync(T value, CancellationToken cancellationToken = default)
    {
        using var cancellationSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

        var t1 = _left.IsSatisfiedByAsync(value, cancellationToken).AsTask();
        var t2 = _right.IsSatisfiedByAsync(value, cancellationToken).AsTask();

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
