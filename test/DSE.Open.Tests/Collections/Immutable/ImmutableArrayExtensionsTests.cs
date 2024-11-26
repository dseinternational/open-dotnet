// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections.Immutable;
using DSE.Open;

namespace DSE.Open.Collections.Immutable;

public class ImmutableArrayExtensionsTests
{
    [Fact]
    public void ContainsValue()
    {
        var array = ImmutableArray.Create(1, 2, 3, 4, 5);

        Assert.True(array.ContainsValue(4));
        Assert.True(array.ContainsValue(5));
        Assert.False(array.ContainsValue(7));
        Assert.False(array.ContainsValue(8));
    }

    [Fact]
    public void IndexOfValue()
    {
        var array = ImmutableArray.Create(1, 2, 3, 4, 5, 4);

        Assert.Equal(3, array.IndexOfValue(4));
        Assert.Equal(4, array.IndexOfValue(5));
        Assert.Equal(-1, array.IndexOfValue(7));
    }

    [Fact]
    public void LastIndexOfValue()
    {
        var array = ImmutableArray.Create(1, 2, 3, 4, 5, 4);

        Assert.Equal(5, array.LastIndexOfValue(4));
        Assert.Equal(4, array.LastIndexOfValue(5));
        Assert.Equal(-1, array.LastIndexOfValue(7));
    }

    [Fact]
    public void IndexOfAny()
    {
        var array = ImmutableArray.Create(1, 2, 3, 4, 5, 4);

        Assert.Equal(3, array.IndexOfAnyValues([4, 7]));
        Assert.Equal(0, array.IndexOfAnyValues([1, 2, 5]));
        Assert.Equal(4, array.IndexOfAnyValues([9, 8, 5]));
        Assert.Equal(-1, array.IndexOfAnyValues([7, 8]));
    }

    [Fact]
    public void LastIndexOfAny()
    {
        var array = ImmutableArray.Create(1, 2, 3, 4, 5, 4);

        Assert.Equal(5, array.LastIndexOfAnyValues([4, 7]));
        Assert.Equal(4, array.LastIndexOfAnyValues([1, 2, 5]));
        Assert.Equal(4, array.LastIndexOfAnyValues([9, 8, 5]));
        Assert.Equal(-1, array.LastIndexOfAnyValues([7, 8]));
    }

    [Fact]
    public void AllContainAnyValues()
    {
        var array = ImmutableArray.Create(
        [
            ImmutableArray.Create(1, 2, 3, 4, 5, 4),
            ImmutableArray.Create(1, 2, 3, 4, 5, 4),
            ImmutableArray.Create(1, 2, 3, 4, 5, 4),
            ImmutableArray.Create(1, 2, 3, 4, 5, 4),
        ]);

        Assert.True(array.AllContainAnyValues([1]));
        Assert.True(array.AllContainAnyValues([4, 7]));
        Assert.True(array.AllContainAnyValues([8, 9, 5]));
    }

    [Fact]
    public void AllContainAnyValues_2()
    {
        var array = ImmutableArray.Create(
        [
            ImmutableArray.Create(1, 2, 3),
            ImmutableArray.Create(3, 4, 5, 4),
            ImmutableArray.Create(1, 3, 4),
            ImmutableArray.Create(1, 2, 3, 5),
        ]);

        Assert.True(array.AllContainAnyValues([3]));
        Assert.True(array.AllContainAnyValues([3, 7]));
        Assert.True(array.AllContainAnyValues([8, 9, 3]));
    }
}
