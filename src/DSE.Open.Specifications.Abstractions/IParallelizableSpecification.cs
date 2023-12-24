// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Specifications;

public interface IParallelizableSpecification<in TValue> : ICancellableSpecification<TValue>
{
    bool IsSatisfiedBy(
        TValue candidate,
        int maxDegreeOfParallelism,
        ParallelExecutionMode executionMode = default,
        ParallelMergeOptions mergeOptions = default,
        CancellationToken cancellationToken = default);
}
