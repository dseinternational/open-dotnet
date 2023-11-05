// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Specifications;

public sealed class IsFalseAsyncSpecification : IAsyncSpecification<bool>
{
    public static readonly IsFalseAsyncSpecification Instance = new();

    public Task<bool> IsSatisfiedByAsync(bool candidate, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(!candidate);
    }
}
