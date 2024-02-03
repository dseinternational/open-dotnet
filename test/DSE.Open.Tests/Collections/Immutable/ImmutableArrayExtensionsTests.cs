// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections.Immutable;
using DSE.Open.Collections.Immutable;

namespace DSE.Open.Tests.Collections.Immutable;

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

        Assert.Equal(3, array.IndexOfAny([4, 7]));
        Assert.Equal(0, array.IndexOfAny([1, 2, 5]));
        Assert.Equal(4, array.IndexOfAny([9, 8, 5]));
        Assert.Equal(-1, array.IndexOfAny([7, 8]));
    }

    [Fact]
    public void LastIndexOfAny()
    {
        var array = ImmutableArray.Create(1, 2, 3, 4, 5, 4);

        Assert.Equal(5, array.LastIndexOfAny([4, 7]));
        Assert.Equal(4, array.LastIndexOfAny([1, 2, 5]));
        Assert.Equal(4, array.LastIndexOfAny([9, 8, 5]));
        Assert.Equal(-1, array.LastIndexOfAny([7, 8]));
    }
}
