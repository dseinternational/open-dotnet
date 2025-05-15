// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Testing.Xunit;

namespace DSE.Open.Numerics;

public class CategorySetTests : LoggedTestsBase
{
    public CategorySetTests(ITestOutputHelper output) : base(output)
    {
    }

    private static CategorySet<int> Empty => [];
    private static CategorySet<int> NonEmpty => [1];

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
public class ReadOnlyCategorySetTests : LoggedTestsBase
{
    public ReadOnlyCategorySetTests(ITestOutputHelper output) : base(output)
    {
    }

    private static ReadOnlyCategorySet<int> Empty => [];
    private static ReadOnlyCategorySet<int> NonEmpty => [1];

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
