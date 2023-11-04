// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Specifications;

public class AllSatisfiedSpecificationTests
{
    [Fact]
    public async Task IsSatisfiedIfAllOfManySatisfied()
    {
        ISpecification<bool>[] specs = [new IsTrueSpecification(), new IsTrueSpecification(), new IsTrueSpecification()];
        var all = specs.AsAllSatisfied();
        Assert.True(await all.IsSatisfiedByAsync(true));
    }

    [Fact]
    public async Task IsNotSatisfiedIfOneOfManySatisfied()
    {
        ISpecification<bool>[] specs = [new IsTrueSpecification(), new IsFalseSpecification(), new IsFalseSpecification()];
        var all = specs.AsAllSatisfied();
        Assert.False(await all.IsSatisfiedByAsync(true));
    }
}
