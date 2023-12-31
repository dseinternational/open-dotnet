// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Specifications;

/// <summary>
/// Determines if a candidate value satisfies a condition.
/// </summary>
/// <typeparam name="TValue">The type of the value to be evaluated.</typeparam>
public interface IAsyncSpecification<in TValue>
{
    /// <summary>
    /// Determines if the specified value satisfies the specification.
    /// </summary>
    /// <param name="candidate">The value to evaluate.</param>
    /// <param name="cancellationToken">A token that can be used to request cancellation.</param>
    /// <returns><see langword="true"/> if the value satisfies the specification,
    /// otherwise <see langword="false"/>.</returns>
    Task<bool> IsSatisfiedByAsync(TValue candidate, CancellationToken cancellationToken = default);
}
