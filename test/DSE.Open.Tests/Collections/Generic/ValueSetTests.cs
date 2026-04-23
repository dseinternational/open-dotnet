// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Collections.Generic;

public class ValueSetTests
{
    private static readonly int[] s_seedItems = [1, 2, 3];
    private static readonly int[] s_seedItemsWithDuplicates = [1, 2, 3, 2, 1];

    [Fact]
    public void AddStoresItemInBackingSet()
    {
        var set = new ValueSet<int>();

        var added = set.Add(42);

        Assert.True(added);
        _ = Assert.Single(set);
        Assert.Contains(42, set);
    }

    [Fact]
    public void AddReturnsFalseForDuplicate()
    {
        var set = new ValueSet<int>();
        _ = set.Add(1);

        var addedAgain = set.Add(1);

        Assert.False(addedAgain);
        _ = Assert.Single(set);
    }

    [Fact]
    public void EnumerableConstructorCopiesItems()
    {
        var set = new ValueSet<int>(s_seedItemsWithDuplicates);

        Assert.Equal(3, set.Count);
        Assert.Contains(1, set);
        Assert.Contains(2, set);
        Assert.Contains(3, set);
    }

    [Fact]
    public void EnumerableConstructorFromValueSetSharesBackingSet()
    {
        var source = new ValueSet<int>(s_seedItems);

        var copy = new ValueSet<int>(source);

        Assert.Equal(3, copy.Count);
        Assert.Contains(1, copy);
        Assert.Contains(2, copy);
        Assert.Contains(3, copy);
    }
}
