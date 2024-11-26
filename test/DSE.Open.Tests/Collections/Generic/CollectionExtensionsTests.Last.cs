// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.


// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Collections.Generic;

public partial class CollectionExtensionsTestsLast
{
    [Fact]
    public void Last_IReadOnlyList_WithElements_ReturnsLast()
    {
        IReadOnlyList<int> list = [1, 2, 3, 4, 5, 6];
        Assert.Equal(6, list.Last());
    }

    [Fact]
    public void Last_IReadOnlyList_NoElements_ThrowsInvalidOperationException()
    {
        IReadOnlyList<int> list = [];
        _ = Assert.Throws<InvalidOperationException>(() => list.Last());
    }

    [Fact]
    public void LastOrDefault_IReadOnlyList_WithElements_ReturnsLast()
    {
        IReadOnlyList<int> list = [1, 2, 3, 4, 5, 6];
        Assert.Equal(6, list.LastOrDefault());
    }

    [Fact]
    public void LastOrDefault_IReadOnlyList_NoElements_ReturnsDefaultValue()
    {
        IReadOnlyList<int> list = [];
        Assert.Equal(0, list.LastOrDefault());
    }
}
