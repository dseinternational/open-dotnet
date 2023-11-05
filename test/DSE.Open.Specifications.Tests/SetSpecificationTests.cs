// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Specifications;

public class SetSpecificationTests
{
    [Fact]
    public void SetComparisonEqualIsSatisfiedByEqualSets()
    {
        var spec = new SetSpecification<int>([1, 2, 3, 4, 5, 6, 7, 8, 9], Collections.SetComparison.Equal);
        Assert.True(spec.IsSatisfiedBy([1, 2, 3, 4, 5, 6, 7, 8, 9]));
    }

    [Fact]
    public void SetComparisonEqualIsNotSatisfiedByUnequalSets()
    {
        var spec = new SetSpecification<int>([1, 2, 3, 4, 5, 6, 7, 8, 9], Collections.SetComparison.Equal);
        Assert.False(spec.IsSatisfiedBy([1, 2, 3, 4, 5, 6, 7, 8, 0]));
    }

    [Fact]
    public void SetComparisonNotEqualIsNotSatisfiedByEqualSets()
    {
        var spec = new SetSpecification<int>([1, 2, 3, 4, 5, 6, 7, 8, 9], Collections.SetComparison.NotEqual);
        Assert.False(spec.IsSatisfiedBy([1, 2, 3, 4, 5, 6, 7, 8, 9]));
    }

    [Fact]
    public void SetComparisonNotEqualIsSatisfiedByUnequalSets()
    {
        var spec = new SetSpecification<int>([1, 2, 3, 4, 5, 6, 7, 8, 9], Collections.SetComparison.NotEqual);
        Assert.True(spec.IsSatisfiedBy([1, 2, 3, 4, 5, 6, 7, 8, 0]));
    }

    [Fact]
    public void SetComparisonSubsetIsSatisfiedBySubset()
    {
        var spec = new SetSpecification<int>([1, 2, 3, 4, 5, 6, 7, 8, 9], Collections.SetComparison.Subset);
        Assert.True(spec.IsSatisfiedBy([1, 2, 3, 4, 5, 6, 7, 8]));
    }

    [Fact]
    public void SetComparisonSubsetNotSatisfiedByDisjointSet()
    {
        var spec = new SetSpecification<int>([1, 2, 3, 4, 5, 6, 7, 8, 9], Collections.SetComparison.Equal);
        Assert.False(spec.IsSatisfiedBy([10, 11, 12]));
    }
}
