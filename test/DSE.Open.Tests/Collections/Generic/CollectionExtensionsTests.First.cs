// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.


// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Collections.Generic;

public partial class CollectionExtensionsTestsFirst
{
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
