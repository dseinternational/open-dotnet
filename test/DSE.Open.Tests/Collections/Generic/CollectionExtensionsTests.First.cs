// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Collections.Generic;

namespace DSE.Open.Tests.Collections.Generic;

public partial class CollectionExtensionsTestsFirst
{
    [Fact]
    public void First_IList_WithElements_ReturnsFirst()
    {
        IList<int> list = [1, 2, 3, 4, 5, 6];
        Assert.Equal(1, list.First());
    }

    [Fact]
    public void First_IList_NoElements_ThrowsInvalidOperationException()
    {
        IList<int> list = [];
        _ = Assert.Throws<InvalidOperationException>(() => list.First());
    }

    [Fact]
    public void FirstOrDefault_IList_WithElements_ReturnsFirst()
    {
        IList<int> list = [1, 2, 3, 4, 5, 6];
        Assert.Equal(1, list.FirstOrDefault());
    }

    [Fact]
    public void FirstOrDefault_IList_NoElements_ReturnsDefaultValue()
    {
        IList<int> list = [];
        Assert.Equal(0, list.FirstOrDefault());
    }

    [Fact]
    public void First_IReadOnlyList_WithElements_ReturnsFirst()
    {
        IReadOnlyList<int> list = [1, 2, 3, 4, 5, 6];
        Assert.Equal(1, list.First());
    }

    [Fact]
    public void First_IReadOnlyList_NoElements_ThrowsInvalidOperationException()
    {
        IReadOnlyList<int> list = [];
        _ = Assert.Throws<InvalidOperationException>(() => list.First());
    }

    [Fact]
    public void FirstOrDefault_IReadOnlyList_WithElements_ReturnsFirst()
    {
        IReadOnlyList<int> list = [1, 2, 3, 4, 5, 6];
        Assert.Equal(1, list.FirstOrDefault());
    }

    [Fact]
    public void FirstOrDefault_IReadOnlyList_NoElements_ReturnsDefaultValue()
    {
        IReadOnlyList<int> list = [];
        Assert.Equal(0, list.FirstOrDefault());
    }
}
