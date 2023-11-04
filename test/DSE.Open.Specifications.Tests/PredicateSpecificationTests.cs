// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Specifications;

public class PredicateSpecificationTests
{
    [Fact]
    public void IsSatisfiedIfPredicateReturnsTrue()
    {
        var spec = new PredicateSpecification<bool>((c) => true);
        Assert.True(spec.IsSatisfiedBy(true));
    }

    [Fact]
    public void IsNotSatisfiedIfPredicateReturnsFalse()
    {
        var spec = new PredicateSpecification<bool>((c) => false);
        Assert.False(spec.IsSatisfiedBy(true));
    }
}
