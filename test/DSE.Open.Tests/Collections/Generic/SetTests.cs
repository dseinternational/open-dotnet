// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Collections.Generic;

public class SetTests
{
    [Fact]
    public void Create()
    {
        Set<int> set = [1, 2, 3, 4, 5, 6, 7, 8, 9];
        Assert.Equal(9, set.Count);
        Enumerable.Range(1, 9).ForEach(i => Assert.Contains(i, set));
    }

    private static Set<int> Empty => [];
    private static Set<int> NonEmpty => [1];

    [Fact]  // ∅ ⊆ ∅  → true
    public void Empty_IsSubsetOf_Empty()
    {
        Assert.True(Empty.IsSubsetOf(Empty));
    }

    [Fact]  // ∅ ⊆ {x}  → true
    public void Empty_IsSubsetOf_NonEmpty()
    {
        Assert.True(Empty.IsSubsetOf(NonEmpty));
    }

    [Fact]  // ∅ ⊂ ∅  → false
    public void Empty_IsProperSubsetOf_Empty()
    {
        Assert.False(Empty.IsProperSubsetOf(Empty));
    }

    [Fact]  // ∅ ⊂ {x}  → true
    public void Empty_IsProperSubsetOf_NonEmpty()
    {
        Assert.True(Empty.IsProperSubsetOf(NonEmpty));
    }

    [Fact]  // ∅ ⊇ ∅  → true
    public void Empty_IsSupersetOf_Empty()
    {
        Assert.True(Empty.IsSupersetOf(Empty));
    }

    [Fact]  // ∅ ⊇ {x}  → false
    public void Empty_IsSupersetOf_NonEmpty()
    {
        Assert.False(Empty.IsSupersetOf(NonEmpty));
    }

    [Fact]  // ∅ ⊃ ∅  → false
    public void Empty_IsProperSupersetOf_Empty()
    {
        Assert.False(Empty.IsProperSupersetOf(Empty));
    }

    [Fact]  // ∅ ⊃ {x}  → false
    public void Empty_IsProperSupersetOf_NonEmpty()
    {
        Assert.False(Empty.IsProperSupersetOf(NonEmpty));
    }
}
