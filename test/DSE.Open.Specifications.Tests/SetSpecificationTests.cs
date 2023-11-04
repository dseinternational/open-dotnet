// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Specifications;

public class SetSpecificationTests
{
    [Fact]
    public async Task SetComparisonEqualIsSatisfiedByEqualSets()
    {
        var spec = new SetSpecification<int>([1, 2, 3, 4, 5, 6, 7, 8, 9], Collections.SetComparison.Equal);
        Assert.True(await spec.IsSatisfiedByAsync([1, 2, 3, 4, 5, 6, 7, 8, 9]));
    }

    [Fact]
    public async Task SetComparisonEqualIsNotSatisfiedByUnequalSets()
    {
        var spec = new SetSpecification<int>([1, 2, 3, 4, 5, 6, 7, 8, 9], Collections.SetComparison.Equal);
        Assert.False(await spec.IsSatisfiedByAsync([1, 2, 3, 4, 5, 6, 7, 8, 0]));
    }

    [Fact]
    public async Task SetComparisonNotEqualIsNotSatisfiedByEqualSets()
    {
        var spec = new SetSpecification<int>([1, 2, 3, 4, 5, 6, 7, 8, 9], Collections.SetComparison.NotEqual);
        Assert.False(await spec.IsSatisfiedByAsync([1, 2, 3, 4, 5, 6, 7, 8, 9]));
    }

    [Fact]
    public async Task SetComparisonNotEqualIsSatisfiedByUnequalSets()
    {
        var spec = new SetSpecification<int>([1, 2, 3, 4, 5, 6, 7, 8, 9], Collections.SetComparison.NotEqual);
        Assert.True(await spec.IsSatisfiedByAsync([1, 2, 3, 4, 5, 6, 7, 8, 0]));
    }

    [Fact]
    public async Task SetComparisonSubsetIsSatisfiedBySubset()
    {
        var spec = new SetSpecification<int>([1, 2, 3, 4, 5, 6, 7, 8, 9], Collections.SetComparison.Subset);
        Assert.True(await spec.IsSatisfiedByAsync([1, 2, 3, 4, 5, 6, 7, 8]));
    }

    [Fact]
    public async Task SetComparisonSubsetNotSatisfiedByDisjointSet()
    {
        var spec = new SetSpecification<int>([1, 2, 3, 4, 5, 6, 7, 8, 9], Collections.SetComparison.Equal);
        Assert.False(await spec.IsSatisfiedByAsync([10, 11, 12]));
    }
}
