// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Specifications;

public class AndSpecificationTests
{
    [Fact]
    public void BothSatisfiedReturnsSatisfied()
    {
        Assert.True(new IsTrueSpecification().And(new IsTrueSpecification()).IsSatisfiedBy(true));
    }

    [Fact]
    public void LeftSatisfiedReturnsNotSatisfied()
    {
        Assert.False(new IsTrueSpecification().And(new IsFalseSpecification()).IsSatisfiedBy(true));
    }

    [Fact]
    public void RightSatisfiedReturnsNotSatisfied()
    {
        Assert.False(new IsFalseSpecification().And(new IsTrueSpecification()).IsSatisfiedBy(true));
    }

    [Fact]
    public void NeitherSatisfiedReturnsNotSatisfied()
    {
        Assert.False(new IsFalseSpecification().And(new IsFalseSpecification()).IsSatisfiedBy(true));
    }
}
