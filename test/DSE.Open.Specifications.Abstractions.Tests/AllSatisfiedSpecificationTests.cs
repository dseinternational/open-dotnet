// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Specifications;

public class AllSatisfiedSpecificationTests
{
    [Fact]
    public void IsSatisfiedIfAllOfManySatisfied()
    {
        ISpecification<bool>[] specs = [new IsTrueSpecification(), new IsTrueSpecification(), new IsTrueSpecification()];
        var all = specs.AsAllSatisfied();
        var satisfied = all.IsSatisfiedBy(true, TestContext.Current.CancellationToken);
        Assert.True(satisfied);
    }

    [Fact]
    public void IsSatisfiedIfAllOfManySatisfiedParallel()
    {
        ISpecification<bool>[] specs = [.. Enumerable.Range(0, 100).Select(_ => new IsTrueSpecification())];
        var all = specs.AsAllSatisfied();
        var satisfied = all.IsSatisfiedBy(true, Environment.ProcessorCount, cancellationToken: TestContext.Current.CancellationToken);
        Assert.True(satisfied);
    }

    [Fact]
    public void IsNotSatisfiedIfOneOfManySatisfied()
    {
        ISpecification<bool>[] specs = [new IsTrueSpecification(), new IsFalseSpecification(), new IsFalseSpecification()];
        var all = specs.AsAllSatisfied();
        var satisfied = all.IsSatisfiedBy(true, TestContext.Current.CancellationToken);
        Assert.False(satisfied);
    }

    [Fact]
    public void IsNotSatisfiedIfOneOfManySatisfiedParallel()
    {
        ISpecification<bool>[] specs = [new IsTrueSpecification(), .. Enumerable.Range(0, 100).Select(_ => new IsFalseSpecification())];
        var all = specs.AsAllSatisfied();
        var satisfied = all.IsSatisfiedBy(true, Environment.ProcessorCount, cancellationToken: TestContext.Current.CancellationToken);
        Assert.False(satisfied);
    }
}
