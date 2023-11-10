// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Specifications;

public interface IAnnotatedAsyncSpecification<T> : IAsyncSpecification<T>
{
    Task<bool> IsSatisfiedByAsync(
        T candidate,
        SpecificationResultAnnotator annotator,
        CancellationToken cancellationToken = default);
}

