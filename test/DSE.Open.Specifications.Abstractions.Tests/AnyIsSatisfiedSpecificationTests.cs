// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Specifications;

public class AnyIsSatisfiedSpecificationTests
{
    [Fact]
    public void IsSatisfiedIfAllOfManySatisfied()
    {
        ISpecification<bool>[] specs = [new IsTrueSpecification(), new IsTrueSpecification(), new IsTrueSpecification()];
        var any = specs.AsAnySatisfied();
        Assert.True(any.IsSatisfiedBy(true));
    }

    [Fact]
    public void IsSatisfiedIfOneOfManySatisfied()
    {
        ISpecification<bool>[] specs = [new IsTrueSpecification(), new IsFalseSpecification(), new IsFalseSpecification()];
        var any = specs.AsAnySatisfied();
        Assert.True(any.IsSatisfiedBy(true));
    }
}
