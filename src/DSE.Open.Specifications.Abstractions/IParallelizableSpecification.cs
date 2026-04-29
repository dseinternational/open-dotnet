// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Specifications;

/// <summary>
/// Determines if a candidate value satisfies a condition, with support for parallel evaluation.
/// </summary>
/// <typeparam name="TValue">The type of the value to be evaluated.</typeparam>
public interface IParallelizableSpecification<in TValue> : ICancellableSpecification<TValue>
{
    /// <summary>
    /// Determines if the specified value satisfies the specification, evaluating any composed
    /// specifications in parallel using the supplied options.
    /// </summary>
    /// <param name="candidate">The value to evaluate.</param>
    /// <param name="maxDegreeOfParallelism">The maximum number of concurrently executing tasks.</param>
    /// <param name="executionMode">The preferred parallel execution mode.</param>
    /// <param name="mergeOptions">The preferred merge options used to combine partial results.</param>
    /// <param name="cancellationToken">A token that can be used to request cancellation.</param>
    /// <returns><see langword="true"/> if the value satisfies the specification,
    /// otherwise <see langword="false"/>.</returns>
    bool IsSatisfiedBy(
        TValue candidate,
        int maxDegreeOfParallelism,
        ParallelExecutionMode executionMode = default,
        ParallelMergeOptions mergeOptions = default,
        CancellationToken cancellationToken = default);
}
