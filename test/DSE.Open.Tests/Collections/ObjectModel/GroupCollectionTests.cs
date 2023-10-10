// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Collections.ObjectModel;

namespace DSE.Open.Tests.Collections.ObjectModel;

public class GroupCollectionTests
{
    [Fact]
    public void Constructor_Assigns_Elements()
    {
        var grouping1 = new Grouping<string, string>("Test 1", ["One", "Two"]);
        var grouping2 = new Grouping<string, string>("Test 2", ["Three", "Four"]);
        var collection = new GroupCollection<string, string, Grouping<string, string>>([grouping1, grouping2]);
        Assert.Same(grouping1, collection[0]);
        Assert.Same(grouping2, collection[1]);
    }
}
