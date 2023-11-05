// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Specifications;

public class OrSpecificationTests
{
    [Fact]
    public void BothSatisfiedReturnsSatisfied()
    {
        Assert.True(new IsTrueSpecification().Or(new IsTrueSpecification()).IsSatisfiedBy(true));
    }

    [Fact]
    public void LeftSatisfiedReturnsSatisfied()
    {
        Assert.True(new IsTrueSpecification().Or(new IsFalseSpecification()).IsSatisfiedBy(true));
    }

    [Fact]
    public void RightSatisfiedReturnsSatisfied()
    {
        Assert.True(new IsFalseSpecification().Or(new IsTrueSpecification()).IsSatisfiedBy(true));
    }

    [Fact]
    public void NeitherSatisfiedReturnsNotSatisfied()
    {
        Assert.False(new IsFalseSpecification().Or(new IsFalseSpecification()).IsSatisfiedBy(true));
    }
}
