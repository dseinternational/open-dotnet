// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Specifications;

/// <summary>
/// A specification whose evaluation is delegated to a supplied predicate function.
/// </summary>
/// <typeparam name="T">The type of the value to be evaluated.</typeparam>
public sealed class PredicateSpecification<T> : ISpecification<T>
{
    private readonly Func<T, bool> _predicate;

    /// <summary>
    /// Initializes a new instance using the specified predicate to evaluate candidates.
    /// </summary>
    /// <param name="predicate">The predicate that determines whether a candidate satisfies the specification.</param>
    /// <exception cref="ArgumentNullException"><paramref name="predicate"/> is <see langword="null"/>.</exception>
    public PredicateSpecification(Func<T, bool> predicate)
    {
        ArgumentNullException.ThrowIfNull(predicate);
        _predicate = predicate;
    }

    /// <summary>
    /// Determines if the specified value satisfies the specification by invoking the configured predicate.
    /// </summary>
    /// <param name="candidate">The value to evaluate.</param>
    /// <returns><see langword="true"/> if the predicate returns <see langword="true"/> for the candidate,
    /// otherwise <see langword="false"/>.</returns>
    public bool IsSatisfiedBy(T candidate)
    {
        return _predicate(candidate);
    }
}
