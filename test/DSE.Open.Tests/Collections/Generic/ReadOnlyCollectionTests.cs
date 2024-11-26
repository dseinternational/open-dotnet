// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.


// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Collections.Generic;

public class ReadOnlyCollectionTests
{
    [Fact]
    public void CreateFromArray()
    {
        int[] items = [1, 2, 3, 4, 5, 6];
        var collection = new ReadOnlyCollection<int>(items);
        Assert.Equal(6, collection.Count);
    }

    [Fact]
    public void CreateFromInitialiser()
    {
        ReadOnlyCollection<int> collection = [1, 2, 3, 4, 5, 6];
        Assert.Equal(6, collection.Count);
    }

    [Fact]
    public void IndexerGet()
    {
        ReadOnlyCollection<int> collection = [1, 2, 3, 4, 5, 6];
        Assert.Equal(1, collection[0]);
        Assert.Equal(2, collection[1]);
        Assert.Equal(3, collection[2]);
        Assert.Equal(4, collection[3]);
        Assert.Equal(5, collection[4]);
        Assert.Equal(6, collection[5]);
    }

    // TODO: complete

}
