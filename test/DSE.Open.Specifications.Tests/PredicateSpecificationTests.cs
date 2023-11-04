// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Specifications;

public class PredicateSpecificationTests
{
    [Fact]
    public async Task IsSatisfiedIfPredicateReturnsTrue()
    {
        var spec = new PredicateSpecification<bool>((c) => true);
        Assert.True(await spec.IsSatisfiedByAsync(true));
    }

    [Fact]
    public async Task IsNotSatisfiedIfPredicateReturnsFalse()
    {
        var spec = new PredicateSpecification<bool>((c) => false);
        Assert.False(await spec.IsSatisfiedByAsync(true));
    }
}

