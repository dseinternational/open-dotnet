// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Specifications;

public sealed class IsFalseSpecification : ISpecification<bool>
{
    public static readonly IsFalseSpecification Instance = new();

    public ValueTask<bool> IsSatisfiedByAsync(bool candidate, CancellationToken cancellationToken = default)
    {
        return ValueTask.FromResult(!candidate);
    }
}
