// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Specifications;

public class AndSpecificationTests
{
    [Fact]
    public async Task BothSatisfiedReturnsSatisfied()
    {
        Assert.True(await new IsTrueSpecification().And(new IsTrueSpecification()).IsSatisfiedByAsync(true));
    }

    [Fact]
    public async Task LeftSatisfiedReturnsNotSatisfied()
    {
        Assert.False(await new IsTrueSpecification().And(new IsFalseSpecification()).IsSatisfiedByAsync(true));
    }

    [Fact]
    public async Task RightSatisfiedReturnsNotSatisfied()
    {
        Assert.False(await new IsFalseSpecification().And(new IsTrueSpecification()).IsSatisfiedByAsync(true));
    }

    [Fact]
    public async Task NeitherSatisfiedReturnsNotSatisfied()
    {
        Assert.False(await new IsFalseSpecification().And(new IsFalseSpecification()).IsSatisfiedByAsync(true));
    }
}
