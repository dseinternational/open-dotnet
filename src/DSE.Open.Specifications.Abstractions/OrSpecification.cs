// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Specifications;

/// <summary>
/// A specification that is satisfied if either of two specifications are satisfied.
/// </summary>
/// <typeparam name="T"></typeparam>
internal sealed class OrSpecification<T> : ISpecification<T>
{
    private readonly ISpecification<T> _left;
    private readonly ISpecification<T> _right;

    public OrSpecification(ISpecification<T> left, ISpecification<T> right)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);
        _left = left;
        _right = right;
    }

    public bool IsSatisfiedBy(T value)
    {
        return _left.IsSatisfiedBy(value) || _right.IsSatisfiedBy(value);
    }
}
