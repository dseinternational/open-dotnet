// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Specifications;

public class OrSpecificationTests
{
    [Fact]
    public async Task BothSatisfiedReturnsSatisfied()
    {
        Assert.True(await new IsTrueSpecification().Or(new IsTrueSpecification()).IsSatisfiedByAsync(true));
    }

    [Fact]
    public async Task LeftSatisfiedReturnsSatisfied()
    {
        Assert.True(await new IsTrueSpecification().Or(new IsFalseSpecification()).IsSatisfiedByAsync(true));
    }

    [Fact]
    public async Task RightSatisfiedReturnsSatisfied()
    {
        Assert.True(await new IsFalseSpecification().Or(new IsTrueSpecification()).IsSatisfiedByAsync(true));
    }

    [Fact]
    public async Task NeitherSatisfiedReturnsNotSatisfied()
    {
        Assert.False(await new IsFalseSpecification().Or(new IsFalseSpecification()).IsSatisfiedByAsync(true));
    }
}
