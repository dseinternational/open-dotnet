// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Specifications;

public class AllSatisfiedAsyncSpecificationTests
{
    [Fact]
    public async Task IsSatisfiedIfAllOfManySatisfied()
    {
        IAsyncSpecification<bool>[] specs = [new IsTrueAsyncSpecification(), new IsTrueAsyncSpecification(), new IsTrueAsyncSpecification()];
        var all = specs.AsAllSatisfied();
        var satisfied = await all.IsSatisfiedByAsync(true, TestContext.Current.CancellationToken);
        Assert.True(satisfied);
    }

    [Fact]
    public async Task IsNotSatisfiedIfOneOfManySatisfied()
    {
        IAsyncSpecification<bool>[] specs = [new IsTrueAsyncSpecification(), new IsFalseAsyncSpecification(), new IsFalseAsyncSpecification()];
        var all = specs.AsAllSatisfied();
        var satisfied = await all.IsSatisfiedByAsync(true, TestContext.Current.CancellationToken);
        Assert.False(satisfied);
    }
}
